Imports System.Data.Odbc
Imports System.Data.Common
Module modGeneral
    Public Const k_CurrentFormat As String = "#,##0.00"
    Public Const k_VoltageFormat As String = "#,##0.00;#,##0.00;-"
    Public Const k_TemperatureFormat As String = "#,##0.00;#,##0.00;-"
    Public Const k_SpeedFormat As String = "#,##0.00"
    Public Const k_BatteryPacks As Integer = 26
    Public Sub FormatValue(ByVal Value As Double, ByVal FormatString As String, ByVal Control As Windows.Forms.Label)
        Control.Text = Format(Value, FormatString)
        Control.ForeColor = CType(IIf(Value < 0, System.Drawing.Color.Red, System.Drawing.Color.Green), System.Drawing.Color)
    End Sub
    Public Function GetDoubleValue(ByVal Value As Object) As Double
        GetDoubleValue = CDbl(IIf(IsDBNull(Value), 0, Value))
    End Function
    Sub Main()
        Dim f As New Main
        f.ShowDialog()
    End Sub
End Module
