Imports Microsoft.VisualBasic

Public Class Timer
    Private interval As Integer
    Private timeout As DateTime

    Public Sub New(ByVal _interval As Double)
        interval = _interval
    End Sub

    Public Sub Reset()
        timeout = Date.Now.AddMilliseconds(interval)
    End Sub

    Public Function Elapsed() As Boolean
        Return Date.Now.CompareTo(timeout) >= 0
    End Function
End Class
