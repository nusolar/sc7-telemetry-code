Imports System.Net
Imports System.Net.Sockets
Imports System.Text
Imports System.Threading
Public Class ClientForm
    Private _Client As Client = Nothing
    Private Delegate Sub StatusUpdateDelegate(ByVal buffer As String)
    Private Delegate Sub ConnectionLostDelegate(ByVal Connected As Boolean)
    Public Class Client
        Private _Address As IPAddress
        Private _Port As Integer
        Private _Owner As ClientForm
        Private _client As TcpClient = Nothing
        Private _stream As NetworkStream = Nothing
        Private _buffer(10000) As Byte
        Private _Timer As Timer
        Private ReadOnly Property Connected As Boolean
            Get
                Try
                    If _client IsNot Nothing Then
                        If _client.Client.Poll(0, SelectMode.SelectRead) Then
                            Dim buff(1) As Byte
                            If (_client.Client.Receive(buff, SocketFlags.Peek) = 0) Then
                                Connected = False
                            Else
                                Connected = True
                            End If
                        Else
                            Connected = True
                        End If
                    Else
                        Connected = False
                    End If
                Catch ex As Exception
                    Connected = False
                End Try

            End Get
        End Property
        Private Sub ConnectionTimeout()
            Try
                If _stream IsNot Nothing Then _stream.Close()
                _client.Close()
                _Owner.ConnectionStatusChange(False)
                '
                '   Attempt reconnect
                '
                _Timer.Dispose()
                _Timer = New Timer(New TimerCallback(AddressOf Connect), Nothing, My.Settings.ReadTimeout, Threading.Timeout.Infinite)
            Catch
            End Try
        End Sub
        Private Sub InitiateRead()
            _stream = _client.GetStream
            If _stream.CanRead Then
                _stream.BeginRead(_buffer, 0, _buffer.Length, AddressOf ReadComplete, _stream)
            End If
        End Sub
        Private Sub ReadComplete(ByVal result As IAsyncResult)
            Try
                If Connected Then
                    Dim bytesread As Integer = _stream.EndRead(result)
                    Dim buffer As String = ""
                    _Timer.Dispose()
                    buffer = [String].Concat(buffer, Encoding.ASCII.GetString(_buffer, 0, bytesread))
                    If buffer.Length > 0 Then
                        _Owner.ProcessUpdate(buffer)
                    End If
                    InitiateRead()
                Else
                    _stream.Close()
                    _client.Close()
                    _stream = Nothing
                    _client = Nothing
                    _Owner.ConnectionStatusChange(False)
                End If
            Catch ex As Exception
                _Owner.ConnectionStatusChange(False)
            End Try
        End Sub
        Private Sub ConnectComplete(ByVal result As IAsyncResult)
            Try
                _Timer.Dispose()
                If _client.Connected Then
                    _client.EndConnect(result)
                    _Owner.ConnectionStatusChange(True)
                    InitiateRead()
                Else
                    _Owner.ConnectionStatusChange(False)
                    _Timer = New Timer(New TimerCallback(AddressOf Connect), Nothing, My.Settings.ReadTimeout, Threading.Timeout.Infinite)
                End If
            Catch ex As Exception
                _Owner.UpdateStatus("Unexpected error: " & ex.Message & " while processing ConnectComplete.")
            End Try
        End Sub
        Public Sub Connect()
            Try
                If _client IsNot Nothing Then
                    _client.Close()
                    _client = Nothing
                End If
                _client = New TcpClient
                _client.ReceiveTimeout = My.Settings.ReadTimeout
                _client.SendTimeout = My.Settings.ReadTimeout
                _client.BeginConnect(_Address, _Port, AddressOf ConnectComplete, Nothing)
                If Not _Timer Is Nothing Then
                    _Timer.Dispose()
                End If
                '
                '   Setup a timer to detect timeout on connection attempt
                '
                _Timer = New Timer(New TimerCallback(AddressOf ConnectionTimeout), Nothing, My.Settings.ReadTimeout, Threading.Timeout.Infinite)
            Catch ex As Exception
                _Owner.UpdateStatus("Unexpected error: " & ex.Message & " while processing Connect.")
            End Try
        End Sub
        Public Sub Send(ByVal Buffer As String)
            Try
                If _client IsNot Nothing Then
                    If Connected Then
                        Dim writestream As NetworkStream = _client.GetStream
                        If writestream.CanWrite Then
                            writestream.Write(System.Text.Encoding.ASCII.GetBytes(Buffer.ToArray, 0, Buffer.Length), 0, Buffer.Length)
                            _Owner.UpdateStatus(Buffer & " sent")
                        End If
                    Else
                        _Owner.ConnectionStatusChange(False)
                    End If
                End If
            Catch ex As Exception
                _Owner.UpdateStatus("Unexpected error: " & ex.Message & " while processing Send.")
            End Try
        End Sub
        Public Sub Close()
            _client.Close()
        End Sub
        Private Function IsIPAddress(ByVal Address As String) As Boolean
            Try
                Dim ip As IPAddress = IPAddress.Parse(Address)
                Return True
            Catch ex As Exception
                Return False
            End Try
        End Function

        Public Sub New(ByVal Owner As ClientForm, ByVal Address As String, ByVal Port As Integer)
            _Owner = Owner
            If IsIPAddress(Address) Then
                _Address = IPAddress.Parse(Address)
            Else
                For Each ip As IPAddress In Dns.GetHostEntry(Address).AddressList
                    If ip.AddressFamily = AddressFamily.InterNetwork Then
                        _Address = ip
                        Exit For
                    End If
                Next
            End If
            _Port = Port
        End Sub
    End Class
    Public Sub UpdateStatus(ByVal StatusText As String)
        '
        '   If called on the wrong thread, invoke self to get onto the correct thread
        '
        If Me.InvokeRequired Then
            Me.Invoke(New StatusUpdateDelegate(AddressOf UpdateStatus), StatusText)
            Exit Sub
        End If
        ServerData.AppendText(Date.Now.ToString("MM/dd/yyyy HH:mm:ss") & " - " & StatusText & vbCrLf)
    End Sub
    Public Sub ProcessUpdate(ByVal Buffer As String)
        Try
            UpdateStatus(Buffer)
        Catch ex As Exception
            MessageBox.Show("Unexpected error: " & ex.Message & " while processing update from  server.", "Unexpected Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
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
            ConnectionStatus.Text = "Connected"
            btnConnect.Text = "Disconnect"
            btnPause.Text = "Pause"
            btnPause.Enabled = True
        Else
            ConnectionStatus.Text = "Not connected"
            btnConnect.Text = "Connect"
            btnPause.Text = "Pause"
            btnPause.Enabled = False
        End If
        Application.DoEvents()
    End Sub
    Private Sub StartListening()
        Try
            ConnectionStatusChange(False)
            _Client = New Client(Me, ServerHostName.Text, My.Settings.ServerPort)
            _Client.Connect()
        Catch ex As Exception
            MessageBox.Show("Unexpected error: " & ex.Message & " while initiating communications with telemetry server.", "Unexpected Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub StopListening()
        If _Client IsNot Nothing Then
            _Client.Close()
            _Client = Nothing
        End If
    End Sub
    Private Sub btnConnect_Click(sender As Object, e As EventArgs) Handles btnConnect.Click
        Select Case btnConnect.Text
            Case "Connect"
                StartListening()
            Case "Disconnect"
                StopListening()
                ConnectionStatus.Text = "Not connected"
                btnConnect.Text = "Connect"
                btnPause.Text = "Pause"
                btnPause.Enabled = False
        End Select

    End Sub

    Private Sub ClientForm_Load(sender As Object, e As EventArgs) Handles Me.Load
        ServerHostName.Text = My.Settings.ServerName
        btnPause.Enabled = False
    End Sub

    Private Sub ServerHostName_Validated(sender As Object, e As EventArgs) Handles ServerHostName.Validated
        My.Settings.ServerName = ServerHostName.Text
    End Sub
    Private Sub btnPause_Click(sender As Object, e As EventArgs) Handles btnPause.Click
        If btnPause.Text = "Pause" Then
            _Client.Send("PAUSE")
            btnPause.Text = "Resume"
        Else
            _Client.Send("RESUME")
            btnPause.Text = "Pause"
        End If
    End Sub
End Class
