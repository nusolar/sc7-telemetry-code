Imports System.Net
Imports System.Net.Sockets
Imports System.Data
Imports System.Data.Common
Imports System.Data.Odbc
Imports System.IO
Imports System.Text
Imports System.Threading
Public Class ServerMain
    Private Const k_ProtocolLogSnippitSize As Integer = 50
    Private Const k_TimestampFormat As String = "dd/MM/yyyy HH:mm:ss.ffff"
    Private _data As DataTable
    Private _row As Integer
    Private _startrow As Integer
    Private _Clients As New Collection
    Private _Server As Server = Nothing
    Private _Client As Client = Nothing
    Private _ServerThread As System.Threading.Thread = Nothing
    Private _Address As IPAddress
    Private _Port As Integer
    Private _SyncLockObject As New Object
    Private _CurrentTelemetry As Telemetry
    Private Delegate Sub WriteProtocolLogDelegate(ByVal buffer As String, ByVal Inbound As Boolean)
    Private Delegate Sub ProcessUpdateDelegate(ByVal buffer As String)
    Private Delegate Sub ConnectionLostDelegate(ByVal Connected As Boolean)
    Private Delegate Sub WriteErrorLogDelegate(ByVal ErrorText As String)
    Private Class Client
        Private _Address As IPAddress
        Private _Port As Integer
        Private _Owner As ServerMain
        Private _client As TcpClient = Nothing
        Private _stream As NetworkStream = Nothing
        Private _buffer(100000) As Byte
        Private _Timer As Timer
        Public ReadTimeout As Integer = 0
        Public LogProtocol As Boolean = False
        Private Sub ConnectionTimeout(ByVal state As Object)
            Try
                _Owner.WriteErrorLog("Connection to solar car timed out")
                _client.Close()
                _Owner.ConnectionStatusChange(False)
                '
                '   Attempt reconnect
                '
                _Timer.Dispose()
                _Timer = New Timer(New TimerCallback(AddressOf Connect), Nothing, My.Settings.ReadTimeout, Threading.Timeout.Infinite)
            Catch ex As Exception
                _Owner.WriteErrorLog(ex.Message & " processing connecting timeout.")
            End Try
        End Sub
        Private Sub InitiateRead()
            Try
                _stream = _client.GetStream
                If _stream.CanRead Then
                    _stream.BeginRead(_buffer, 0, _buffer.Length, AddressOf ReadComplete, _stream)
                    _Timer.Dispose()
                    If ReadTimeout > 0 Then
                        _Timer = New Timer(New TimerCallback(AddressOf ConnectionTimeout), Nothing, ReadTimeout, Threading.Timeout.Infinite)
                    End If
                End If
            Catch ex As Exception
                _Owner.WriteErrorLog(ex.Message & " while initiating read from solar car, reconnecting.")

                _Timer = New Timer(New TimerCallback(AddressOf Connect), Nothing, My.Settings.ReadTimeout, 250)
            End Try
        End Sub
        Private Sub ReadComplete(ByVal result As IAsyncResult)
            Try
                Dim bytesread As Integer = _stream.EndRead(result)
                Dim buffer As String = ""
                _Timer.Dispose()
                buffer = [String].Concat(buffer, Encoding.ASCII.GetString(_buffer, 0, bytesread))
                If LogProtocol Then
                    _Owner.WriteProtocolLog(buffer, True)
                End If
                Dim messages() As String = Split(buffer, vbLf)
                For i As Integer = 0 To messages.GetUpperBound(0)
                    If messages(i).Length > 0 Then
                        If messages(i).StartsWith("OK") Then
                            ' Eat the OK
                        Else
                            If messages(i).StartsWith("NEW") Or messages(i).StartsWith("OLD") Then
                                If messages(i).Contains("TS01=") Then   ' Last field of message, full message
                                    _Owner.ProcessUpdate(messages(i))
                                Else
                                    _Owner.WriteErrorLog("Partial message ignored, " & messages(i))
                                End If
                            Else
                                _Owner.WriteErrorLog("Partial message ignored, " & messages(i))
                            End If
                        End If
                    End If
                Next
                InitiateRead()
            Catch ex As Exception
                _Owner.WriteErrorLog(ex.Message & " while processing update from solar car.")
            End Try
        End Sub
        Private Sub ConnectComplete(ByVal result As IAsyncResult)
            Try
                _Timer.Dispose()
                If Not _client Is Nothing Then
                    If _client.Connected Then
                        _Owner.WriteErrorLog("Connect complete.")
                        _client.EndConnect(result)
                        _Owner.ConnectionStatusChange(True)
                        InitiateRead()
                    Else
                        _Owner.WriteErrorLog("Connect complete called but client is not connected.")
                        _Timer = New Timer(New TimerCallback(AddressOf Connect), Nothing, My.Settings.ReadTimeout, 250)
                    End If
                Else
                    _Owner.WriteErrorLog("Connect complete called with null _client object.  Retrying connection attempt.")
                    _Timer = New Timer(New TimerCallback(AddressOf Connect), Nothing, My.Settings.ReadTimeout, 250)
                End If
            Catch ex As Exception
                _Owner.WriteErrorLog(ex.Message & " while processing connect complete.")
            End Try
        End Sub
        Public Sub SendResponse(ByVal Response As String)
            Try
                Dim stream As NetworkStream = _client.GetStream
                If stream.CanWrite Then
                    Dim data As [Byte]() = System.Text.Encoding.ASCII.GetBytes(Response)
                    stream.Write(data, 0, data.Length)
                    InitiateRead()
                End If
                If LogProtocol Then
                    _Owner.WriteProtocolLog(Response, False)
                End If
            Catch ex As Exception
                _Owner.WriteErrorLog(ex.Message & " while sending a response to the solar car.")
            End Try
        End Sub
        Public Sub Connect(ByVal state As Object)
            _client = New TcpClient
            Thread.Sleep(1000)
            _client.BeginConnect(_Address, _Port, AddressOf ConnectComplete, Nothing)
            If Not _Timer Is Nothing Then
                _Timer.Dispose()
            End If
            _Timer = New Timer(New TimerCallback(AddressOf ConnectionTimeout), Nothing, My.Settings.ReadTimeout, Threading.Timeout.Infinite)
        End Sub
        Public Sub New(ByVal Owner As ServerMain, ByVal Address As String, ByVal Port As Integer)
            _Owner = Owner
            _Address = IPAddress.Parse(Address)
            _Port = Port
        End Sub
    End Class
    Public Sub WriteProtocolLog(ByVal buffer As String, ByVal Inbound As Boolean)
        Try
            If Me.InvokeRequired Then
                Me.Invoke(New WriteProtocolLogDelegate(AddressOf WriteProtocolLog), buffer, Inbound)
                Exit Sub
            End If

            Dim log As String = CType(IIf(Inbound, ", <- ", ", -> "), String) & buffer.Substring(0, Math.Min(k_ProtocolLogSnippitSize, buffer.Length))
            If buffer.Length > k_ProtocolLogSnippitSize Then
                log &= "..."
            End If
            If Inbound Then
                log &= " (" & buffer.Length.ToString & " bytes)"
            End If
            File.AppendAllText(My.Application.Info.DirectoryPath & "\ProtocolLog.txt", Now.ToString(k_TimestampFormat) & log & vbCrLf)

        Catch ex As Exception
            WriteErrorLog(ex.Message & " while logging protocol with the car.")
        End Try
    End Sub
    Public Sub WriteErrorLog(ByVal ErrorText As String)
        Try
            If Me.InvokeRequired Then
                Me.Invoke(New WriteErrorLogDelegate(AddressOf WriteErrorLog), ErrorText)
                Exit Sub
            End If

            Dim message As String = Now.ToString(k_TimestampFormat) & ", " & ErrorText & vbCrLf
            txtTrace.AppendText(message)
            File.AppendAllText(My.Application.Info.DirectoryPath & "\ErrorLog.txt", message)

        Catch ex As Exception

        End Try
    End Sub
    Public Sub ConnectionStatusChange(ByVal Connected As Boolean)
        '
        '   If called on the wrong thread, invoke self to get onto the correct thread
        '
        If Me.InvokeRequired Then
            Me.Invoke(New ConnectionLostDelegate(AddressOf ConnectionStatusChange), Connected)
            Exit Sub
        End If
        '
        If Connected Then
            NotifyIcon1.Icon = My.Resources.Connected_Small
            NotifyIcon1.Text = "NUSolar telemetry server connected to solar car"
            ssConnectedStatus.Image = NotifyIcon1.Icon.ToBitmap
            ssConnectedStatus.Text = "Connected to solar car"
            If My.Settings.InitializeCarTimeOnStart Then
                InitializeCarClock()
                Thread.Sleep(5000)
            End If
            If Not chkPause.Checked Then
                SendAll()
            End If
        Else
            NotifyIcon1.Icon = My.Resources.Disconnected_Small
            NotifyIcon1.Text = "NUSolar telemetry server not connected to solar car"
            ssConnectedStatus.Image = NotifyIcon1.Icon.ToBitmap
            ssConnectedStatus.Text = "Not connected to solar car"
        End If
    End Sub
    Public Sub ProcessUpdate(ByVal Buffer As String)
        Try
            '
            '   If called on the wrong thread, invoke self to get onto the correct thread
            '
            If Me.InvokeRequired Then
                Me.Invoke(New ProcessUpdateDelegate(AddressOf ProcessUpdate), Buffer)
                Exit Sub
            End If
            '
            SyncLock _SyncLockObject
                If LogMessages Then
                    File.AppendAllText(My.Application.Info.DirectoryPath & "\Logfile.txt", Now.ToString(k_TimestampFormat) & ", " & Buffer & vbCrLf)
                End If
                _CurrentTelemetry.Serialized = Buffer
                _CurrentTelemetry.NewData = Buffer.StartsWith("NEW")
                tsRowNumber.Text = "Last row: " & _CurrentTelemetry.PropertyValue("RowNum").ToString
                _Client.SendResponse("RECEIVED " & _CurrentTelemetry.PropertyValue("RowNum").ToString)
                Timer1.Start()
            End SyncLock
        Catch ex As Exception
            MessageBox.Show("Unexpected error: " & ex.Message & " while processing update from solar car.", "Unexpected Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            WriteErrorLog(ex.Message & " while processing update from solar car.")
        End Try
    End Sub
    Private ReadOnly Property WriteNewRows As Boolean
        Get
            WriteNewRows = chkWriteNewRows.Checked
        End Get
    End Property
    Private ReadOnly Property LogMessages As Boolean
        Get
            LogMessages = chkLogMessages.Checked
        End Get
    End Property
    Private ReadOnly Property LogProtocol As Boolean
        Get
            LogProtocol = chkLogProtocol.Checked
        End Get
    End Property
    Public Sub AddClient(ByVal Client As TcpClient)
        Try
            SyncLock _SyncLockObject
                _Clients.Add(Client)
            End SyncLock
        Catch ex As Exception
            MessageBox.Show("Unexpected error: " & ex.Message & " while adding a new client", "Unexpected error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            WriteErrorLog(ex.Message & " while adding a new client")
        End Try
    End Sub
    Private Sub ConfigureBuffer()
        Dim TelemetryConfiguration As String = File.ReadAllText("Telemetry.xml", System.Text.Encoding.GetEncoding("iso-8859-1"))
        _CurrentTelemetry = New Telemetry(TelemetryConfiguration)
    End Sub
    Private Sub StartListening()
        Try
            ConnectionStatusChange(False)
            _Client = New Client(Me, My.Settings.CarAddress, My.Settings.CarPort)
            _Client.LogProtocol = LogProtocol
            _Client.Connect(Nothing)
        Catch ex As Exception
            MessageBox.Show("Unexpected error: " & ex.Message & " while initiating communications with solar car.", "Unexpected Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            WriteErrorLog(ex.Message & " while initiating communications with solar car.")
        End Try
    End Sub
    Private Sub PauseSending()
        _Client.ReadTimeout = 0
        _Client.SendResponse("PAUSE")
    End Sub
    Private Sub ResumeSending()
        _Client.ReadTimeout = My.Settings.ReadTimeout
        _Client.SendResponse("RESUME")
    End Sub
    Private Sub SendAll()
        _Client.ReadTimeout = My.Settings.ReadTimeout
        _Client.SendResponse("SENDALL")
    End Sub
    Private Sub InitializeCarClock()
        _Client.ReadTimeout = My.Settings.ReadTimeout
        _Client.SendResponse("SETDATE " & Date.Now.ToString("MM/dd/yy HH:mm:ss"))
    End Sub
    Private Sub Main_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        _ServerThread.Abort()
        _ServerThread = Nothing
    End Sub
    Private Sub Main_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Visible = False
        ConfigureBuffer()
        chkWriteNewRows.Checked = My.Settings.SaveToDatabase
        chkLogProtocol.Checked = My.Settings.LogProtocol
        chkLogMessages.Checked = My.Settings.LogMessages
        _Server = New Server(Me, My.Settings.ServerAddress, My.Settings.ServerPort)
        _ServerThread = New System.Threading.Thread(AddressOf _Server.Listen)
        _ServerThread.Start()
        Timer1.Interval = 1
        HelloTimer.Interval = 2500
        HelloTimer.Enabled = True
        StartListening()
    End Sub
    Private Sub HelloTimer_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles HelloTimer.Tick
        SyncLock _SyncLockObject
            Dim stream As NetworkStream = Nothing
            Dim data As [Byte]() = System.Text.Encoding.ASCII.GetBytes("PING")
            For Each Client As TcpClient In _Clients
                If Client.Connected Then
                    Try
                        stream = Client.GetStream
                        stream.Write(data, 0, data.Length)
                    Catch ex As Exception
                        WriteErrorLog(ex.Message & " while sending hello timer to clients.")
                    End Try
                Else

                End If
            Next
        End SyncLock
    End Sub
    Private Sub Timer1_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        Try
            SyncLock _SyncLockObject
                Dim stream As NetworkStream = Nothing
                If (CType(_CurrentTelemetry.PropertyValue("ts1"), Date) > Date.MinValue) Or (DateDiff(DateInterval.Year, CType(_CurrentTelemetry.PropertyValue("ts1"), Date), Date.Now) > 1) Or My.Settings.OverrideTimestamp Then
                    _CurrentTelemetry.PropertyValue("ts1") = Date.Now
                End If
                Dim data As [Byte]() = System.Text.Encoding.ASCII.GetBytes(_CurrentTelemetry.Serialized)
                If WriteNewRows Then
                    Dim cn As OdbcConnection = New OdbcConnection(My.Settings.DSN)
                    cn.Open()
                    _CurrentTelemetry.InsertRow(cn)
                    cn.Close()
                End If
                If _CurrentTelemetry.NewData Then
                    For Each Client As TcpClient In _Clients
                        If Client.Connected Then
                            Try
                                stream = Client.GetStream
                                stream.Write(data, 0, data.Length)
                            Catch ex As Exception
                                WriteErrorLog(ex.Message & " while sending data to clients.")
                            End Try
                        Else

                        End If
                    Next
                End If
                Timer1.Stop()
            End SyncLock
        Catch ex As Exception
            MessageBox.Show("Unexpected error: " & ex.Message & " while notifying clients of data update.", "Unexpected Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            WriteErrorLog(ex.Message & " while notifying clients of data update.")
        End Try
    End Sub

    Private Sub btnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Close()
    End Sub

    Private Sub chkPause_CheckStateChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkPause.CheckStateChanged
        If chkPause.Checked Then
            PauseSending()
        Else
            ResumeSending()
        End If
    End Sub

    Private Sub NotifyIcon1_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles NotifyIcon1.DoubleClick
        Visible = True
        WindowState = FormWindowState.Normal
    End Sub

    Private Sub ServerMain_SizeChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.SizeChanged
        If WindowState = FormWindowState.Minimized Then
            Visible = False
        End If
    End Sub

    Private Sub chkLogProtocol_CheckStateChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkLogProtocol.CheckStateChanged
        If Not _Client Is Nothing Then
            _Client.LogProtocol = LogProtocol
        End If
    End Sub
End Class
