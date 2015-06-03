Imports System.Collections.Concurrent
Imports System.IO
Imports System.Net
Imports System.Net.Sockets
Imports System.Text

Public Class DataServer
    Private _DataStack As ConcurrentStack(Of String)
    Private _ErrorWriter As LogWriter
    Private _DebugWriter As LogWriter
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
        _ErrorWriter = New LogWriter("server_error_log.txt")
        _ErrorWriter.ClearLog()
        _DebugWriter = New LogWriter("server_debug_log.txt", True)
        _DebugWriter.ClearLog()
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
        _DebugWriter.AddMessage("*** Data server run with state " & _State)
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
        _DebugWriter.WriteAll()
    End Sub

    Private Sub CreateListener()
        _Listener = New TcpListener(_IPAddress, _Port)
        Try
            _Listener.Start(1)
            _ListenerStarted = True
            _NextState = ServerState.PostIP
            _DebugWriter.AddMessage("Listener started at IP address " & _IPAddress.ToString() & " port " & _Port)
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
            _DebugWriter.AddMessage("IP address " & _IPAddress.ToString & " posted to dropbox")
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
                _DebugWriter.AddMessage("Connected to client")
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
            _DebugWriter.AddMessage("Destroyed current listener")
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
            _DebugWriter.AddMessage("IP of server changed")
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
            _DebugWriter.AddMessage("Sent " & _DataString & " to client")
        End If
    End Sub

    Private Sub Receive()
        ' Empty for now
    End Sub

    Private Sub StopListener()
        _Listener.Stop()
        _ListenerStarted = False
        _DebugWriter.AddMessage("Stopped listener")
    End Sub

    Private Sub CloseConnection()
        _Client.Close()
        _Listener.Stop()
        _ListenerStarted = False
        _Connected = False
        _DebugWriter.AddMessage("Closed connection")
    End Sub

    Private Function GetIP() As IPAddress
        Dim addresses As IPAddress() = Dns.GetHostEntry("localhost").AddressList
        Dim address As IPAddress = Dns.GetHostEntry("localhost").AddressList(1)
        Return address
    End Function
End Class
