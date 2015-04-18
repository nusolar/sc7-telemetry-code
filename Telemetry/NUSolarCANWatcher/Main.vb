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

    Private _ErrorLog As String
    Private _DebugLog As String

    Private _COMPorts As List(Of String)
    Private _Port As SerialPort
    Private _COMConnected As Boolean

    Private _CANMessages As Collection

    Private _SaveCountdown As Stopwatch

    Private _InsertCommand As String
    Private _Values As String

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

    Private Sub ClearErrorLog()
        My.Computer.FileSystem.WriteAllText(_ErrorLog, "", False)
    End Sub

    Private Sub WriteDebug(ByVal debugMessage As String)
        If My.Settings.EnableDebug Then
            My.Computer.FileSystem.WriteAllText(_DebugLog, Format(Now, "G") & vbTab & debugMessage & vbNewLine, True)
        End If
    End Sub

    Private Sub ClearDebugLog()
        My.Computer.FileSystem.WriteAllText(_DebugLog, "", False)
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
        WriteDebug("*** OPENING COM PORT")

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

        _COMConnected = True
        WriteDebug("opened " & _Port.PortName)
    End Sub

    Private Function LoadCANFields() As Boolean
        Debug.WriteLine("Loading CAN fields")
        WriteDebug("*** LOADING SQL DATABASE")

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
                        WriteDebug("cantag " & data.CANTag & " field " & data.FieldName)

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

        Debug.WriteLine("Reading CAN message")
        WriteDebug("*** READING CAN MESSAGE")

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
            WriteDebug("raw message " & Message)
            'Recognize :S & N

            If Message.Length = 22 Then
                Tag = Message.Substring(2, 3)
                'CanData = Message.Substring(6, 16)
                For i As Integer = 20 To 6 Step -2
                    CanData &= Message.Substring(i, 2)
                Next
                Debug.Print("CANTAG " & Tag & " CANDATA " & CanData)
                WriteDebug("cantag " & Tag & " candata " & CanData)


                If _CANMessages.Contains(Tag) Then
                    _CANMessages(Tag).NewDataValue = New cCANData(CanData)
                    For Each datafield As cDataField In CType(_CANMessages(Tag), CANMessageData).CANFields
                        WriteDebug("field " & datafield.FieldName & " value " & datafield.DataValueAsString)
                    Next
                End If
            Else
                LogError("Invalid CAN packet received from COM port: " & Message, "Invalid CAN packet", False)
            End If

        Catch timeoutEx As System.TimeoutException
            LogError("COM port read timed out while attempting to get CAN packet", "COM Read Timeout")
        Catch ioEx As System.IO.IOException
            LogError("COM port disconnected while attempting to get CAN packet", "COM Port Disconnection")
            _COMConnected = False
        Catch invalidOpEx As System.InvalidOperationException
            LogError("COM port closed while attempting to get CAN packet", "COM Port Closed")
            _COMConnected = False
            ConfigureCOMPort()
        Catch ex As Exception
            LogError("Unexpected error - " & ex.Message & vbCrLf & "while getting can message")
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
            Debug.Print("Saving data Values")
            'WriteDebug("*** WRITING TO SQL DATABASE")

            _Values = "VALUES ("
            With DataGrid
                .Rows.Clear()
                For Each CANMessage As CANMessageData In _CANMessages
                    For Each datafield As cDataField In CANMessage.CANFields
                        .Rows.Add({datafield.FieldName, datafield.CANTag, datafield.CANByteOffset, datafield.DataValueAsString})
                        _Values &= datafield.DataValueAsString & ","
                    Next
                Next
            End With
            _Values = _Values.Substring(0, _Values.Length - 1) & ")"
            'WriteDebug(_InsertCommand)
            'WriteDebug(_Values)

            Using cnn As New SqlConnection(My.Settings.DSN)
                cnn.Open()
                Dim cmd As New SqlCommand
                With cmd
                    .CommandText = _InsertCommand & _Values
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

        Catch sqlEx As System.Data.SqlClient.SqlException
            LogError("Error writing to SQL database: " & sqlEx.Errors(0).Message, "SQL Write Error")
        Catch ex As Exception
            LogError("Unexpected error - " & ex.Message & vbCrLf & "while writing to database", "Unexpected Error")
        End Try
    End Sub
#End Region
#Region "Event Handlers"
    Private Sub Main_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        _ErrorLog = My.Settings.ErrorLogName
        ClearErrorLog()
        _DebugLog = My.Settings.DebugLogName
        ClearDebugLog()
        _COMConnected = False
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
            LogError("Unexpected error - " & ex.Message & vbCrLf & "while Loading form", "Unexpected Error")
        End Try
    End Sub
    Private Sub CANCheckTimer_Tick(sender As Object, e As System.EventArgs) Handles CANCheckTimer.Tick
        If Not chkPause.Checked Then
            If _COMConnected And Not Me.CANRead_BW.IsBusy Then
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
