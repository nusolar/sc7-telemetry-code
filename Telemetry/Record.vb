Class Record
    Public origDate As DateTime
    Public row As Integer
    Public data As Dictionary(Of String, String)

    Sub New(ByVal _date As DateTime, ByVal _row As Integer)
        origDate = _date
        row = _row
        data = New Dictionary(Of String, String)
    End Sub
End Class