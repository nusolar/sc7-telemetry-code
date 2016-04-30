Imports System.Net.Sockets
Imports System.Collections.Generic
Imports System.Collections.Concurrent
Imports System.Threading

Module Main
    '-----------------------------TYPE DEFINITIONS-------------------------------'
    Enum ClientState
        CLOSED
        CONNECT_SENT
        CONNECTED
        CLOSE_WAIT
    End Enum

    '------------------------------SHARED DATA-------------------------------------'
    Dim _DataQueue As ConcurrentQueue(Of Record)
    Dim _Closing As Boolean = False

    '--------------------------------CODE------------------------------------------'
    Sub Main()
        ' start connection thread
        Dim connThread As Thread = New Thread(AddressOf RunConn)
        connThread.Start()

        ' enter user interaction loop
        _Closing = False
        While True
            Dim input As String = Console.ReadLine()
            If input = "quit" Then
                _Closing = True
            End If
        End While

        ' wait for threads to finish
        connThread.Join()
    End Sub

    Sub RunConn()
        ' create queue, connection
        Dim wants As List(Of String) = New List(Of String)
        _DataQueue = New ConcurrentQueue(Of Record)
        Dim conn As ClientConnection = New ClientConnection(wants, _DataQueue)
        Dim state As ClientState = ClientState.CLOSED
        Dim rc As ConnectionResult = ConnectionResult.FAILED

        ' start main loop
        While True
            Select Case state
                Case ClientState.CLOSED
                    Console.WriteLine("***CLOSED state")

                    ' try opening connection
                    rc = conn.SendConnect()
                    If rc = ConnectionResult.OK Then
                        state = ClientState.CONNECT_SENT
                    Else
                        Thread.Sleep(1000)
                    End If
                Case ClientState.CONNECT_SENT
                    Console.WriteLine("***CONNECT_SENT state")

                    ' check for connect ACK
                    rc = conn.ReadConnectAck()
                    If rc = ConnectionResult.OK Then
                        state = ClientState.CONNECTED
                    ElseIf rc = ConnectionResult.FAILED Then
                        state = ClientState.CLOSED
                    ElseIf conn.CheckConnectTimer() Then
                        Console.WriteLine("Connection timer expired.")
                        state = ClientState.CLOSED
                    Else
                        Thread.Sleep(500)
                    End If
                Case ClientState.CONNECTED
                    Console.WriteLine("***CONNECTED state")

                    ' try reading data
                    rc = conn.ReadData()
                    If rc = ConnectionResult.FAILED Then
                        state = ClientState.CLOSED
                    End If

                    ' check send timer
                    If conn.CheckSendTimer() Then
                        rc = conn.SendKeepAlive()
                        If rc = ConnectionResult.FAILED Then
                            state = ClientState.CLOSED
                        End If
                    End If

                    ' check recv timer
                    If conn.CheckRecvTimer() Then
                        Console.WriteLine("Receive timer expired.")
                        state = ClientState.CLOSED
                    End If

                    Thread.Sleep(500)
                Case ClientState.CLOSE_WAIT
                        Console.WriteLine("***CLOSE_WAIT state")

                        ' check for close ACK
                        rc = conn.ReadCloseAck()
                        If rc = ConnectionResult.OK OrElse rc = ConnectionResult.FAILED OrElse conn.CheckCloseTimer() Then
                            state = ClientState.CLOSED
                            Exit While
                        End If

                        Thread.Sleep(500)
            End Select

            ' if closing, send close message
            If _Closing Then
                Console.WriteLine("***CLOSING")

                If state = ClientState.CONNECTED Then
                    rc = conn.SendClose()
                    If rc = ConnectionResult.FAILED Then
                        state = ClientState.CLOSED
                    Else
                        state = ClientState.CLOSE_WAIT
                    End If
                Else
                    state = ClientState.CLOSED
                End If
            End If
        End While
    End Sub
End Module
