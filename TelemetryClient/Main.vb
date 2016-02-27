Imports System.Net.Sockets
Imports System.Collections.Generic

Module Main
    '-----------------------------TYPE DEFINITIONS-------------------------------'
    Enum ClientState
        CLOSED
        CONNECT_SENT
        CONNECTED
        CLOSE_WAIT
    End Enum

    Class Connection
        Private socket As Socket

        Private recvTimeout As DateTime
        Private sendTimeout As DateTime
        Private closeTimeout As DateTime

        Private sinceRow As Boolean
        Private lastRcvd As Int32
        Private sinceDate As DateTime

        Private wantAll As Boolean
        Private wantFields As List(Of String)

        '' Init socket, timers, want info.
        Private Sub Init(ByVal fields As List(Of String))
            ' init socket (IPv4, TCP)
            socket = Nothing

            ' init timouts
            recvTimeout = Date.Now()
            sendTimeout = Date.Now()
            closeTimeout = Date.Now()

            ' init want info
            wantFields = fields
            If fields.Count = 0 Then
                wantAll = True
            Else
                wantAll = False
            End If
        End Sub

        '' Attempts to establish a connection to the server. Returns true on success,
        '' false otherwise.
        Private Function Connect() As Boolean
            ' init socket (IPv4, TCP)
            socket = New Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
            socket.ReceiveTimeout = My.Settings.ReceiveTimeout
            socket.SendTimeout = My.Settings.SendTimeout

            ' try connect
            Try
                socket.Connect(My.Settings.ServerAddress, My.Settings.ServerPort)
            Catch ex As SocketException
                Console.WriteLine("Connection failed: " & ex.Message)
                Close()
                Return False
            End Try

            ' connect succeeded
            Console.WriteLine("Connection established.")
            Return True
        End Function

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
            Console.WriteLine("Read succeeded.")
            Return True
        End Function

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
            Console.WriteLine("Write succeeded.")
            Return True
        End Function

        Private Sub Close()
            Try
                socket.Close()
            Catch ex As SocketException
                Console.WriteLine("Socket close failed: " & ex.Message)
                Return
            End Try

            Console.WriteLine("Socket closed.")
        End Sub

        '' Constructor for getting all data.
        Public Sub New(ByVal fields As List(Of String))
            Me.New(0, fields)
        End Sub

        '' Constructor for getting all data since startDate.
        Public Sub New(ByVal startDate As DateTime, ByVal fields As List(Of String))
            ' init since info
            sinceRow = False
            lastRcvd = -1
            sinceDate = startDate

            ' init other fields
            Init(fields)
        End Sub

        '' Constructor for getting all data since and including startRow.
        Public Sub New(ByVal startRow As Int32, ByVal fields As List(Of String))
            ' init since info
            sinceRow = True
            lastRcvd = startRow - 1

            ' init other fields
            Init(fields)
        End Sub

        '' Attempts to start a session with the server by sending the CONNECT message.
        '' Returns true on success, false othwerise.
        Public Function StartSession() As Boolean
            ' try connect
            If Not Connect() Then
                Return False
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
                Return False
            End If
            Console.WriteLine("Sent {0}", message)

            ' read server response
            Dim response As String = ""
            If Not Read(response) Then
                Return False
            End If
            Console.WriteLine("Read {0}", response)

            ' check server response
            Dim words As String() = response.Split().Where(Function(s) s <> String.Empty)
            If words.Length = 2 AndAlso words(0) = "CONNECT" AndAlso words(1) = "ok" Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Sub ReadData()

        End Sub

        Public Sub CheckRecvTimer()

        End Sub

        Public Sub CheckSendTimer()

        End Sub

        Public Sub EndSession()

        End Sub

        Public Sub CheckCloseTimer()

        End Sub
    End Class


    Sub Main()
        'Dim addr As String = "169.254.167.110"
        'Dim port As Int32 = 2000
        'Dim client As TcpClient = New TcpClient(addr, port)
        'Console.WriteLine("Connected.")

        'Dim message As String = "hello from client"
        'Dim data As Byte() = System.Text.Encoding.ASCII.GetBytes(message)
        'Dim stream As NetworkStream = client.GetStream()
        'stream.Write(data, 0, data.Length)
        'Console.WriteLine("Sent {0}", message)

        'Dim bytes(1024) As Byte
        'stream.Read(bytes, 0, bytes.Length)
        'Dim response As String = Nothing
        'response = System.Text.Encoding.ASCII.GetString(bytes)
        'Console.WriteLine("received {0}", response)

        'stream.Close()
        'client.Close()
        'Console.WriteLine("Closed.")

        'Console.Write("Press enter to continue...")
        'Console.Read()
    End Sub
End Module
