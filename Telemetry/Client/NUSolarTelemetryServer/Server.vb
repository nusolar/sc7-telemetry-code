Imports System.Net
Imports System.Net.Sockets
Public Class Server
    Private _Address As IPAddress
    Private _Port As Integer
    Private _Owner As ServerMain
    Private _listener As TcpListener = Nothing
    Private _ClientConnected As New Threading.ManualResetEvent(False)

    Private Sub DoAcceptTcpClientCallback(ar As IAsyncResult)
        If Not ar Is Nothing Then
            Dim listener As TcpListener = CType(ar.AsyncState, TcpListener)
            Try
                Dim client As TcpClient = listener.EndAcceptTcpClient(ar)
                _Owner.AddClient(client)
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
    Public Sub New(ByVal Owner As ServerMain, ByVal Address As String, ByVal Port As Integer)
        _Owner = Owner
        _Address = IPAddress.Parse(Address)
        _Port = Port
    End Sub
End Class
