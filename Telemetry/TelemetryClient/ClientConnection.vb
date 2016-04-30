Imports System.Net.Sockets
Imports System.Collections.Generic
Imports System.Collections.Concurrent

Class ClientConnection
    Inherits ConnectionBase

    Private connectTimer As Timer
    Private recvTimer As Timer
    Private sendTimer As Timer
    Private closeTimer As Timer

    Private sinceRow As Boolean
    Private sinceAll As Boolean
    Private sinceDate As DateTime
    Private wantAll As Boolean
    Private wantFields As List(Of String)

    Private lastRcvd As Int32
    Private dataQueue As ConcurrentQueue(Of Record)

    '' Init socket, timers, want info.
    Private Sub Init(ByVal fields As List(Of String), ByVal queue As ConcurrentQueue(Of Record))
        ' init socket (IPv4, TCP)
        socket = Nothing
        listener = Nothing

        ' init timouts
        connectTimer = New Timer(My.Settings.ConnectTimer)
        recvTimer = New Timer(My.Settings.ReceiveTimer)
        sendTimer = New Timer(My.Settings.SendTimer)
        closeTimer = New Timer(My.Settings.SendTimer)

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
        Me.New(1, fields, queue)
        sinceAll = True
    End Sub

    '' Constructor for getting all data since startDate.
    Public Sub New(ByVal startDate As DateTime, ByVal fields As List(Of String), ByVal queue As ConcurrentQueue(Of Record))
        ' init since info
        sinceRow = False
        sinceAll = False
        sinceDate = startDate

        ' init other fields
        Init(fields, queue)
    End Sub

    '' Constructor for getting all data since and including startRow.
    Public Sub New(ByVal startRow As Int32, ByVal fields As List(Of String), ByVal queue As ConcurrentQueue(Of Record))
        ' init since info
        sinceRow = True
        lastRcvd = startRow - 1
        sinceDate = Nothing

        ' init other fields
        Init(fields, queue)
    End Sub

    '' Attempts to start a session with the server by sending the CONNECT message.
    '' Returns FAILED (if error) or OK (if message sent successfully).
    Public Function SendConnect() As ConnectionResult
        ' try connect
        If Not Connect() Then
            Return ConnectionResult.FAILED
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
            If sinceAll Then
                message &= "SINCE-ALL"
            Else
                message &= "SINCE-ROW " & (lastRcvd + 1)
            End If
        Else
            message &= "SINCE-DATE " & sinceDate
        End If

        ' terminate message
        message &= vbLf

        ' try send message
        If Not Write(message) Then
            Return ConnectionResult.FAILED
        End If

        ' result ok
        Console.WriteLine("Send connect succeeded.")
        connectTimer.Reset()
        Return ConnectionResult.OK
    End Function

    '' Attempts to read the connection ack from the socket.
    '' Returns NO_DATA (nothing available), FAILED (error), or OK (got ack).
    Public Function ReadConnectAck() As ConnectionResult
        ' try reading data from socket
        Dim words As String() = New String() {}
        Dim result = TryRead(words)
        If result <> ConnectionResult.OK Then
            Return result
        End If

        ' check server response
        If words.Length = 3 AndAlso words(0) = "CONNECT" AndAlso words(1) = "ok" Then
            Console.WriteLine("Read connect ACK.")
            recvTimer.Reset()
            sendTimer.Reset()
            lastRcvd = CInt(words(2)) - 1
            Return ConnectionResult.OK
        Else
            Console.WriteLine("Failed to read connect ACK.")
            Return ConnectionResult.FAILED
        End If
    End Function

    '' Attempts to read data from the server (which may be DATA or KEEP-ALIVE).
    '' Returns NO_DATA (nothing available), FAILED (error), OK (got data).
    Public Function ReadData() As ConnectionResult
        ' try reading data from socket
        Dim words As String() = New String() {}
        Dim result = TryRead(words)
        If result <> ConnectionResult.OK Then
            Return result
        End If

        ' check server response
        If words(0) = "DATA" Then
            dataQueue.Enqueue(ParseData(words))
        ElseIf words(0) <> "KEEP-ALIVE" Then
            Return ConnectionResult.FAILED
        End If

        ' reset recv timer
        Console.WriteLine("Read data succeeded.")
        recvTimer.Reset()
        Return ConnectionResult.OK
    End Function

    '' Attempts to send a KEEP-ALIVE message to the server.
    '' Returns FAILED (error) or OK (sent message).
    Public Function SendKeepAlive() As ConnectionResult
        ' create message
        Dim message As String = "KEEP-ALIVE nusolar" & vbLf

        ' try write
        If Not Write(message) Then
            Return ConnectionResult.FAILED
        End If

        ' reset send timer
        sendTimer.Reset()
        Console.WriteLine("Send keep-alive succeeded.")
        Return ConnectionResult.OK
    End Function

    '' Attempts to send a close message to the server.
    '' Returns FAILED (error) or OK (success).
    Public Function SendClose() As ConnectionResult
        ' create message
        Dim message As String = "CLOSE nusolar" & vbLf

        ' try write
        If Not Write(message) Then
            Return ConnectionResult.FAILED
        End If

        ' set close timer
        closeTimer.Reset()
        Console.WriteLine("Send close succeeded.")
        Return ConnectionResult.OK
    End Function

    '' Attempts to read a close ack from the server.
    '' Returns FAILED (error), NO_DATA (no ack), or OK (read ack).
    Public Function ReadCloseAck() As ConnectionResult
        ' try reading data from socket
        Dim words As String() = New String() {}
        Dim result = TryRead(words)
        If result <> ConnectionResult.OK Then
            Return result
        End If

        ' check server response
        If words.Length = 1 AndAlso words(0) = "CLOSE" Then
            Console.WriteLine("Read close ACK.")
            Return ConnectionResult.OK
        ElseIf words(0) = "DATA" Then
            dataQueue.Enqueue(ParseData(words))
            Return ConnectionResult.NO_DATA
        ElseIf words.Length = 1 AndAlso words(0) = "KEEP-ALIVE" Then
            Return ConnectionResult.NO_DATA
        Else
            Return ConnectionResult.FAILED
        End If
    End Function

    '' Returns true if the connection timer has expired.
    Public Function CheckConnectTimer() As Boolean
        If connectTimer.Elapsed() Then
            Close()
            Return True
        Else
            Return False
        End If
    End Function

    '' Returns true if the receive timer has expired.
    Public Function CheckRecvTimer() As Boolean
        If recvTimer.Elapsed() Then
            Close()
            Return True
        Else
            Return False
        End If
    End Function

    '' Returns true if the send timer has expired.
    Public Function CheckSendTimer() As Boolean
        Return sendTimer.Elapsed()
    End Function

    '' Returns true if the close timer has expired.
    Public Function CheckCloseTimer() As Boolean
        Return closeTimer.Elapsed()
    End Function
End Class
