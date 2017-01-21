Imports System.Net
Imports System.Net.Sockets
Imports System.Text
Imports System.IO
Public Class ServerForm
    Private _Clients As New Collection
    Private _SyncLockObject As New Object
    Private _Server As Server = Nothing
    Private _ServerThread As System.Threading.Thread = Nothing
    Private Delegate Sub UpdateStatusDelegate(ByVal StatusText As String)
    Class Clients
        Public Key As String = ""
        Public Client As TcpClient
        Public Stream As NetworkStream
        Public Buffer(10000) As Byte
        Public EnableSending As Boolean
    End Class
    Private Class Server
        Private _Address As IPAddress
        Private _Port As Integer
        Private _Owner As ServerForm
        Private _stream As NetworkStream = Nothing
        Private _listener As TcpListener = Nothing
        Private _ClientObject As Clients = Nothing
        Private _ClientConnected As New Threading.ManualResetEvent(False)
        Private _NoListen As Boolean = True
        Private _SyncLockObject As New Object

        Public Property NoListen As Boolean
            Get
                SyncLock _SyncLockObject
                    NoListen = _NoListen
                End SyncLock
            End Get
            Set(value As Boolean)
                SyncLock _SyncLockObject
                    _NoListen = value
                    If _NoListen Then
                        _listener.Stop()
                        _ClientConnected.Set()
                    End If
                End SyncLock
            End Set
        End Property
        Private Sub DoAcceptTcpClientCallback(ar As IAsyncResult)
            If Not ar Is Nothing Then
                Dim listener As TcpListener = CType(ar.AsyncState, TcpListener)
                Try
                    Dim client As TcpClient = listener.EndAcceptTcpClient(ar)
                    _ClientObject = New Clients
                    With _ClientObject
                        .Client = client
                        .EnableSending = True
                        .Key = client.Client.RemoteEndPoint.ToString
                    End With
                    _Owner.AddClient(_ClientObject)
                    _ClientConnected.Set()
                Catch
                End Try
            End If
        End Sub
        Public Sub Listen()
            Try
                _NoListen = False
                Dim KeepListening As Boolean = Not _NoListen
                _listener = New TcpListener(_Address, _Port)
                _listener.Start()
                Do While KeepListening
                    If KeepListening Then
                        _ClientConnected.Reset()
                        _listener.BeginAcceptTcpClient(New AsyncCallback(AddressOf DoAcceptTcpClientCallback), _listener)
                        _ClientConnected.WaitOne()
                        SyncLock _SyncLockObject
                            KeepListening = Not _NoListen
                        End SyncLock
                        If KeepListening Then
                            InitiateRead(_ClientObject)
                        End If
                    End If
                Loop
                _listener.Stop()
            Catch ex As Threading.ThreadAbortException
                '
                '   Shutting down, just exit
                '

            Catch ex As Exception
                _Owner.UpdateStatus("Unexpected error: " & ex.Message & " while processing client connection requests")
            Finally
            End Try
        End Sub
        Private Sub InitiateRead(ByVal Client As Clients)
            Try
                Client.Stream = Client.Client.GetStream
                If Client.Stream.CanRead Then
                    Client.Stream.BeginRead(Client.Buffer, 0, Client.Buffer.Length, AddressOf ReadComplete, Client)
                End If
            Catch ex As Exception
                Client.EnableSending = False
            End Try
        End Sub
        Private Sub ReadComplete(result As IAsyncResult)
            Try
                Dim client As Clients = CType(result.AsyncState, Clients)
                Dim bytesread As Integer = client.Stream.EndRead(result)
                Dim buffer As String = ""
                buffer = [String].Concat(buffer, Encoding.ASCII.GetString(client.Buffer, 0, bytesread))
                Dim command() As String = Split(buffer, " ")
                Select Case command(0).ToUpper
                    Case "SENDALL"
                        client.EnableSending = True
                    Case "PAUSE"
                        client.EnableSending = False
                        _Owner.UpdateStatus("Sending PAUSED for client: " & client.Key)
                    Case "RESUME"
                        client.EnableSending = True
                        _Owner.UpdateStatus("Sending RESUMED for client: " & client.Key)
                    Case Else
                        If command(0).ToUpper.Length <> 0 Then
                            _Owner.UpdateStatus("Unknown command: " & command(0).ToUpper & " received")
                        End If
                End Select
                '
                '   Write response
                '
                If client.Stream IsNot Nothing Then
                    Dim stream As NetworkStream = client.Stream
                    If stream.CanWrite Then
                        Dim data As [Byte]() = System.Text.Encoding.ASCII.GetBytes("OK" & vbLf)
                        stream.Write(data, 0, data.Length)
                    End If
                    InitiateRead(client)
                End If

            Catch ex As Exception
                Dim client As Clients = CType(result.AsyncState, Clients)
                client.EnableSending = False
            End Try
        End Sub
        Public Sub New(ByVal Owner As Form, ByVal Address As String, ByVal Port As Integer)
            _Owner = Owner
            _Address = IPAddress.Parse(Address)
            _Port = Port
        End Sub
    End Class
    Public Sub UpdateStatus(ByVal Status As String)
        '
        '   If called on the wrong thread, invoke self to get onto the correct thread
        '
        If Me.InvokeRequired Then
            Me.Invoke(New UpdateStatusDelegate(AddressOf UpdateStatus), Status)
            Exit Sub
        End If
        StatusText.AppendText(Date.Now.ToString("MM/dd/yyyy HH:mm:ss") & " - " & Status & vbCrLf)
    End Sub
    Public Sub AddClient(ByVal Client As Clients)
        Try
            SyncLock _SyncLockObject
                _Clients.Add(Client, Client.Key)
                UpdateStatus("Client connection accepted from " & Client.Key)
            End SyncLock
        Catch ex As Exception
            MessageBox.Show("Unexpected error: " & ex.Message & " while adding a new client", "Unexpected error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub btnStart_Click(sender As Object, e As System.EventArgs) Handles btnStart.Click
        If btnStart.Text = "Start" Then
            _Server = New Server(Me, My.Settings.ServerAddress, My.Settings.ServerPort)
            _ServerThread = New System.Threading.Thread(AddressOf _Server.Listen)
            _ServerThread.Start()
            Timer1.Interval = My.Settings.Timerspeed
            Timer1.Start()
            btnStart.Text = "Stop"
            UpdateStatus("Waiting for connections")
        Else
            _Server.NoListen = True
            Timer1.Stop()
            If _Clients.Count > 0 Then
                For Each Client As Clients In _Clients
                    If Client.Client.Connected And Client.EnableSending Then
                        Client.Client.Close()
                    End If
                Next
            End If
            _ServerThread = Nothing
            btnStart.Text = "Start"
            UpdateStatus("Stopped")
        End If
    End Sub
    Private Sub Timer1_Tick(sender As Object, e As System.EventArgs) Handles Timer1.Tick
        SyncLock _SyncLockObject
            Dim stream As NetworkStream = Nothing

            If _Clients.Count > 0 Then
                Dim data As [Byte]() = System.Text.Encoding.ASCII.GetBytes("NEW," & "TS01=" & Date.Now.ToString("MM/dd/yyyy HH:mm:ss"))
                For Each Client As Clients In _Clients
                    If Client.Client.Connected And Client.EnableSending Then
                        Try
                            stream = Client.Client.GetStream
                            stream.Write(data, 0, data.Length)
                        Catch ex As Exception
                            Stop
                        End Try
                    Else
                        If Not Client.Client.Connected Then
                            _Clients.Remove(Client.Key)
                            UpdateStatus("Client connection dropped by " & Client.Key)
                        End If
                    End If
                Next
            End If

        End SyncLock
    End Sub
    Private Sub Form1_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
        If Not _ServerThread Is Nothing Then
            _ServerThread.Abort()
            _ServerThread = Nothing
        End If
    End Sub

    Private Sub ServerForm_Load(sender As Object, e As EventArgs) Handles Me.Load
        HostName.Text = Dns.GetHostName
        cboIPAddresses.Items.Clear()
        cboIPAddresses.Items.Add("127.0.0.1")
        For Each ip As IPAddress In Dns.GetHostAddresses(HostName.Text)
            If ip.AddressFamily = AddressFamily.InterNetwork Then
                cboIPAddresses.Items.Add(ip.ToString)
            End If
        Next
        cboIPAddresses.SelectedItem = My.Settings.ServerAddress
    End Sub

    Private Sub cboIPAddresses_TextChanged(sender As Object, e As EventArgs) Handles cboIPAddresses.TextChanged
        My.Settings.ServerAddress = cboIPAddresses.SelectedItem
    End Sub
End Class
