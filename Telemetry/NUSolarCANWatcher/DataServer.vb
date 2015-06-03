Imports System.Collections.Concurrent
Imports System.IO
Imports System.Net
Imports System.Net.Sockets
Imports System.Text

Public Class DataServer
    Private _DataStack As ConcurrentStack(Of String)
    Private _ErrorWriter As LogWriter
    Private _State As ServerState
    Private _NextState As ServerState

    Private _IPPostPath As String
    Private _AccessToken As String

    Private _Listener As TcpListener
    Private _Port As Int32
    Private _IPAddress As IPAddress
    Private _Client As TcpClient
    Private _Stream As NetworkStream
    Private _ListenerStarted As Boolean
    Private _Connected As Boolean

    Private _DataString As String
    Private _DataBytes As Byte()


    Public Enum ServerState
        CreateListener
        PostIP
        Listen
        Connected
        DestroyListener
    End Enum

    Public Sub New(ByVal IPPostPath As String, ByVal AccessToken As String, ByVal Port As Int32)
        _DataStack = New ConcurrentStack(Of String)
        _ErrorWriter = New LogWriter("server_log.txt")
        _State = ServerState.CreateListener
        _NextState = Nothing

        _IPPostPath = IPPostPath
        _AccessToken = AccessToken

        _Listener = Nothing
        _Port = Port
        _IPAddress = GetIP()
        _Client = Nothing
        _ListenerStarted = False
        _Connected = False

        _DataString = ""
        _DataBytes = Nothing
    End Sub

    Public Sub Run()
        Select Case _State
            Case ServerState.CreateListener
                CreateListener()
            Case ServerState.PostIP
                PostIP()
            Case ServerState.Listen
                Listen()
            Case ServerState.Connected
                Connected()
            Case ServerState.DestroyListener
                DestroyListener()
        End Select
        UpdateState()
        LoadData()
    End Sub

    Private Sub CreateListener()
        _Listener = New TcpListener(_IPAddress, _Port)
        Try
            _Listener.Start(1)
            _ListenerStarted = True
            _NextState = ServerState.PostIP
        Catch ex As Exception
            _ErrorWriter.Write("Unable to create TCP listener: " & ex.Message)
            StopListener()
            _NextState = ServerState.CreateListener
        End Try
    End Sub

    Private Sub PostIP()
        Try
            Shell("java -jar " & _IPPostPath & " " & _AccessToken & " " & _IPAddress.ToString)
            _NextState = ServerState.Listen
        Catch ex As Exception
            _ErrorWriter.Write("Failed to post IP address: " & ex.Message)
            _NextState = ServerState.PostIP
        End Try
    End Sub

    Private Sub Listen()
        If _Listener.Pending() Then
            Try
                _Client = _Listener.AcceptTcpClient()
                _Stream = _Client.GetStream()
                _Connected = True
                _NextState = ServerState.Connected
            Catch ex As Exception
                _ErrorWriter.Write("Failed to connect to client: " & ex.Message)
                StopListener()
                _NextState = ServerState.CreateListener
            End Try
        Else
            _NextState = ServerState.Listen
        End If
    End Sub

    Private Sub Connected()
        Try
            Send()
            Receive()
            _NextState = ServerState.Connected
        Catch ex As Exception
            _ErrorWriter.Write("Error while writing to client: " & ex.Message)
            CloseConnection()
            _NextState = ServerState.CreateListener
        End Try
    End Sub

    Private Sub DestroyListener()
        Try
            If _Connected Then
                CloseConnection()
            End If
            If _ListenerStarted Then
                StopListener()
            End If
            _NextState = ServerState.CreateListener
        Catch ex As Exception
            _ErrorWriter.Write("Error while destroying listener: " & ex.Message)
            _ListenerStarted = False
            _Connected = False
            _NextState = ServerState.CreateListener
        End Try
    End Sub

    Private Sub UpdateState()
        Dim newIP As IPAddress = GetIP()
        If Not newIP.Equals(_IPAddress) Then
            _IPAddress = newIP
            _State = ServerState.DestroyListener
        Else
            _State = _NextState
        End If
    End Sub

    Private Sub LoadData()
        _DataStack.Push(Format(Now, "G"))
    End Sub

    Private Sub Send()
        If Not _DataStack.IsEmpty Then
            _DataStack.TryPeek(_DataString)
            _DataBytes = Encoding.ASCII.GetBytes(_DataString)
            _Stream.Write(_DataBytes, 0, _DataBytes.Length)
            _DataStack.TryPop(_DataString)                          ' will only get here if send successful
        End If
    End Sub

    Private Sub Receive()
        ' Empty for now
    End Sub

    Private Sub StopListener()
        _Listener.Stop()
        _ListenerStarted = False
    End Sub

    Private Sub CloseConnection()
        _Client.Close()
        _Listener.Stop()
        _ListenerStarted = False
        _Connected = False
    End Sub

    Private Function GetIP() As IPAddress
        Return Dns.GetHostEntry(Dns.GetHostName()).AddressList(0)
    End Function
End Class
