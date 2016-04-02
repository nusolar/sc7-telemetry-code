Imports System.Net.Sockets
Imports System.Collections.Generic
Imports System.Collections.Concurrent

Class Connection
    Private socket As Socket

    Private connectTimeout As DateTime
    Private recvTimeout As DateTime
    Private sendTimeout As DateTime
    Private closeTimeout As DateTime

    Private sinceRow As Boolean
    Private sinceDate As DateTime
    Private wantAll As Boolean
    Private wantFields As List(Of String)

    Private lastRcvd As Int32
    Private dataQueue As ConcurrentQueue(Of Record)

    '' Init socket, timers, want info.
    Private Sub Init(ByVal fields As List(Of String), ByVal queue As ConcurrentQueue(Of Record))
        ' init socket (IPv4, TCP)
        socket = Nothing

        ' init timouts
        connectTimeout = Nothing
        recvTimeout = Nothing
        sendTimeout = Nothing
        closeTimeout = Nothing

        ' init want info
        wantFields = fields
        If fields.Count = 0 Then
            wantAll = True
        Else
            wantAll = False
        End If

        ' init data queue
        dataQueue = queue
    End Sub

    '' Attempts to establish a connection to the server. Returns true on success,
    '' false otherwise.
    Private Function Connect() As Boolean
        ' init socket (IPv4, TCP)
        socket = New Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
        socket.ReceiveTimeout = My.Settings.SocketReceiveTimeout
        socket.SendTimeout = My.Settings.SocketSendTimeout

        ' try connect
        Try
            socket.Connect(My.Settings.ServerAddress, My.Settings.ServerPort)
        Catch ex As SocketException
            Console.WriteLine("Socket connection failed: " & ex.Message)
            Close()
            Return False
        End Try

        ' connect succeeded
        Console.WriteLine("Socket connected.")
        Return True
    End Function

    '' Attempts to read a line from the socket. Will block.
    Private Function Read(ByRef message As String) As Boolean
        Dim buffer(2048) As Byte
        Dim i As Integer = 0
        Dim bytesRead As Integer = 0
        Dim readByte(1) As Byte

        ' try reading
        Try
            While True
                ' read one byte
                bytesRead = socket.Receive(readByte)

                ' if byte actually read add to buffer
                If bytesRead > 0 Then
                    ' add to buffer
                    buffer(i) = readByte(0)

                    ' increment buffer position
                    i += 1

                    ' if newline, end of message
                    If buffer(i) = vbLf Then
                        Exit While
                    End If
                End If
            End While
        Catch ex As SocketException
            Console.WriteLine("Read failed: " & ex.Message)
            Close()
            Return False
        End Try

        ' convert buffer to string
        message = System.Text.Encoding.ASCII.GetString(buffer, 0, i)
        Console.WriteLine("Read {0}", message)
        Return True
    End Function

    '' Attempts to read a line from the socket and then split the line
    '' on whitespace. Will not block (returns if there is no data).
    Private Function TryRead(ByRef words As String()) As ClientResult
        ' check if data available
        If socket.Available <= 0 Then
            Return ClientResult.NO_DATA
        End If

        ' read server response
        Dim response As String = ""
        If Not Read(response) Then
            Return ClientResult.FAILED
        End If

        ' check server response
        words = response.Split().Where(Function(s) s <> String.Empty)
        Return ClientResult.OK
    End Function

    '' Attempts to write the given message to the socket.
    Private Function Write(ByVal message As String) As Boolean
        Dim bytes As Byte() = System.Text.Encoding.ASCII.GetBytes(message)

        ' try write
        Try
            socket.Send(bytes)
        Catch ex As SocketException
            Console.WriteLine("Write failed: " & ex.Message)
            Close()
            Return False
        End Try

        ' write OK
        Console.WriteLine("Wrote {0}", message)
        Return True
    End Function

    '' Closes the socket.
    Private Sub Close()
        Try
            socket.Close()
        Catch ex As SocketException
            Console.WriteLine("Socket close failed: " & ex.Message)
            Return
        End Try

        Console.WriteLine("Socket closed.")
    End Sub

    '' Helper function for parsing a data message.
    Private Function ParseData(ByVal words As String()) As Record
        ' init record
        Dim rec As New Record(DateTime.Parse(words(0)), CInt(words(1)))

        ' put field, value pairs in dict
        For i As Integer = 2 To words.Length Step 2
            rec.data.Add(words(i), words(i + 1))
        Next

        ' done
        Return rec
    End Function

    '' Constructor for getting all data.
    Public Sub New(ByVal fields As List(Of String), ByVal queue As ConcurrentQueue(Of Record))
        Me.New(0, fields, queue)
    End Sub

    '' Constructor for getting all data since startDate.
    Public Sub New(ByVal startDate As DateTime, ByVal fields As List(Of String), ByVal queue As ConcurrentQueue(Of Record))
        ' init since info
        sinceRow = False
        sinceDate = startDate

        ' init other fields
        Init(fields, queue)
    End Sub

    '' Constructor for getting all data since and including startRow.
    Public Sub New(ByVal startRow As Int32, ByVal fields As List(Of String), ByVal queue As ConcurrentQueue(Of Record))
        ' init since info
        sinceRow = True
        sinceDate = Nothing

        ' init other fields
        Init(fields, queue)
    End Sub

    '' Attempts to start a session with the server by sending the CONNECT message.
    '' Returns FAILED (if error) or OK (if message sent successfully).
    Public Function SendConnect() As ClientResult
        ' try connect
        If Not Connect() Then
            Return ClientResult.FAILED
        End If

        ' init message
        Dim message As String = "CONNECT nusolar "

        ' add want info
        If wantAll Then
            message &= "WANT-ALL "
        Else
            message &= "WANT "
            For Each field As String In wantFields
                message &= field & " "
            Next
        End If

        ' add since info
        If sinceRow Then
            message &= "SINCE-ROW " & (lastRcvd + 1)
        Else
            message &= "SINCE-DATE " & sinceDate
        End If

        ' terminate message
        message &= vbCrLf

        ' try send message
        If Not Write(message) Then
            Return ClientResult.FAILED
        End If

        ' result ok
        Console.WriteLine("Send connect succeeded.")
        connectTimeout = Date.Now.AddMilliseconds(My.Settings.ConnectTimer)
        Return ClientResult.OK
    End Function

    '' Attempts to read the connection ack from the socket.
    '' Returns NO_DATA (nothing available), FAILED (error), or OK (got ack).
    Public Function ReadConnectAck() As ClientResult
        ' try reading data from socket
        Dim words As String() = New String() {}
        Dim result = TryRead(words)
        If result <> ClientResult.OK Then
            Return result
        End If

        ' check server response
        If words.Length = 3 AndAlso words(0) = "CONNECT" AndAlso words(1) = "ok" Then
            Console.WriteLine("Read connect ACK.")
            recvTimeout = Date.Now.AddMilliseconds(My.Settings.ReceiveTimer)
            sendTimeout = Date.Now.AddMilliseconds(My.Settings.SendTimer)
            lastRcvd = CInt(words(2)) - 1
            Return ClientResult.OK
        Else
            Console.WriteLine("Failed to read connect ACK.")
            Return ClientResult.FAILED
        End If
    End Function

    '' Returns true if the connection timer has expired.
    Public Function CheckConnectTimer() As Boolean
        Return Date.Now.CompareTo(connectTimeout) >= 0
    End Function

    '' Attempts to read data from the server (which may be DATA or KEEP-ALIVE).
    '' Returns NO_DATA (nothing available), FAILED (error), OK (got data).
    Public Function ReadData() As ClientResult
        ' try reading data from socket
        Dim words As String() = New String() {}
        Dim result = TryRead(words)
        If result <> ClientResult.OK Then
            Return result
        End If

        ' check server response
        If words(0) = "DATA" Then
            dataQueue.Enqueue(ParseData(words))
        ElseIf words(0) <> "KEEP-ALIVE" Then
            Return ClientResult.FAILED
        End If

        ' reset recv timer
        Console.WriteLine("Read data succeeded.")
        recvTimeout = Date.Now.AddMilliseconds(My.Settings.ReceiveTimer)
        Return ClientResult.OK
    End Function

    '' Attempts to send a KEEP-ALIVE message to the server.
    '' Returns FAILED (error) or OK (sent message).
    Public Function SendKeepAlive() As ClientResult
        ' create message
        Dim message As String = "KEEP-ALIVE nusolar" & vbCrLf

        ' try write
        If Not Write(message) Then
            Return ClientResult.FAILED
        End If

        ' reset send timer
        sendTimeout = Date.Now.AddMilliseconds(My.Settings.SendTimer)
        Console.WriteLine("Send keep-alive succeeded.")
        Return ClientResult.OK
    End Function

    Public Function CheckRecvTimer() As Boolean
        Return Date.Now.CompareTo(recvTimeout) >= 0
    End Function

    Public Function CheckSendTimer() As Boolean
        Return Date.Now.CompareTo(sendTimeout) >= 0
    End Function

    Public Function SendClose() As ClientResult
        ' create message
        Dim message As String = "CLOSE nusolar" & vbCrLf

        ' try write
        If Not Write(message) Then
            Return ClientResult.FAILED
        End If

        ' set close timer
        closeTimeout = Date.Now.AddMilliseconds(My.Settings.CloseTimer)
        Console.WriteLine("Send close succeeded.")
        Return ClientResult.OK
    End Function

    Public Function ReadCloseAck() As ClientResult
        ' try reading data from socket
        Dim words As String() = New String() {}
        Dim result = TryRead(words)
        If result <> ClientResult.OK Then
            Return result
        End If

        ' check server response
        If words.Length = 1 AndAlso words(0) = "CLOSE" Then
            Console.WriteLine("Read close ACK.")
            Return ClientResult.OK
        ElseIf words(0) = "DATA" Then
            dataQueue.Enqueue(ParseData(words))
            Return ClientResult.NO_DATA
        ElseIf words.Length = 1 AndAlso words(0) = "KEEP-ALIVE" Then
            Return ClientResult.NO_DATA
        Else
            Return ClientResult.FAILED
        End If
    End Function

    Public Function CheckCloseTimer() As Boolean
        Return Date.Now.CompareTo(closeTimeout) >= 0
    End Function
End Class
