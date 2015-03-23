Imports System.Data.SqlClient
Imports System.IO.Ports

Public Class Main
    Private Class CANMessageData
        Public CANTag As String
        Public CANFields As New Collection
        Public WriteOnly Property NewDataValue As cCANData
            Set(value As cCANData)
                For Each field As cDataField In CANFields
                    field.NewDataValue = value
                Next
            End Set
        End Property
    End Class

    Private _CANFields As Collection
    Private _SaveCountdown As Stopwatch

    Private _Port As SerialPort

#Region "Private Methods"
    Private Function ConfigureCOMPort()
        Try
            _port = New SerialPort()
            'Basic Setups
            _Port.PortName = My.Settings.COMPort
            _port.BaudRate = 115200
            _port.DataBits = 8
            _port.Parity = Parity.None

            _port.StopBits = 1
            'This checks whether the connection is on
            _port.Handshake = False

            'Time outs are 500 milliseconds and this is a failsafe system that stops data reading after 500 milliseconds of no data
            _port.ReadTimeout = 500
            _port.WriteTimeout = 500

            _Port.Open()

        Catch ex As Exception
            MsgBox("Failed to Open " & My.Settings.COMPort & " Close any other programs that might be using it", MsgBoxStyle.Critical, "Unable to open the Comport")
            End
        End Try

    End Function
    Private Function LoadCANFields() As Boolean
        LoadCANFields = False
        Try
            Dim LastCANTag As String = ""
            Dim CANMessage As CANMessageData = Nothing
            _CANFields = New Collection
            Using cnn As New SqlConnection(My.Settings.DSN)
                cnn.Open()
                Dim cmd As New SqlCommand
                With cmd
                    .CommandText = "p_GetCANFields"
                    .CommandType = CommandType.StoredProcedure
                    .CommandTimeout = 0
                    .Connection = cnn
                    Dim dr As SqlDataReader = .ExecuteReader
                    Do While dr.Read
                        Dim data As New cDataField(dr)
                        If data.CANTag <> LastCANTag Then
                            If Not CANMessage Is Nothing Then
                                _CANFields.Add(CANMessage, CANMessage.CANTag)
                            End If
                            CANMessage = New CANMessageData
                            CANMessage.CANTag = data.CANTag
                            LastCANTag = data.CANTag
                        End If
                        CANMessage.CANFields.Add(data)
                    Loop
                    dr.Close()
                    If Not CANMessage Is Nothing Then
                        _CANFields.Add(CANMessage, CANMessage.CANTag)
                    End If
                End With
            End Using
            LoadCANFields = True
        Catch ex As Exception
            MsgBox("Unexpected error - " & ex.Message & vbCrLf & "while Loading CAN Field collection", MsgBoxStyle.Critical, "Unexpected Error")
        End Try
    End Function
    Private Sub GetCANMessage()
        Dim Message As String = ""
        Dim Tag As String = ""
        Dim CanData As String = ""

        Try
            '
            '   This is where you would read the CAN buss and extract the tag/value pairs
            '
            '   To save the value into the collection, you would use the expression
            '
            '       _CANFields(<can tag>).NewDataValue = <can value>
            '
            '   For exammple, if the can tag was in the variable called Tag and the 8 byte value associated with the tag
            '   was in the variable called Data (an instance of cCANData) , the assignment statement would be:
            '
            '       _CANFields(Tag).NewDataValue = Data
            '

            Message = _Port.ReadTo(";")
            'Recognize :S & N

            If Message.Length = 22 Then
                Tag = Message.Substring(2, 3)
                'CanData = Message.Substring(6, 16)
                For i As Integer = 20 To 6 Step -2
                    CanData &= Message.Substring(i, 2)
                Next


                Debug.Print("CANTAG " & Tag & " CANDATA " & CanData)
                If Tag <> "500" And Tag <> "600" Then
                    _CANFields(Tag).newDataValue = New cCANData(CanData)
                End If
            End If

            ' Do While loop based on exception 
    End Sub
    Private Sub SaveData()
        Try
            '
            '   This is where the collection of data is written to the database.
            '
            '   We will fill this in as our next example.  For now, we will update them
            '   in the grid on the form.
            '
            Debug.Print("Saving data Values")
            With DataGrid
                .Rows.Clear()
                For Each CANMessage As CANMessageData In _CANFields
                    For Each datafield As cDataField In CANMessage.CANFields
                        .Rows.Add({datafield.FieldName, datafield.CANTag, datafield.CANByteOffset, datafield.DataValueAsString})
                    Next
                Next
            End With
            '
            '   After saving values to database, reset for the next polling cycle
            '
            For Each CANMessage As CANMessageData In _CANFields
                For Each datafield As cDataField In CANMessage.CANFields
                    datafield.Reset()
                Next
            Next
        Catch ex As Exception

        End Try
    End Sub
#End Region
#Region "Event Handlers"
    Private Sub Main_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        Try
            If LoadCANFields() Then
                ConfigureCOMPort()
                CANCheckTimer.Interval = My.Settings.CANCheckInterval
                CANCheckTimer.Enabled = True
                _SaveCountdown = Stopwatch.StartNew
            Else
                End
            End If

        Catch ex As Exception
            MsgBox("Unexpected error - " & ex.Message & vbCrLf & "while Loading form", MsgBoxStyle.Critical, "Unexpected Error")
        End Try
    End Sub
    Private Sub CANCheckTimer_Tick(sender As Object, e As System.EventArgs) Handles CANCheckTimer.Tick
        If Not chkPause.Checked Then
            GetCANMessage()
            If _SaveCountdown.ElapsedMilliseconds > My.Settings.ValueStorageInterval Then
                SaveData()
                _SaveCountdown = Stopwatch.StartNew
            End If
        End If
    End Sub
    Private Sub btnClose_Click(sender As Object, e As System.EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub
#End Region
End Class
