Imports System.Net
Imports System.Net.Sockets
Imports System.Collections.Generic
Imports System.Collections.Concurrent

Class ServerConnection
    Inherits ConnectionBase

    Private dataReq As DataRequest
    Private dataQueue As ConcurrentQueue(Of Record)

    Private recvTimer As Timer
    Private sendTimer As Timer

    '' Attempts to accept an incoming connection. Will block.
    Private Function Accept() As Boolean
        Try
            ' create listener
            Dim endp As IPEndPoint = New IPEndPoint(IPAddress.Any, My.Settings.Port)
            listener = New Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
            listener.Bind(endp)
            listener.Listen(0)

            ' accept connection
            socket = listener.Accept()
            socket.ReceiveTimeout = My.Settings.SocketReceiveTimeout
            socket.SendTimeout = My.Settings.SocketSendTimeout
        Catch ex As Exception
            Console.WriteLine("Accept failed: " & ex.Message)
            Return False
        End Try

        Console.WriteLine("Accept succeeded")
        Return True
    End Function

    '' Parses a connect message from the client. Returns true if the message
    '' is well formatted, false otherwise.
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

        ' set socket to null
        socket = Nothing
    End Sub

    '' Attempts to read the connect request from the client. 
    '' Returns OK (success) or FAILED (error)
    Public Function ReadConnect() As ConnectionResult
        ' accept incoming connection
        If Not Accept() Then
            Return ConnectionResult.FAILED
        End If

        ' try reading connect request from socket
        Dim response As String = ""
        If Not Read(response) Then
            Return ConnectionResult.FAILED
        End If

        ' check connect request
        Dim words As String() = response.Split().Where(Function(s) s <> String.Empty).ToArray()
        If words.Length > 0 AndAlso words(0) = "CONNECT" Then
            If Not ParseConnect(words) Then
                Console.WriteLine("Invalid connect request.")
                Write("CONNECT bad" & vbLf)
                Return ConnectionResult.FAILED
            End If
        Else
            Console.WriteLine("Failed to read connect.")
            Return ConnectionResult.FAILED
        End If

        ' send connect ACK
        If Not Write("CONNECT ok " & 1 & vbLf) Then
            Console.WriteLine("Failed to send connect ACK.")
            Return ConnectionResult.FAILED
        End If

        ' success
        recvTimer.Reset()
        sendTimer.Reset()
        Console.WriteLine("Connect succeeded.")
        Return ConnectionResult.OK
    End Function

    '' Attempts to send a row of data to the client.
    '' Returns OK (success), NO_DATA (none available), or FAILED (error).
    Public Function SendData() As ConnectionResult
        ' check if data available
        If False Then
            ' crate message
            Dim message As String = "DATA " & Date.Now.ToShortDateString() & " 1" & vbLf

            ' try write
            If Not Write(message) Then
                Return ConnectionResult.FAILED
            End If

            ' done
            Console.WriteLine("Send data succeeded.")
            Return ConnectionResult.OK
        Else
            Return ConnectionResult.NO_DATA
        End If
    End Function

    '' Attempts to read close request or keep-alive message from the client.
    '' Returns OK (read close), NO_DATA (no request or keep-alive), or FAILED (error).
    Public Function ReadMessage() As ConnectionResult
        ' try reading data from socket
        Dim words As String() = New String() {}
        Dim result = TryRead(words)
        If result <> ConnectionResult.OK Then
            Return result
        End If

        ' determine type of message
        If words.Length = 2 AndAlso words(0) = "KEEP-ALIVE" AndAlso words(1) = "nusolar" Then
            Console.WriteLine("Read keep-alive")
            recvTimer.Reset()
            Return ConnectionResult.NO_DATA
        ElseIf words.Length = 2 AndAlso words(0) = "CLOSE" AndAlso words(1) = "nusolar" Then
            Console.WriteLine("Read close request")
            Return ConnectionResult.OK
        End If

        ' unknown message type
        Console.WriteLine("Unknown message received")
        Return ConnectionResult.FAILED
    End Function

    '' Attempts to send a close ACK to the client.
    '' Returns OK (success) or FAILED (error).
    Public Function SendClose() As ConnectionResult
        ' create message
        Dim message As String = "CLOSE" & vbLf

        ' try write
        If Not Write(message) Then
            Return ConnectionResult.FAILED
        End If

        ' done
        Console.WriteLine("Send close succeeded.")
        Return ConnectionResult.OK
    End Function

    '' Attempts to send a KEEP-ALIVE message to the client.
    '' Returns OK (success) or FAILED (error).
    Public Function SendKeepAlive() As ConnectionResult
        ' create message
        Dim message As String = "KEEP-ALIVE" & vbLf

        ' try write
        If Not Write(message) Then
            Return ConnectionResult.FAILED
        End If

        ' reset send timer
        sendTimer.Reset()
        Console.WriteLine("Send keep-alive succeeded.")
        Return ConnectionResult.OK
    End Function

    '' Returns true if the send timer has expired.
    Public Function CheckSendTimer() As Boolean
        Return sendTimer.Elapsed()
    End Function

    '' Returns true if the recv timer has expired.
    Public Function CheckRecvTimer() As Boolean
        If recvTimer.Elapsed() Then
            Close()
            Return True
        Else
            Return False
        End If
    End Function
End Class
