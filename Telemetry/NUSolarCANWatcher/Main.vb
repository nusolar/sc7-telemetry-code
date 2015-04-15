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

    Private _COMPorts As String()
    Private _CANMessages As Collection
    Private _SaveCountdown As Stopwatch
    Private _InsertCommand As String

    Private _Port As SerialPort

#Region "Private Methods"
    Private Sub TestDB()
        Using cnn As New SqlConnection(My.Settings.DSN)
            cnn.Open()
            Dim cmd As New SqlCommand
            With cmd
                .CommandText = "SELECT TOP 10 [FieldName],[Id]  FROM [NUSolarTelemetry].[dbo].[tblDataItems] ORDER BY [ID]"
                .CommandType = CommandType.Text
                .Connection = cnn
                Dim dr As SqlDataReader = .ExecuteReader
                Do While dr.Read()
                    Console.WriteLine(vbTab & dr.GetInt32(1) & dr.GetString(0))
                Loop

            End With

        End Using
    End Sub
    Private Function ConfigureCOMPort()
        ' Get current port names
        _COMPorts = SerialPort.GetPortNames
        Dim ConnectionTrialCount As Integer
        Dim ConnectionIndex As Integer = Array.IndexOf(_COMPorts, My.Settings.COMPort)
        Dim ConnectionSucceded As Boolean = False

        'Check if the serial port was found in the array of connected ports, if not then set it to the first index
        If ConnectionIndex = -1 Then
            ConnectionIndex = 0
        End If

        While Not ConnectionSucceded
            Try
                _Port = New SerialPort()
                'Basic Setups
                _Port.PortName = _COMPorts(ConnectionIndex)
                _Port.BaudRate = 115200
                _Port.DataBits = 8
                _Port.Parity = Parity.None

                _Port.StopBits = 1
                'This checks whether the connection is on
                _Port.Handshake = False

                'Time outs are 500 milliseconds and this is a failsafe system that stops data reading after 500 milliseconds of no data
                _Port.ReadTimeout = 500
                _Port.WriteTimeout = 500

                _Port.Open()
                If _Port.IsOpen Then ConnectionSucceded = True

            Catch connEx As System.IO.IOException
                MsgBox("Error connecting to CAN-USB converter.", MsgBoxStyle.Critical, "Unable to open the Comport")
            Catch ex As Exception
                MsgBox("Failed to Open " & My.Settings.COMPort & " Close any other programs that might be using it", MsgBoxStyle.Critical, "Unable to open the Comport")
            End Try
        End While

    End Function
    Private Function LoadCANFields() As Boolean
        LoadCANFields = False
        Try
            Dim LastCANTag As String = ""
            Dim CANMessage As CANMessageData = Nothing
            _CANMessages = New Collection
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
                                _CANMessages.Add(CANMessage, CANMessage.CANTag)
                            End If
                            CANMessage = New CANMessageData
                            CANMessage.CANTag = data.CANTag
                            LastCANTag = data.CANTag
                        End If
                        CANMessage.CANFields.Add(data)
                    Loop
                    dr.Close()
                    If Not CANMessage Is Nothing Then
                        _CANMessages.Add(CANMessage, CANMessage.CANTag)
                    End If
                End With
            End Using
            LoadCANFields = True
        Catch connEx As System.Data.SqlClient.SqlException
            MsgBox("Error connecting to SQL database while attempting to load.", MsgBoxStyle.Critical, "SQL Connection Error")
        Catch ex As Exception
            MsgBox("Unexpected error - " & ex.Message & vbCrLf & "while Loading CAN Field collection", MsgBoxStyle.Critical, "Unexpected Error")
        End Try
    End Function
    Private Function InitInsertStatement()
        _InsertCommand = "INSERT INTO tblHistory ("

        For Each CANMessage As CANMessageData In _CANMessages
            For Each datafield As cDataField In CANMessage.CANFields
                _InsertCommand &= datafield.FieldName & ", "
            Next
        Next

        _InsertCommand = _InsertCommand.Substring(0, _InsertCommand.Length - 2) & ") "
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
            '       _CANMessages(<can tag>).NewDataValue = <can value>
            '
            '   For exammple, if the can tag was in the variable called Tag and the 8 byte value associated with the tag
            '   was in the variable called Data (an instance of cCANData) , the assignment statement would be:
            '
            '       _CANMessages(Tag).NewDataValue = Data
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
                    Try
                        _CANMessages(Tag).NewDataValue = New cCANData(CanData)
                    Catch ex As Exception
                    End Try
                End If
            End If

            ' Do While loop based on exception 
        Catch ex As Exception
            MsgBox("Unexpected error - " & ex.Message & vbCrLf & "while getting can message", MsgBoxStyle.Critical, "Unexpected Error")

        End Try


    End Sub
    Private Sub SaveData()
        Try
            '
            '   This is where the collection of data is written to the database.
            '
            '   We will fill this in as our next example.  For now, we will update them
            '   in the grid on the form.
            '
            Dim values As String = "VALUES ("
            Debug.Print("Saving data Values")
            With DataGrid
                .Rows.Clear()
                For Each CANMessage As CANMessageData In _CANMessages
                    For Each datafield As cDataField In CANMessage.CANFields
                        .Rows.Add({datafield.FieldName, datafield.CANTag, datafield.CANByteOffset, datafield.DataValueAsString})
                        values &= datafield.DataValueAsString & ","
                    Next
                Next
            End With
            values = values.Substring(0, values.Length - 1) & ")"

            Using cnn As New SqlConnection(My.Settings.DSN)
                cnn.Open()
                Dim cmd As New SqlCommand
                With cmd
                    .CommandText = _InsertCommand & values
                    .CommandType = CommandType.Text
                    .CommandTimeout = 0
                    .Connection = cnn
                    .ExecuteNonQuery()
                End With
            End Using
            '
            '   After saving values to database, reset for the next polling cycle
            '
            For Each CANMessage As CANMessageData In _CANMessages
                For Each datafield As cDataField In CANMessage.CANFields
                    datafield.Reset()
                Next
            Next
        Catch ex As Exception
            MsgBox("Unexpected error - " & ex.Message & vbCrLf & "while Loading form", MsgBoxStyle.Critical, "Unexpected Error")
        End Try
    End Sub
#End Region
#Region "Event Handlers"
    Private Sub Main_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        Try
            If LoadCANFields() Then
                InitInsertStatement()
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
            If Not Me.CANRead_BW.IsBusy Then
                Me.CANRead_BW.RunWorkerAsync()
            End If
            If _SaveCountdown.ElapsedMilliseconds > My.Settings.ValueStorageInterval Then
                SaveData()
                _SaveCountdown = Stopwatch.StartNew
            End If
        End If
    End Sub
    Private Sub btnClose_Click(sender As Object, e As System.EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub
    Private Sub CANRead_BW_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles CANRead_BW.DoWork
        GetCANMessage()
    End Sub
    Private Sub CANRead_BW_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles CANRead_BW.RunWorkerCompleted
    End Sub
#End Region
End Class
