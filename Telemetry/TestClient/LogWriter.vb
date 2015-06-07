Imports System.Collections.Concurrent

Public Class LogWriter
    Private _Messages As ConcurrentQueue(Of String)
    Private _LogFile As String
    Private _Enabled As Boolean
    Public Sub New(ByVal LogFile As String, Optional Enabled As Boolean = True)
        MyBase.New()
        _Messages = New ConcurrentQueue(Of String)
        _LogFile = LogFile
        _Enabled = Enabled
    End Sub
    Public Sub AddMessage(ByVal Message As String)
        If _Enabled Then
            _Messages.Enqueue(Format(Now, "G") & vbTab & Message & vbNewLine)
        End If
    End Sub
    Public Sub Write(ByVal Message As String)
        AddMessage(Message)
        WriteAll()
    End Sub
    Public Sub WriteAll()
        Dim Message As String = ""
        Dim tries As Integer = 0
        While Not _Messages.IsEmpty
            If _Messages.TryDequeue(Message) Then
                My.Computer.FileSystem.WriteAllText(_LogFile, Message, True)
                tries = 0
            Else
                tries += 1
                If tries > 100 Then
                    My.Computer.FileSystem.WriteAllText(_LogFile, Format(Now, "G") & vbTab & "Unable to write message to Log File. Another Thread may have the collection locked." & vbNewLine, True)
                    Exit While
                End If
            End If
        End While
    End Sub
    Public Sub ClearLog()
        My.Computer.FileSystem.WriteAllText(_LogFile, "", False)
    End Sub
End Class