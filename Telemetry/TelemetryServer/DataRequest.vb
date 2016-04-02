Public Class DataRequest
    Public active As Boolean
    Public wants As List(Of String)
    Public sinceRow As Integer

    Public Sub New()
        active = False
    End Sub
End Class
