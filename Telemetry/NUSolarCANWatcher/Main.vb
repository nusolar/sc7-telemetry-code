Imports System.Data.SqlClient
Imports System.IO.Ports
Imports System.Collections.Concurrent
Imports System.Threading

Public Class Main
    ' log variables
    Private _ErrorWriter As LogWriter
    Private _DebugWriter As LogWriter
    Private _LogState As LogState
    Private _LogThread As Thread

    ' COM variables
    Private _Port As SerialPort
    Private _LastCOMWrite As Long
    Private _COMState As COMState
    Private _COMThread As Thread
    Private _COMLock As New Object

    ' SQL variables
    Private _SQLConn As SqlConnection
    Private _InsertCommand As String
    Private _Values As String
    Private _CANMessages As ConcurrentDictionary(Of String, CANMessageData)
    Private _SQLState As SQLState
    Private _SQLThread As Thread

    ' state enums
    Private Enum LogState
        OPEN
        RUN
        CLOSE
        QUIT
    End Enum

    Private Enum COMState
        OPEN
        RUN
        CLOSE
        QUIT
    End Enum

    Private Enum SQLState
        OPEN
        RUN
        CLOSE
        QUIT
    End Enum

    ' object representing data for one CAN tag
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

    ' log writer object
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
                _Messages.Enqueue(Format(DateAndTime.Now, "MM/dd/yyyy hh:mm:ss.fff tt") & vbTab & Message & vbNewLine)
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

#Region "Private Methods"
    Private Function OpenSqlConnection() As Boolean
        _DebugWriter.AddMessage("*** OPENING SQL CONNECTION")

        Try
            _SQLConn = New SqlConnection(My.Settings.DSN)
            _SQLConn.Open()
            Return True
        Catch sqlEx As System.Data.SqlClient.SqlException
            _ErrorWriter.AddMessage("Error opening SQL connection: " & sqlEx.Errors(0).Message)
            _ErrorWriter.WriteAll()
            Return False
        Catch ex As Exception
            _ErrorWriter.AddMessage("Unexpected error - " & ex.Message & ", while opening SQL connection")
            _ErrorWriter.WriteAll()
            Return True
        End Try
    End Function
    Private Sub CloseSqlConnection()
        _DebugWriter.AddMessage("*** CLOSING SQL CONNECTION")

        Try
            _SQLConn.Close()
        Catch sqlEx As System.Data.SqlClient.SqlException
            _ErrorWriter.AddMessage("Error closing SQL connection: " & sqlEx.Errors(0).Message)
            _ErrorWriter.WriteAll()
        Catch ex As Exception
            _ErrorWriter.AddMessage("Unexpected error - " & ex.Message & ", while closing SQL connection")
            _ErrorWriter.WriteAll()
        End Try
    End Sub

    Private Function OpenCOMPort() As Boolean
        _DebugWriter.AddMessage("*** OPENING COM PORT")

        'Get current port names
        Dim COMPorts As List(Of String)

        'Basic Setup
        _Port = New SerialPort()
        _Port.BaudRate = My.Settings.BaudRate
        _Port.DataBits = 8
        _Port.Parity = Parity.None
        _Port.StopBits = 1
        _Port.Handshake = False
        _Port.ReadTimeout = My.Settings.COMTimeout
        _Port.WriteTimeout = 500

        ' Get list of current ports
        COMPorts = (SerialPort.GetPortNames).ToList
        COMPorts.Insert(0, My.Settings.COMPort) ' Insert Predefined COMPort to try

        For Each port As String In COMPorts
            Try
                _Port.PortName = port
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
                    _DebugWriter.AddMessage("opened " & _Port.PortName)
                    Return True
                End If
            Catch connEx As System.IO.IOException
                _ErrorWriter.AddMessage("Unable to connect to " & My.Settings.COMPort)
                _ErrorWriter.WriteAll()
            Catch accessEx As System.UnauthorizedAccessException
                _ErrorWriter.AddMessage("Access Denied. Failed to open " & My.Settings.COMPort)
                _ErrorWriter.WriteAll()
            Catch ex As Exception
                _ErrorWriter.AddMessage("Unexpected error - " & ex.Message & ", while connecting to COM port")
                _ErrorWriter.WriteAll()
            End Try
        Next port

        Return False
    End Function
    Private Sub CloseCOMPort()
        _DebugWriter.AddMessage("*** CLOSING COM PORT")

        Try
            _Port.Close()
        Catch ioEx As System.IO.IOException
            _ErrorWriter.AddMessage("Uanble to close COM port.")
            _ErrorWriter.WriteAll()
        Catch ex As Exception
            _ErrorWriter.AddMessage("Unexpected error: " & ex.Message & ", while closing COM port.")
            _ErrorWriter.WriteAll()
        End Try
    End Sub

    Private Function LoadCANFields() As Boolean
        _DebugWriter.AddMessage("*** LOADING SQL DATABASE")

        ' read CAN fields from database
        LoadCANFields = False
        Try
            Dim LastCANTag As String = ""
            Dim CANMessage As CANMessageData = Nothing ' CANMessageData contains all columns for one tag
            _CANMessages = New ConcurrentDictionary(Of String, CANMessageData) ' will hold all CANMessage data objects

            Dim cmd As New SqlCommand
            With cmd
                .CommandText = "p_GetCANFields"
                .CommandType = CommandType.StoredProcedure
                .CommandTimeout = 0
                .Connection = _SQLConn
                Dim dr As SqlDataReader = .ExecuteReader
                Do While dr.Read
                    Dim data As New cDataField(dr) ' cDataField represents column in database
                    _DebugWriter.AddMessage("cantag " & data.CANTag & " field " & data.FieldName)

                    If data.CANTag <> LastCANTag Then ' new CAN tag
                        If Not CANMessage Is Nothing Then
                            _CANMessages.TryAdd(CANMessage.CANTag, CANMessage) ' add tag info object
                        End If
                        CANMessage = New CANMessageData ' new tag info object
                        CANMessage.CANTag = data.CANTag
                        LastCANTag = data.CANTag
                    End If
                    CANMessage.CANFields.Add(data) ' add column information
                Loop
                dr.Close()
                If Not CANMessage Is Nothing Then
                    _CANMessages.TryAdd(CANMessage.CANTag, CANMessage)
                End If
            End With

            ' init insert query
            _InsertCommand = "INSERT INTO tblHistory ("

            For Each message As CANMessageData In _CANMessages.Values
                For Each datafield As cDataField In message.CANFields
                    _InsertCommand &= datafield.FieldName & ", "
                Next
            Next

            _InsertCommand = _InsertCommand.Substring(0, _InsertCommand.Length - 2) & ") "

            ' done
            LoadCANFields = True
        Catch sqlEx As System.Data.SqlClient.SqlException
            _ErrorWriter.AddMessage("Error loading SQL database: " & sqlEx.Errors(0).Message)
            _ErrorWriter.WriteAll()
        Catch ex As Exception
            _ErrorWriter.AddMessage("Unexpected error - " & ex.Message & ", while loading CAN Field SQL database")
            _ErrorWriter.WriteAll()
        End Try
    End Function
    Private Function GetCANMessage() As Boolean
        Dim Message As String = ""
        Dim Tag As String = ""
        Dim CanData As String = ""
        Dim CurrentMessage As CANMessageData = Nothing
        GetCANMessage = True

        ' read message
        _DebugWriter.AddMessage("*** READING CAN MESSAGE")
        Try
            Message = _Port.ReadTo(";")
            _DebugWriter.AddMessage("bytes remaining " & _Port.BytesToRead)
            _DebugWriter.AddMessage("raw message " & Message)

            If Message.Length = 22 Then
                Tag = Message.Substring(2, 3)
                For i As Integer = 20 To 6 Step -2 ' bytes are read from COM port in reverse order
                    CanData &= Message.Substring(i, 2)
                Next
                _DebugWriter.AddMessage("cantag " & Tag & " candata " & CanData)

                If _CANMessages.TryGetValue(Tag, CurrentMessage) Then
                    CurrentMessage.NewDataValue = New cCANData(CanData) ' update value of tag info object
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
        Catch ioEx As System.IO.IOException
            _ErrorWriter.AddMessage("COM port disconnected while attempting to get CAN packet")
            GetCANMessage = False
        Catch invalidOpEx As System.InvalidOperationException
            _ErrorWriter.AddMessage("COM port closed while attempting to get CAN packet")
            GetCANMessage = False
        Catch ex As Exception
            _ErrorWriter.AddMessage("Unexpected error - " & ex.Message & " while getting can message")
        End Try
    End Function
    Private Sub SaveData()
        Dim GridScroll As Integer
        Try
            _DebugWriter.AddMessage("*** WRITING TO SQL DATABASE")

            ' Construct query string and update data grid
            _Values = "VALUES ("
            If My.Settings.EnableDebug Then
                ' GridScroll = DataGrid.FirstDisplayedScrollingRowIndex
                ' DataGrid.Rows.Clear()
            End If
            For Each CANMessage As CANMessageData In _CANMessages.Values
                For Each datafield As cDataField In CANMessage.CANFields
                    If My.Settings.EnableDebug Then
                        ' DataGrid.Rows.Add({datafield.FieldName, datafield.CANTag, datafield.CANByteOffset, datafield.DataValueAsString})
                    End If
                    _Values &= datafield.DataValueAsString & ","
                    datafield.Reset()
                Next
            Next
            If My.Settings.EnableDebug Then
                If GridScroll >= 0 Then ' force grid to stop scrolling to top after every update
                    ' DataGrid.FirstDisplayedScrollingRowIndex = GridScroll
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
        Catch ex As Exception
            _ErrorWriter.AddMessage("Unexpected error - " & ex.Message & " while writing to database")
        End Try
    End Sub
    Private Sub WriteCANMessage(ByVal sqlConn As Boolean)
        Dim message As String = ":S" & My.Settings.TelStatusID & "N"

        ' add SQL connected and COM connected bits
        If sqlConn Then
            message &= "03"
        Else
            message &= "02"
        End If

        ' Fill rest of message with 0's
        message &= "00000000000000;"

        ' Send the message out over CAN.
        Try
            _Port.Write(message)
        Catch timeoutEx As System.TimeoutException
            _ErrorWriter.AddMessage("COM port timed out while writing CAN packet")
        Catch ioEx As System.ArgumentNullException
            _ErrorWriter.AddMessage("Invalid String: '" & message & "' writen to COM port")
        Catch invalidOpEx As System.InvalidOperationException
            _ErrorWriter.AddMessage("COM port closed while writing CAN packet")
        Catch ex As Exception
            _ErrorWriter.AddMessage("Unexpected error: " & ex.Message & ", while writing CAN packet")
        End Try
    End Sub
#End Region
#Region "Event Handlers"
    Private Sub Main_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        ' init state
        _COMState = COMState.OPEN
        _SQLState = SQLState.OPEN
        _LogState = LogState.OPEN

        ' init error and debug loggers
        _ErrorWriter = New LogWriter("error_log " & Format(Now, "M-d-yyyy") & " " & Format(Now, "hh.mm.ss tt") & ".txt", True)
        _ErrorWriter.ClearLog()
        _DebugWriter = New LogWriter("debug_log " & Format(Now, "M-d-yyyy") & " " & Format(Now, "hh.mm.ss tt") & ".txt", My.Settings.EnableDebug)
        _DebugWriter.ClearLog()
        _LogState = LogState.RUN
        _LogThread = New Thread(AddressOf RunLog)
        _LogThread.Start()

        ' open COM port
        If OpenCOMPort() Then
            _COMState = COMState.RUN
        End If
        _LastCOMWrite = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond
        _COMThread = New Thread(AddressOf RunCOM)
        _COMThread.Start()

        ' open SQL and load data fields
        If OpenSqlConnection() Then
            If LoadCANFields() Then
                _SQLState = SQLState.RUN
            Else
                CloseSqlConnection()
            End If
        End If
        _SQLThread = New Thread(AddressOf RunSQL)
        _SQLThread.Start()
    End Sub
    Private Sub btnClose_Click(sender As Object, e As System.EventArgs) Handles btnClose.Click
        ' request close of COM
        SyncLock _COMLock
            If _COMState <> COMState.OPEN Then
                _COMState = COMState.CLOSE
            Else
                _COMState = COMState.QUIT
            End If
        End SyncLock

        ' request close of SQL
        If _SQLState <> SQLState.OPEN Then
            _SQLState = SQLState.CLOSE
        Else
            _SQLState = SQLState.QUIT
        End If

        ' wait for log/COM/SQL threads to finish
        _COMThread.Join()
        _SQLThread.Join()

        ' dump logs and end log thread
        _ErrorWriter.WriteAll()
        _DebugWriter.WriteAll()
        _LogState = LogState.CLOSE
        _LogThread.Join()
    End Sub
    Private Sub RunLog()
        While True
            Select Case _LogState
                Case LogState.RUN
                    _ErrorWriter.WriteAll()
                    _DebugWriter.WriteAll()
                    Thread.Sleep(My.Settings.LogWriteInterval)
                Case LogState.CLOSE
                    _LogState = LogState.QUIT
                Case LogState.QUIT
                    Exit While
                Case Else
                    Thread.Sleep(My.Settings.LogIdleInterval)
            End Select
        End While
    End Sub
    Private Sub RunCOM()
        Dim currentMillis As Long = _LastCOMWrite

        While True
            SyncLock _COMLock
                Select Case _COMState
                    Case COMState.OPEN
                        If OpenCOMPort() Then
                            _COMState = COMState.RUN
                        End If
                    Case COMState.RUN
                        If Not GetCANMessage() Then ' COM disconnected
                            _COMState = COMState.OPEN
                        Else ' check if CAN status needs to be transmitted
                            currentMillis = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond
                            If currentMillis - _LastCOMWrite >= My.Settings.COMWriteInterval Then
                                WriteCANMessage(_SQLState <> SQLState.OPEN)
                                _LastCOMWrite = currentMillis
                            End If
                        End If
                    Case COMState.CLOSE
                        CloseCOMPort()
                        _COMState = COMState.QUIT
                    Case COMState.QUIT
                        Exit While
                End Select
            End SyncLock
        End While
    End Sub
    Private Sub RunSQL()
        While True
            Select Case _SQLState
                Case SQLState.RUN
                    SaveData()
                    Thread.Sleep(My.Settings.SQLWriteInterval)
                Case SQLState.CLOSE
                    CloseSqlConnection()
                    _SQLState = SQLState.QUIT
                Case SQLState.QUIT
                    Exit While
                Case Else
                    Thread.Sleep(My.Settings.SQLIdleInterval)
            End Select
        End While
    End Sub
#End Region
End Class
