Imports System.Net.Sockets
Imports System.Collections.Generic
Imports System.Collections.Concurrent

Module Main
    '-----------------------------TYPE DEFINITIONS-------------------------------'
    Enum ClientResult
        FAILED
        OK
        NO_DATA
        RECV_CONNECT
        RECV_DATA
        RECV_KEEP_ALIVE
        RECV_CLOSE
    End Enum

    Enum ClientState
        CLOSED
        CONNECT_SENT
        CONNECTED
        CLOSE_WAIT
    End Enum

    '--------------------------------CODE------------------------------------------'

    Sub Main()
        ' create queue, connection
        Dim wants As List(Of String) = New List(Of String)
        Dim queue As ConcurrentQueue(Of Record) = New ConcurrentQueue(Of Record)
        Dim conn As Connection = New Connection(wants, queue)
        Dim state As ClientState = ClientState.CLOSED
        Dim rc As ClientResult = ClientResult.FAILED

        ' start main loop
        While True
            Select Case state
                Case ClientState.CLOSED
                    Console.WriteLine("***CLOSED state")

                    ' try opening connection
                    rc = conn.SendConnect()
                    If rc = ClientResult.OK Then
                        state = ClientState.CONNECT_SENT
                    Else
                        Threading.Thread.Sleep(1000)
                    End If
                Case ClientState.CONNECT_SENT
                    Console.WriteLine("***CONNECT_SENT state")

                    ' check for connect ACK
                    rc = conn.ReadConnectAck()
                    If rc = ClientResult.OK Then
                        state = ClientState.CONNECTED
                    ElseIf rc = ClientResult.FAILED Then
                        state = ClientState.CLOSED
                    ElseIf conn.CheckConnectTimer() Then
                        Console.WriteLine("Connection timer expired.")
                        state = ClientState.CLOSED
                    Else
                        Threading.Thread.Sleep(100)
                    End If
                Case ClientState.CONNECTED
                    Console.WriteLine("***CONNECTED state")

                    ' try reading data
                    rc = conn.ReadData()
                    If rc = ClientResult.FAILED Then
                        state = ClientState.CLOSED
                    End If

                    ' check send timer
                    If conn.CheckSendTimer() Then
                        rc = conn.SendKeepAlive()
                        If rc = ClientResult.FAILED Then
                            state = ClientState.CLOSED
                        End If
                    End If

                    ' check recv timer
                    If conn.CheckRecvTimer() Then
                        state = ClientState.CLOSED
                    End If
                Case ClientState.CLOSE_WAIT
                    Console.WriteLine("***CLOSE_WAIT state")

                    ' check for close ACK
                    rc = conn.ReadCloseAck()
                    If rc = ClientResult.OK OrElse rc = ClientResult.FAILED OrElse conn.CheckCloseTimer() Then
                        state = ClientState.CLOSED
                        Exit While
                    End If
            End Select
        End While
    End Sub
End Module
