Imports System.Data.SqlClient
Imports System.IO.Ports
Imports System.Collections.Concurrent

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

    Private Class LogWriter
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
        Public Sub WriteAll()
            Dim Message As String = ""
            Dim tries As Integer = 0
            While Not _Messages.IsEmpty
                If _Messages.TryDequeue(Message) Then
                    My.Computer.FileSystem.WriteAllText(_LogFile, Message, True)
                    tries = 0
                Else
                    tries += 1
                    If tries > My.Settings.LogWriteMaxAttempts Then
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

    Private _ErrorWriter As LogWriter
    Private _DebugWriter As LogWriter

    Private _COMPorts As List(Of String)
    Private _Port As SerialPort
    Private _COMConnected As Boolean
    Private _SQLConn As SqlConnection

    Private _CANMessages As ConcurrentDictionary(Of String, CANMessageData)

    Private _SaveCountdown As Stopwatch

    Private _InsertCommand As String
    Private _Values As String

#Region "Private Methods"
    Private Sub OpenSqlConnection()
        Try
            _SQLConn = New SqlConnection(My.Settings.DSN)
            _SQLConn.Open()
        Catch sqlEx As System.Data.SqlClient.SqlException
            _ErrorWriter.AddMessage("Error opening SQL connection: " & sqlEx.Errors(0).Message)
            _ErrorWriter.WriteAll()
            ErrorDialog("Error opening SQL connection: " & sqlEx.Errors(0).Message, "SQL Error")
        Catch ex As Exception
            _ErrorWriter.AddMessage("Unexpected error - " & ex.Message & ", while opening SQL connection")
            _ErrorWriter.WriteAll()
            ErrorDialog("Unexpected error - " & ex.Message & ", while opening SQL connection")
        End Try
    End Sub
    Private Sub CloseSqlConnection()
        Try
            _SQLConn.Close()
        Catch sqlEx As System.Data.SqlClient.SqlException
            _ErrorWriter.AddMessage("Error closing SQL connection: " & sqlEx.Errors(0).Message)
            _ErrorWriter.WriteAll()
            ErrorDialog("Error closing SQL connection: " & sqlEx.Errors(0).Message, "SQL Error")
        Catch ex As Exception
            _ErrorWriter.AddMessage("Unexpected error - " & ex.Message & ", while closing SQL connection")
            _ErrorWriter.WriteAll()
            ErrorDialog("Unexpected error - " & ex.Message & ", while closing SQL connection")
        End Try
    End Sub
    Private Sub InitInsertStatement()
        _InsertCommand = "INSERT INTO tblHistory ("

        For Each CANMessage As CANMessageData In _CANMessages.Values
            For Each datafield As cDataField In CANMessage.CANFields
                _InsertCommand &= datafield.FieldName & ", "
            Next
        Next

        _InsertCommand = _InsertCommand.Substring(0, _InsertCommand.Length - 2) & ") "
    End Sub

    Private Sub ErrorDialog(ByVal errorMessage As String, Optional boxTitle As String = "Unexpected Error")
        If My.Settings.ShowDebugMessageBoxes Then
            MsgBox(Format(Now, "T") & " " & errorMessage & vbNewLine, MsgBoxStyle.Critical, boxTitle)
        End If
    End Sub

    Private Sub ConfigureCOMPort()
        _DebugWriter.AddMessage("*** OPENING COM PORT")

        ' Get current port names
        Dim ConnectionTrialCount As Integer
        Dim ConnectionSucceded As Boolean = False

        _Port = New SerialPort()
        'Basic Setups
        _Port.BaudRate = My.Settings.BaudRate
        _Port.DataBits = 8
        _Port.Parity = Parity.None

        _Port.StopBits = 1
        'This checks whether the connection is on
        _Port.Handshake = False

        'Time outs are 500 milliseconds and this is a failsafe system that stops data reading after 500 milliseconds of no data
        _Port.ReadTimeout = My.Settings.COMTimeout
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
                    _ErrorWriter.AddMessage("Unable to connect to " & My.Settings.COMPort)
                    _ErrorWriter.WriteAll()
                    ErrorDialog("Unable to connect to " & My.Settings.COMPort & ". Trying next Port.", "Unable to open COM port")
                Catch accessEx As System.UnauthorizedAccessException
                    _ErrorWriter.AddMessage("Access Denied. Failed to open " & My.Settings.COMPort)
                    _ErrorWriter.WriteAll()
                    ErrorDialog("Access Denied. Failed to open " & My.Settings.COMPort & " Close any other programs that might be using it. Trying next port", "Unable to open COM port")
                Catch ex As Exception
                    _ErrorWriter.AddMessage("Unexpected error - " & ex.Message & ", while connecting to COM port")
                    _ErrorWriter.WriteAll()
                    ErrorDialog("Unexpected error - " & ex.Message & ", while connecting to COM port. Trying next Port.")
                End Try
            Next port

            ' We tried all the ports we knew about at this point, so wait a little bit before going back to the top and
            ' getting the list of ports again.
            System.Threading.Thread.Sleep(5000)

        End While

        _COMConnected = True
        _DebugWriter.AddMessage("opened " & _Port.PortName)
    End Sub

    Private Function LoadCANFields() As Boolean
        Debug.WriteLine("Loading CAN fields")
        _DebugWriter.AddMessage("*** LOADING SQL DATABASE")

        LoadCANFields = False
        Try
            Dim LastCANTag As String = ""
            Dim CANMessage As CANMessageData = Nothing
            _CANMessages = New ConcurrentDictionary(Of String, CANMessageData)

            Dim cmd As New SqlCommand
            With cmd
                .CommandText = "p_GetCANFields"
                .CommandType = CommandType.StoredProcedure
                .CommandTimeout = 0
                .Connection = _SQLConn
                Dim dr As SqlDataReader = .ExecuteReader
                Do While dr.Read
                    Dim data As New cDataField(dr)
                    _DebugWriter.AddMessage("cantag " & data.CANTag & " field " & data.FieldName)

                    If data.CANTag <> LastCANTag Then
                        If Not CANMessage Is Nothing Then
                            _CANMessages.TryAdd(CANMessage.CANTag, CANMessage)
                        End If
                        CANMessage = New CANMessageData
                        CANMessage.CANTag = data.CANTag
                        LastCANTag = data.CANTag
                    End If
                    CANMessage.CANFields.Add(data)
                Loop
                dr.Close()
                If Not CANMessage Is Nothing Then
                    _CANMessages.TryAdd(CANMessage.CANTag, CANMessage)
                End If
            End With

            LoadCANFields = True

        Catch sqlEx As System.Data.SqlClient.SqlException
            _ErrorWriter.AddMessage("Error loading SQL database: " & sqlEx.Errors(0).Message)
            _ErrorWriter.WriteAll()
            ErrorDialog("Error loading SQL database: " & sqlEx.Errors(0).Message, "SQL Error")
        Catch ex As Exception
            _ErrorWriter.AddMessage("Unexpected error - " & ex.Message & ", while loading CAN Field SQL database")
            _ErrorWriter.WriteAll()
            ErrorDialog("Unexpected error - " & ex.Message & ", while loading CAN Field SQL database")
        End Try
    End Function
    Private Sub GetCANMessage()
        Dim Message As String = ""
        Dim Tag As String = ""
        Dim CanData As String = ""
        Dim CurrentMessage As CANMessageData = Nothing

        _DebugWriter.AddMessage("*** READING CAN MESSAGE")

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
            _DebugWriter.AddMessage("bytes remaining " & _Port.BytesToRead)
            _DebugWriter.AddMessage("raw message " & Message)

            If Message.Length = 22 Then
                Tag = Message.Substring(2, 3)
                'CanData = Message.Substring(6, 16)
                For i As Integer = 20 To 6 Step -2
                    CanData &= Message.Substring(i, 2)
                Next
                Debug.Print("CANTAG " & Tag & " CANDATA " & CanData)
                _DebugWriter.AddMessage("cantag " & Tag & " candata " & CanData)

                If Tag.Substring(1, 2) <> "00" AndAlso _CANMessages.TryGetValue(Tag, CurrentMessage) Then
                    CurrentMessage.NewDataValue = New cCANData(CanData)
                    If My.Settings.EnableDebug Then
                        For Each datafield As cDataField In CurrentMessage.CANFields
                            _DebugWriter.AddMessage("field " & datafield.FieldName & " value " & datafield.DataValueAsString)
                        Next
                    End If
                End If
            Else
                _ErrorWriter.AddMessage("Invalid CAN packet received from COM port: " & Message)
            End If

        Catch timeoutEx As System.TimeoutException
            _ErrorWriter.AddMessage("COM port read timed out while attempting to get CAN packet")
            ErrorDialog("COM port read timed out while attempting to get CAN packet", "COM Read Timeout")
        Catch ioEx As System.IO.IOException
            _ErrorWriter.AddMessage("COM port disconnected while attempting to get CAN packet")
            ErrorDialog("COM port disconnected while attempting to get CAN packet", "COM Port Disconnection")
            _COMConnected = False
        Catch invalidOpEx As System.InvalidOperationException
            _ErrorWriter.AddMessage("COM port closed while attempting to get CAN packet")
            ErrorDialog("COM port closed while attempting to get CAN packet", "COM Port Closed")
            _COMConnected = False
        Catch ex As Exception
            _ErrorWriter.AddMessage("Unexpected error - " & ex.Message & " while getting can message")
            ErrorDialog("Unexpected error - " & ex.Message & " while getting can message")
        End Try
    End Sub
    Private Sub SaveData()
        Dim GridScroll As Integer
        Try
            '
            '   This is where the collection of data is written to the database.
            '
            '   We will fill this in as our next example.  For now, we will update them
            '   in the grid on the form.
            '
            Debug.Print("Saving data Values")
            _DebugWriter.AddMessage("*** WRITING TO SQL DATABASE")

            ' Construct query string and update data grid
            _Values = "VALUES ("
            If My.Settings.EnableDebug Then
                GridScroll = DataGrid.FirstDisplayedScrollingRowIndex
                DataGrid.Rows.Clear()
            End If
            For Each CANMessage As CANMessageData In _CANMessages.Values
                For Each datafield As cDataField In CANMessage.CANFields
                    If My.Settings.EnableDebug Then
                        DataGrid.Rows.Add({datafield.FieldName, datafield.CANTag, datafield.CANByteOffset, datafield.DataValueAsString})
                    End If
                    _Values &= datafield.DataValueAsString & ","
                    datafield.Reset()
                Next
            Next
            If My.Settings.EnableDebug Then
                If GridScroll >= 0 Then
                    DataGrid.FirstDisplayedScrollingRowIndex = GridScroll
                End If
            End If
            _Values = _Values.Substring(0, _Values.Length - 1) & ")"
            _DebugWriter.AddMessage(_InsertCommand)
            _DebugWriter.AddMessage(_Values)

            ' Execute insert command
            Dim cmd As New SqlCommand
            With cmd
                .CommandText = _InsertCommand & _Values
                .CommandType = CommandType.Text
                .CommandTimeout = 0
                .Connection = _SQLConn
                .ExecuteNonQuery()
            End With

        Catch sqlEx As System.Data.SqlClient.SqlException
            _ErrorWriter.AddMessage("Error writing to SQL database: " & sqlEx.Errors(0).Message)
            ErrorDialog("Error writing to SQL database: " & sqlEx.Errors(0).Message, "SQL Write Error")
        Catch ex As Exception
            _ErrorWriter.AddMessage("Unexpected error - " & ex.Message & " while writing to database")
            ErrorDialog("Unexpected error - " & ex.Message & " while writing to database", "Unexpected Error")
        End Try
    End Sub
#End Region
#Region "Event Handlers"
    Private Sub Main_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        ' init error and debug loggers
        _ErrorWriter = New LogWriter("error_log " & Format(Now, "M-d-yyyy") & " " & Format(Now, "hh.mm.ss tt") & ".txt")
        _ErrorWriter.ClearLog()
        _DebugWriter = New LogWriter("debug_log " & Format(Now, "M-d-yyyy") & " " & Format(Now, "hh.mm.ss tt") & ".txt", My.Settings.EnableDebug)
        _DebugWriter.ClearLog()
        OpenSqlConnection()

        ' init COM communications and begin reading
        Try
            If LoadCANFields() Then
                InitInsertStatement()
                ConfigureCOMPort()
                SaveDataTimer.Interval = My.Settings.ValueStorageInterval
                SaveDataTimer.Enabled = True
                CANRead_BW.RunWorkerAsync()
            End If
        Catch ex As Exception
            _ErrorWriter.AddMessage("Unexpected error - " & ex.Message & " while Loading form")
            ErrorDialog("Unexpected error - " & ex.Message & " while Loading form", "Unexpected Error")
        End Try

    End Sub
    Private Sub SaveDataTimer_Tick(sender As Object, e As System.EventArgs) Handles SaveDataTimer.Tick
        SaveData()
        _ErrorWriter.WriteAll()
        _DebugWriter.WriteAll()
    End Sub
    Private Sub btnClose_Click(sender As Object, e As System.EventArgs) Handles btnClose.Click
        CloseSqlConnection()
        Me.Close()
    End Sub
    Private Sub CANRead_BW_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles CANRead_BW.DoWork
        While (True)
            If Not chkPause.Checked Then
                If _COMConnected Then
                    GetCANMessage()
                Else
                    ConfigureCOMPort()
                End If
            End If
        End While
    End Sub
    Private Sub ResumePollingReset(sender As Object, e As System.EventArgs) Handles chkPause.CheckedChanged
        If Not chkPause.Checked Then
            _Port.DiscardInBuffer() ' Clear the serial port when we resume polling so that we are getting the most recent can packets.
        End If
    End Sub
#End Region
End Class
