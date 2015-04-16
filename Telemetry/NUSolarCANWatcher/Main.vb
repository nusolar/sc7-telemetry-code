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

    Private _COMPorts As List(Of String)
    Private _CANMessages As Collection
    Private _SaveCountdown As Stopwatch
    Private _InsertCommand As String
    Private _ErrorLog As String = "error_log.txt"

    Private _Port As SerialPort

#Region "Private Methods"
    Private Sub InitInsertStatement()
        _InsertCommand = "INSERT INTO tblHistory ("

        For Each CANMessage As CANMessageData In _CANMessages
            For Each datafield As cDataField In CANMessage.CANFields
                _InsertCommand &= datafield.FieldName & ", "
            Next
        Next

        _InsertCommand = _InsertCommand.Substring(0, _InsertCommand.Length - 2) & ") "
    End Sub

    Private Sub LogError(ByVal errorMessage As String, Optional boxTitle As String = "Unexpected Error", Optional showbox As Boolean = True)
        If My.Settings.ShowDebugMessageBoxes And showbox Then
            MsgBox(Format(Now, "T") & " " & errorMessage & vbNewLine, MsgBoxStyle.Critical, boxTitle)
        End If

        My.Computer.FileSystem.WriteAllText(_ErrorLog, Format(Now, "G") & vbTab & errorMessage & vbNewLine, True)
    End Sub

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
    Private Sub ConfigureCOMPort()
        ' Get current port names
        Dim ConnectionTrialCount As Integer
        Dim ConnectionIndex As Integer
        Dim ConnectionSucceded As Boolean = False

        _Port = New SerialPort()
        'Basic Setups
        _Port.BaudRate = 115200
        _Port.DataBits = 8
        _Port.Parity = Parity.None

        _Port.StopBits = 1
        'This checks whether the connection is on
        _Port.Handshake = False

        'Time outs are 500 milliseconds and this is a failsafe system that stops data reading after 500 milliseconds of no data
        _Port.ReadTimeout = 500
        _Port.WriteTimeout = 500

        'Loop until a connection succeeds 
        While Not ConnectionSucceded
            ' Get list of current ports
            _COMPorts = (SerialPort.GetPortNames).ToList
            _COMPorts.Insert(0, My.Settings.COMPort) ' Insert Predefined COMPort to try

            For Each port As String In _COMPorts

                Try
                    ConnectionTrialCount += 1
                    _Port.PortName = port
                    _Port.Close()
                    _Port.Open()

                    If _Port.IsOpen Then
                        '_Port.Write(":CONFIG;")
                        '     If True Then 'Try to send CONFIG command here to check whether this is the right object
                        '         ConnectionSucceded = True
                        '         Exit While ' Break Here
                        '     Else
                        '         LogError("Gridconnect CAN-USB not identified on " & _COMPorts(ConnectionIndex) & ". Trying next port.", "Invalid Device Error")
                        '         ConnectionIndex = (ConnectionIndex + 1) Mod _COMPorts.Length
                        '     End If
                        ConnectionSucceded = True
                        Exit While
                        
                    End If

                Catch connEx As System.IO.IOException
                    LogError("Unable to connect to " & My.Settings.COMPort & ". Trying next Port.", "Unable to open COM port")
                Catch accessEx As System.UnauthorizedAccessException
                    LogError("Access Denied. Failed to open " & My.Settings.COMPort & " Close any other programs that might be using it. Trying next port", "Unable to open COM port")
                Catch ex As Exception
                    LogError("Unexpected error - " & ex.Message & ", while connecting to COM port. Trying next Port.")
                End Try
            Next port

            ' We tried all the ports we knew about at this point, so wait a little bit before going back to the top and
            ' getting the list of ports again.
            System.Threading.Thread.Sleep(5000)

        End While
    End Sub

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
        Catch sqlEx As System.Data.SqlClient.SqlException
            LogError("Error loading SQL database: " & sqlEx.Errors(0).Message, "SQL Error")
        Catch ex As Exception
            LogError("Unexpected error - " & ex.Message & ", while loading CAN Field SQL database")
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
                    	LogError("Unknown Tag - " & Tag & " encountered. Discarding packet...", "Invalid CAN Tag", false) ' Don't show a box here
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
