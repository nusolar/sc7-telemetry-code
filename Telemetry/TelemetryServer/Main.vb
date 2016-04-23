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

                ' check for close, keep-alive
                rc = conn.ReadMessage()
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
