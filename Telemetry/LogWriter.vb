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
            _Messages.Enqueue(Format(DateAndTime.Now, "MM/dd/yyyy hh:mm:ss.fff tt") & vbTab & Message & vbNewLine)
        End If
    End Sub
    Public Sub WriteAll()
        Dim Message As String = ""
        Dim tries As Integer = 0
        While Not _Messages.IsEmpty
            If _Messages.TryDequeue(Message) Then
                My.Computer.FileSystem.WriteAllText(_LogFile, Message, True)
            End If
        End While
    End Sub
    Public Sub ClearLog()
        My.Computer.FileSystem.WriteAllText(_LogFile, "", False)
    End Sub
End Class
