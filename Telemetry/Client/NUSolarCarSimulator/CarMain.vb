Imports System.Net
Imports System.Net.Sockets
Imports System.Text
Imports System.IO
Public Class CarMain
    Private _Clients As New Collection
    Private _SyncLockObject As New Object
    Private _Server As Server = Nothing
    Private _Messages() As String
    Private _Line As Integer
    Private _ServerThread As System.Threading.Thread = Nothing
    Class Clients
        Public Client As TcpClient
        Public Stream As NetworkStream
        Public Buffer(10000) As Byte
        Public EnableSending As Boolean
        Public LastRow As Integer
    End Class
    Private Class Server
        Private _Address As IPAddress
        Private _Port As Integer
        Private _Owner As CarMain
        Private _stream As NetworkStream = Nothing
        Private _listener As TcpListener = Nothing
        Private _ClientObject As Clients = Nothing
        Private _ClientConnected As New Threading.ManualResetEvent(False)

        Private Sub DoAcceptTcpClientCallback(ar As IAsyncResult)
            If Not ar Is Nothing Then
                Dim listener As TcpListener = CType(ar.AsyncState, TcpListener)
                Try
                    Dim client As TcpClient = listener.EndAcceptTcpClient(ar)
                    _ClientObject = New Clients
                    With _ClientObject
                        .Client = client
                        .EnableSending = False
                        .LastRow = 0
                    End With
                    _Owner.AddClient(_ClientObject)
                    _ClientConnected.Set()
                Catch
                End Try
            End If
        End Sub
        Public Sub Listen()
            Try
                _listener = New TcpListener(_Address, _Port)
                _listener.Start()
                Do While True
                    _ClientConnected.Reset()
                    _listener.BeginAcceptTcpClient(New AsyncCallback(AddressOf DoAcceptTcpClientCallback), _listener)
                    _ClientConnected.WaitOne()
                    InitiateRead(_ClientObject)
                Loop
            Catch ex As Threading.ThreadAbortException
                '
                '   Shutting down, just exit
                '
                _listener.Stop()
                Exit Sub
            Catch ex As Exception
                MessageBox.Show("Unexpected error: " & ex.Message & " while processing server requests", "Unexpected error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub
        Private Sub InitiateRead(ByVal Client As Clients)
            Client.Stream = Client.Client.GetStream
            If Client.Stream.CanRead Then
                Client.Stream.BeginRead(Client.Buffer, 0, Client.Buffer.Length, AddressOf ReadComplete, Client)
            End If
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
                    Case "RESUME"
                        client.EnableSending = False
                    Case "RECEIVED"
                        client.LastRow = CInt(command(1))
                End Select
                Dim stream As NetworkStream = client.Stream
                If Stream.CanWrite Then
                    Dim data As [Byte]() = System.Text.Encoding.ASCII.GetBytes("OK" & vbLf)
                    Stream.Write(data, 0, data.Length)
                End If
                InitiateRead(client)
            Catch ex As Exception

            End Try
        End Sub

        Public Sub New(ByVal Owner As CarMain, ByVal Address As String, ByVal Port As Integer)
            _Owner = Owner
            _Address = IPAddress.Parse(Address)
            _Port = Port
        End Sub
    End Class
    Public Sub AddClient(ByVal Client As Clients)
        Try
            SyncLock _SyncLockObject
                _Clients.Add(Client)
            End SyncLock
        Catch ex As Exception
            MessageBox.Show("Unexpected error: " & ex.Message & " while adding a new client", "Unexpected error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnClose_Click(sender As Object, e As System.EventArgs) Handles btnClose.Click
        Close()
    End Sub

    Private Sub btnStart_Click(sender As Object, e As System.EventArgs) Handles btnStart.Click
        If btnStart.Text = "Start" Then
            _Line = 0
            _Messages = File.ReadAllLines(txtCarMessageFileName.Text, System.Text.Encoding.GetEncoding("iso-8859-1"))
            _Server = New Server(Me, My.Settings.ServerAddress, My.Settings.ServerPort)
            _ServerThread = New System.Threading.Thread(AddressOf _Server.Listen)
            _ServerThread.Start()
            Timer1.Interval = My.Settings.TimerSpeed
            Timer1.Start()
            btnStart.Text = "Stop"
        Else
            _ServerThread.Abort()
            _ServerThread = Nothing
            Timer1.Stop()
            btnStart.Text = "Start"
        End If
    End Sub

    Private Sub CarMain_FormClosed(sender As Object, e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        If Not _ServerThread Is Nothing Then
            _ServerThread.Abort()
            _ServerThread = Nothing
        End If
    End Sub

    Private Sub CarMain_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        txtCarMessageFileName.Text = My.Settings.CarMessageFile
        startingTimeStamp.Value = Date.Now
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As System.EventArgs) Handles Timer1.Tick
        SyncLock _SyncLockObject
            Dim stream As NetworkStream = Nothing

            If _Clients.Count > 0 And _Line < _Messages.Length Then
                Dim data As [Byte]() = System.Text.Encoding.ASCII.GetBytes("NEW" & vbTab & _Messages(_Line) & "," & "TS01=" & DateAdd(DateInterval.Second, _Line, startingTimeStamp.Value).ToString("MM/dd/yyyy HH:mm:ss") & vbLf)
                _Line += 1
                For Each Client As Clients In _Clients
                    If Client.Client.Connected And Client.EnableSending Then
                        Try
                            stream = Client.Client.GetStream
                            stream.Write(data, 0, data.Length)
                        Catch ex As Exception
                        End Try
                    Else

                    End If
                Next
            End If

        End SyncLock
    End Sub
End Class
