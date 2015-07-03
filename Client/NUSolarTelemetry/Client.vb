Imports System.Net
Imports System.Net.Sockets
Imports System.Text
Imports System.Threading
Public Class Client
    Private _Address As IPAddress
    Private _Port As Integer
    Private _Owner As Main
    Private _client As TcpClient = Nothing
    Private _stream As NetworkStream = Nothing
    Private _buffer(10000) As Byte
    Private _Timer As Timer
    Private Sub ConnectionTimeout()
        Try
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
            _Timer.Dispose()
            _Timer = New Timer(New TimerCallback(AddressOf ConnectionTimeout), Nothing, My.Settings.ReadTimeout, Threading.Timeout.Infinite)
        End If
    End Sub
    Private Sub ReadComplete(ByVal result As IAsyncResult)
        Try
            Dim bytesread As Integer = _stream.EndRead(result)
            Dim buffer As String = ""
            _Timer.Dispose()
            buffer = [String].Concat(buffer, Encoding.ASCII.GetString(_buffer, 0, bytesread))
            _Owner.ProcessUpdate(buffer)
            InitiateRead()
        Catch ex As Exception

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
                _Timer = New Timer(New TimerCallback(AddressOf Connect), Nothing, My.Settings.ReadTimeout, Threading.Timeout.Infinite)
            End If
        Catch ex As Exception

        End Try
    End Sub
    Public Sub Connect()
        _client = New TcpClient
        _client.BeginConnect(_Address, _Port, AddressOf ConnectComplete, Nothing)
        If Not _Timer Is Nothing Then
            _Timer.Dispose()
        End If
        _Timer = New Timer(New TimerCallback(AddressOf ConnectionTimeout), Nothing, My.Settings.ReadTimeout, Threading.Timeout.Infinite)
    End Sub
    Public Sub New(ByVal Owner As Main, ByVal Address As String, ByVal Port As Integer)
        _Owner = Owner
        _Address = IPAddress.Parse(Address)
        _Port = Port
    End Sub
End Class
