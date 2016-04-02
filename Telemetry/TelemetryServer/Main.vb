Imports System.Net
Imports System.Net.Sockets
Imports System.Collections.Generic
Imports System.Collections.Concurrent
Imports System.Threading

Module Main
    '-----------------------------TYPE DEFINITIONS-------------------------------'
    Enum ServerResult
        FAILED
        OK
        NO_DATA
    End Enum

    Enum ServerState
        CLOSED
        CONNECTED
    End Enum

    '------------------------------SHARED DATA-------------------------------------'
    Dim _DataReq As DataRequest = New DataRequest()
    Dim _DataQueue As ConcurrentQueue(Of Record) = New ConcurrentQueue(Of Record)
    Dim _Closing As Boolean = False

    '--------------------------------CODE------------------------------------------'
    Sub Main()
        ' start connection thread
        Dim connThread As Thread = New Thread(AddressOf RunConn)
        connThread.Start()

        ' enter user interaction loop
        While True
            Dim input As String = Console.ReadLine()
            If input = "quit" Then
                _Closing = True
            End If
        End While

        ' wait for threads to finish
        connThread.Join()

        'Dim addr As IPAddress = IPAddress.Any
        'Dim port As Int32 = 2000
        'Dim listener As New TcpListener(addr, port)

        'listener.Start(10)
        'Console.WriteLine("Start listening.")

        'Dim client As TcpClient = listener.AcceptTcpClient()
        'Console.WriteLine("Connected.")

        'Dim bytes(1024) As Byte
        'Dim data As String = Nothing
        'Dim stream As NetworkStream = client.GetStream()
        'Dim i As Int32 = stream.Read(bytes, 0, bytes.Length)
        'data = System.Text.Encoding.ASCII.GetString(bytes, 0, i)
        'Console.WriteLine("Received {0}", data)

        'Dim message As String = "hello from server"
        'Dim messageBytes As Byte() = System.Text.Encoding.ASCII.GetBytes(message)
        'stream.Write(messageBytes, 0, messageBytes.Length)
        'Console.WriteLine("Sent {0}", message)

        'client.Close()
        'listener.Stop()
        'Console.WriteLine("Closed.")

        'Console.Write("Press enter to continue...")
        'Console.Read()
    End Sub

    Sub RunConn()
        ' create connection
        Dim conn As Connection = New Connection(_DataReq, _DataQueue)
        Dim state As ServerState = ServerState.CLOSED
        Dim rc As ServerResult = ServerResult.FAILED

        ' start main loop
        Select Case state
            Case ServerState.CLOSED
                Console.WriteLine("***CLOSED state")

                rc = conn.ReadConnect()
                If rc = ServerResult.OK Then
                    state = ServerState.CONNECTED
                Else
                    Thread.Sleep(1000)
                End If
            Case ServerState.CONNECTED
                Console.WriteLine("***CONNECTED state")

                ' send data
                rc = conn.SendData()
                If rc = ServerResult.FAILED Then
                    state = ServerState.CLOSED
                End If

                ' check for close
                rc = conn.ReadClose()
                If rc = ServerResult.FAILED Then
                    state = ServerState.CLOSED
                ElseIf rc = ServerResult.OK Then
                    rc = conn.SendClose()
                    state = ServerState.CLOSED
                End If

                ' check send timer
                If conn.CheckSendTimer() Then
                    rc = conn.SendKeepAlive()
                    If rc = ServerResult.FAILED Then
                        state = ServerState.CLOSED
                    End If
                End If

                ' check receive timer
                If conn.CheckRecvTimer() Then
                    state = ServerState.CLOSED
                End If
        End Select
    End Sub
End Module
