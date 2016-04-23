Imports System.Net
Imports System.Net.Sockets
Imports System.Collections.Generic
Imports System.Collections.Concurrent

Class Connection
    Private listener As Socket
    Private socket As Socket

    Private dataReq As DataRequest
    Private dataQueue As ConcurrentQueue(Of Record)

    Private recvTimer As Timer
    Private sendTimer As Timer

    '' Attempts to accept an incoming connection. Will block.
    Private Function Accept() As Boolean
        Try
            socket = listener.Accept()
            socket.ReceiveTimeout = My.Settings.SocketReceiveTimeout
            socket.SendTimeout = My.Settings.SocketSendTimeout
        Catch ex As Exception
            Console.WriteLine("Accept failed.")
            Return False
        End Try

        Console.WriteLine("Accept succeeded")
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
    Private Function TryRead(ByRef words As String()) As ServerResult
        ' check if data available
        If socket.Available <= 0 Then
            Return ServerResult.NO_DATA
        End If

        ' read client message
        Dim response As String = ""
        If Not Read(response) Then
            Return ServerResult.FAILED
        End If

        ' parse client message
        words = response.Split().Where(Function(s) s <> String.Empty)
        Return ServerResult.OK
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
            listener.Close()
            If socket IsNot Nothing Then
                socket.Close()
            End If
        Catch ex As SocketException
            Console.WriteLine("Socket close failed: " & ex.Message)
            Return
        End Try

        Console.WriteLine("Socket closed.")
    End Sub

    '' Parses a connect message from the client. Returns true if the message
    '' is well formatted. false otherwise.
    Private Function ParseConnect(ByVal words As String()) As Boolean
        Return words.Length = 4 AndAlso words(0) = "CONNECT" AndAlso
            words(1) = "nusolar" AndAlso words(2) = "WANT-ALL" AndAlso
            words(3) = "SINCE-ALL"
    End Function

    '' Constructor for the server's connection. Creates the listen socket.
    Public Sub New(ByVal _dataReq As DataRequest, ByVal _dataQueue As ConcurrentQueue(Of Record))
        ' init request, queue
        dataReq = _dataReq
        dataQueue = _dataQueue

        ' init timeouts
        sendTimer = New Timer(My.Settings.SendTimer)
        recvTimer = New Timer(My.Settings.RecvTimer)

        ' create listener
        Dim endp As IPEndPoint = New IPEndPoint(IPAddress.Any, My.Settings.Port)
        listener = New Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
        Try
            listener.Bind(endp)
            listener.Listen(10)
        Catch ex As Exception
            Console.WriteLine("Failed to create listener: " & ex.Message)
        End Try

        ' set socket to null
        socket = Nothing
    End Sub

    '' Attempts to read the connect request from the client. 
    '' Returns OK (success) or FAILED (error)
    Public Function ReadConnect() As ServerResult
        ' accept incoming connection
        If Not Accept() Then
            Return ServerResult.FAILED
        End If

        ' try reading connect request from socket
        Dim response As String = ""
        If Not Read(response) Then
            Return ServerResult.FAILED
        End If

        ' check connect request
        Dim words As String() = response.Split().Where(Function(s) s <> String.Empty)
        If words.Length > 0 AndAlso words(0) = "CONNECT" Then
            If Not ParseConnect(words) Then
                Console.WriteLine("Invalid connect request.")
                Write("CONNECT bad" & vbCrLf)
                Return ServerResult.FAILED
            End If
        Else
            Console.WriteLine("Failed to read connect.")
            Return ServerResult.FAILED
        End If

        ' send connect ACK
        If Not Write("CONNECT ok " & dataReq.sinceRow & vbCrLf) Then
            Console.WriteLine("Failed to send connect ACK.")
            Return ServerResult.FAILED
        End If

        ' success
        recvTimer.Reset()
        sendTimer.Reset()
        Console.WriteLine("Connect succeeded.")
        Return ServerResult.OK
    End Function

    '' Attempts to send a row of data to the client.
    '' Returns OK (success), NO_DATA (none available), or FAILED (error).
    Public Function SendData() As ServerResult
        ' create message
        Dim message As String = "DATA " & Date.Now.ToShortDateString() & " 1" & vbCrLf

        ' try write
        If Not Write(message) Then
            Return ServerResult.FAILED
        End If

        ' done
        Console.WriteLine("Send data succeeded.")
        Return ServerResult.OK
    End Function

    '' Attempts to read close request or keep-alive message from the client.
    '' Returns OK (read close), NO_DATA (no request or keep-alive), or FAILED (error).
    Public Function ReadMessage() As ServerResult
        ' try reading data from socket
        Dim words As String() = New String() {}
        Dim result = TryRead(words)
        If result <> ServerResult.OK Then
            Return result
        End If

        ' determine type of message
        If words.Length = 2 AndAlso words(0) = "KEEP-ALIVE" AndAlso words(1) = "nusolar" Then
            Console.WriteLine("Read keep-alive")
            recvTimer.Reset()
            Return ServerResult.NO_DATA
        ElseIf words.Length = 2 AndAlso words(0) = "CLOSE" AndAlso words(1) = "nusolar" Then
            Console.WriteLine("Read close request")
            Return ServerResult.OK
        End If

        ' unknown message type
        Console.WriteLine("Unknown message received")
        Return ServerResult.FAILED
    End Function

    '' Attempts to send a close ACK to the client.
    '' Returns OK (success) or FAILED (error).
    Public Function SendClose() As ServerResult
        ' create message
        Dim message As String = "CLOSE" & vbCrLf

        ' try write
        If Not Write(message) Then
            Return ServerResult.FAILED
        End If

        ' done
        Console.WriteLine("Send close succeeded.")
        Return ServerResult.OK
    End Function

    '' Attempts to send a KEEP-ALIVE message to the client.
    '' Returns OK (success) or FAILED (error).
    Public Function SendKeepAlive() As ServerResult
        ' create message
        Dim message As String = "KEEP-ALIVE" & vbCrLf

        ' try write
        If Not Write(message) Then
            Return ServerResult.FAILED
        End If

        ' reset send timer
        sendTimer.Reset()
        Console.WriteLine("Send keep-alive succeeded.")
        Return ServerResult.OK
    End Function

    '' Returns true if the send timer has expired.
    Public Function CheckSendTimer() As Boolean
        Return sendTimer.Elapsed()
    End Function

    '' Returns true if the recv timer has expired.
    Public Function CheckRecvTimer() As Boolean
        Return recvTimer.Elapsed()
    End Function
End Class
