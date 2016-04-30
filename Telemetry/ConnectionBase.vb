Imports Microsoft.VisualBasic
Imports System.Net
Imports System.Net.Sockets
Imports System.Collections.Generic
Imports System.Collections.Concurrent

Public Enum ConnectionResult
    FAILED
    OK
    NO_DATA
End Enum

Public Class ConnectionBase
    Protected socket As Socket
    Protected listener As Socket

    '' Attempts to read a line from the socket. Will block.
    Protected Function Read(ByRef message As String) As Boolean
        Dim buffer(2048) As Byte
        Dim i As Integer = 0
        Dim bytesRead As Integer = 0
        Dim readByte(0) As Byte

        ' try reading
        Try
            While True
                ' read one byte
                bytesRead = socket.Receive(readByte)

                ' if byte actually read add to buffer
                If bytesRead > 0 Then
                    ' add to buffer
                    buffer(i) = readByte(0)

                    ' if newline, end of message
                    If buffer(i) = CByte(Asc(vbLf)) Then
                        Exit While
                    End If

                    ' increment buffer position
                    i += 1
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
    Protected Function TryRead(ByRef words As String()) As ConnectionResult
        ' check if data available
        If socket.Available <= 0 Then
            Return ConnectionResult.NO_DATA
        End If

        ' read client message
        Dim response As String = ""
        If Not Read(response) Then
            Return ConnectionResult.FAILED
        End If

        ' parse client message
        words = response.Split().Where(Function(s) s <> String.Empty).ToArray()
        Return ConnectionResult.OK
    End Function

    '' Attempts to write the given message to the socket.
    Protected Function Write(ByVal message As String) As Boolean
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
    Protected Sub Close()
        Try
            If listener IsNot Nothing Then
                listener.Close()
            End If
            If socket IsNot Nothing Then
                socket.Close()
            End If
        Catch ex As SocketException
            Console.WriteLine("Socket close failed: " & ex.Message)
            Return
        End Try

        Console.WriteLine("Socket closed.")
    End Sub
End Class
