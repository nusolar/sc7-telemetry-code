Imports System.Collections.Concurrent
Imports System.Collections.Generic
Imports System.IO
Imports System.Net
Imports System.Net.Sockets
Imports System.Text
Imports System.Data.SqlClient

Public Class DataServer
    Private _DataStack As ConcurrentStack(Of DataRow)
    Private _SQLConn As SqlConnection

    Private _ErrorWriter As LogWriter
    Private _DebugWriter As LogWriter
    Private _State As ServerState
    Private _NextState As ServerState

    Private _IPPostPath As String
    Private _AccessToken As String
    Private _Users As List(Of String)

    Private _Listener As TcpListener
    Private _Port As Int32
    Private _IPAddress As IPAddress
    Private _Client As TcpClient
    Private _Stream As NetworkStream
    Private _DataBuffer As Byte()

    Private _Listening As Boolean
    Private _ClientConnected As Boolean
    Private _Run As Boolean

    Public Enum ServerState
        CreateListener
        PostIP
        Listen
        Connected
        DestroyListener
    End Enum

    Private Class DataRow
        Private _RowNum As Int32
        Private _NumCols As Int32
        Private _DataString As String

        Public Sub New(ByVal dr As SqlDataReader)
            _RowNum = dr("RowNum")
            _NumCols = dr.FieldCount
            _DataString = ""
            For i As Integer = 0 To _NumCols - 1
                _DataString &= dr.GetName(i) & "=" & dr.GetValue(i) & ","
            Next
            _DataString = _DataString.Substring(0, _DataString.Length - 1)
        End Sub

        Public ReadOnly Property RowNum As Int32
            Get
                Return _RowNum
            End Get
        End Property

        Public ReadOnly Property NumCols As Int32
            Get
                Return _NumCols
            End Get
        End Property

        Public ReadOnly Property DataString As String
            Get
                Return _DataString
            End Get
        End Property
    End Class

    Public Sub New()
        _DataStack = New ConcurrentStack(Of DataRow)
        _SQLConn = New SqlConnection(My.Settings.DSN)

        _ErrorWriter = New LogWriter("server_error_log.txt")
        _ErrorWriter.ClearLog()
        _DebugWriter = New LogWriter("server_debug_log.txt", True)
        _DebugWriter.ClearLog()
        _State = ServerState.CreateListener
        _NextState = Nothing

        _IPPostPath = My.Settings.IPPostPath
        _AccessToken = My.Settings.DropboxAccessToken
        _Users = New List(Of String)
        _Users.Add("nalindquist")

        _Listener = Nothing
        _Port = My.Settings.ServerPort
        _IPAddress = GetIP()
        _Client = Nothing
        _DataBuffer = New Byte(255) {}

        _Listening = False
        _ClientConnected = False
        _Run = False
    End Sub

#Region "Public Methods"
    Public Sub Init()
        Try
            _SQLConn.Open()
            LoadDataRows(1, True)
        Catch ex As Exception
            _ErrorWriter.Write("Error opening SQL conection: " & ex.Message)
        End Try

        _Run = True
    End Sub

    Public Sub Run()
        While _Run
            _DebugWriter.AddMessage("*** Data server run in state " & StateToString(_State))
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
            _DebugWriter.WriteAll()
        End While
    End Sub

    Public Sub LoadLastRow()
        ' build query string
        Dim query As String = "SELECT TOP 1 * FROM tblHistory ORDER BY " & _
                              "RowNum DESC"
        _DebugWriter.AddMessage("Attempting to execute query: " & query)

        ' execute query
        Try
            Dim cmd As New SqlCommand()
            cmd.CommandText = query
            cmd.CommandType = CommandType.Text
            cmd.CommandTimeout = 0
            cmd.Connection = _SQLConn
            Dim dr = cmd.ExecuteReader()

            Do While dr.Read()
                _DataStack.Push(New DataRow(dr))
            Loop
            dr.Close()

            _DebugWriter.AddMessage("Loaded last row from database")
        Catch ex As Exception
            _ErrorWriter.Write("Error loading last row from database: " & ex.Message)
        End Try
    End Sub

    Public Sub Close()
        _Run = False

        If _ClientConnected Then
            CloseConnection()
        End If
        If _Listening Then
            StopListener()
        End If

        Try
            _SQLConn.Close()
        Catch ex As Exception
            _ErrorWriter.Write("Error closing SQL connection: " & ex.Message)
        End Try
    End Sub
#End Region

#Region "State Functions"
    Private Sub CreateListener()
        If _IPAddress IsNot Nothing Then
            _Listener = New TcpListener(_IPAddress, _Port)
            Try
                _Listener.Start()
                _Listening = True
                _NextState = ServerState.PostIP
                _DebugWriter.AddMessage("Listener started at " & _IPAddress.ToString() & ":" & _Port)
            Catch ex As Exception
                StopListener()
                _ErrorWriter.Write("Unable to create TCP listener: " & ex.Message)
                _NextState = ServerState.CreateListener
            End Try
        Else
            _ErrorWriter.Write("No IP address available to listen on")
            _NextState = ServerState.CreateListener
        End If
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
                ' open connection to client
                _Client = _Listener.AcceptTcpClient()
                _Stream = _Client.GetStream()
                _ClientConnected = True
                _DebugWriter.AddMessage("Connected to client at " & _Client.Client.RemoteEndPoint.ToString)

                ' authenticate connection
                If Not My.Settings.AuthenticationOn OrElse Authenticate() Then
                    _NextState = ServerState.Connected
                    _DebugWriter.AddMessage("Authenticated client")
                Else
                    CloseConnection()
                    _NextState = ServerState.Listen
                    _DebugWriter.AddMessage("Failed to authenticate client")
                End If
            Catch ex As Exception
                StopListener()
                _ErrorWriter.Write("Failed to connect to client: " & ex.Message)
                _NextState = ServerState.CreateListener
            End Try
        Else
            Threading.Thread.Sleep(10)
            _NextState = ServerState.Listen
            _DebugWriter.AddMessage("No client to connect to")
            Debug.WriteLine("Listening...")
        End If
    End Sub

    Private Sub Connected()
        Try
            _DebugWriter.AddMessage("Attempting to send data row from stack")
            SendDataRowFromStack()

            If _ClientConnected Then
                _NextState = ServerState.Connected
            Else
                _NextState = ServerState.Listen
            End If
        Catch ex As Exception
            CloseConnection()
            _ErrorWriter.Write("Error while connected to client: " & ex.Message)
            _NextState = ServerState.Listen
        End Try
    End Sub

    Private Sub DestroyListener()
        Try
            CloseConnection()
            StopListener()
            _NextState = ServerState.CreateListener
            _DebugWriter.AddMessage("Destroyed current listener")
        Catch ex As Exception
            _ErrorWriter.Write("Error while destroying listener: " & ex.Message)
            _Listening = False
            _ClientConnected = False
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
#End Region

#Region "Private Methods"
    Private Function Authenticate() As Boolean
        Dim message As String = ""
        _DebugWriter.AddMessage("Beginning authentication")

        ' first exchange: receive "HELLO", send "NUSOLAR SC6"
        message = Receive(5000)
        If Not message.Equals("HELLO") Then
            _DebugWriter.AddMessage("Expected HELLO, authentication failure")
            Return False
        End If
        If Not Send("NUSOLAR SC6") Then
            Return False
        End If
        _DebugWriter.AddMessage("First stage of authentication successful")

        ' second exchange: recieve "USER: <username>", send "OK" or "UNAUTHORIZED"
        message = Receive(5000)
        If Not (message.Length > 6 AndAlso message.Substring(0, 6).Equals("USER: ") _
                AndAlso _Users.Contains(message.Substring(6))) Then
            _DebugWriter.AddMessage("Invalid user, authentication failure")
            Send("UNAUTHORIZED")
            Return False
        End If
        If Not Send("OK") Then
            Return False
        End If
        _DebugWriter.AddMessage("Second stage of authentication successful")

        ' third exchange: receive "VERSION 1.0", send "OK" or "INVALID VERSION"
        message = Receive(5000)
        If Not message.Equals("VERSION 1.0") Then
            _DebugWriter.AddMessage("Expected VERSION 1.0, authentication failure")
            Send("UNATHORIZED")
            Return False
        End If
        If Not Send("OK") Then
            Return False
        End If
        _DebugWriter.AddMessage("Third stage of authentication successful")

        ' success
        Return True
    End Function

    Private Function Send(ByVal message As String) As Boolean
        Try
            Dim dataBytes As Byte() = Encoding.ASCII.GetBytes(message)
            _Stream.Write(dataBytes, 0, dataBytes.Length)
            _DebugWriter.AddMessage("Sent '" & message & "' to client")
            Debug.WriteLine("Sent " & message & " to client")
            Return True
        Catch ex As Exception
            CloseConnection()
            _ErrorWriter.Write("Failed to send '" & message & "' to client: " & ex.Message)
            Return False
        End Try
    End Function

    Private Function Receive(ByVal timeout As Int64) As String
        Try
            ' wait for data to arrive
            Dim startTime As Int32 = My.Computer.Clock.TickCount
            Dim currentTime As Int32 = startTime
            While Not _Stream.DataAvailable
                currentTime = My.Computer.Clock.TickCount
                If currentTime - startTime >= timeout Then
                    _DebugWriter.AddMessage("Timed out while waiting to receive message")
                    Return ""   ' timeout
                End If
                System.Threading.Thread.Sleep(1)
            End While

            ' data available
            Dim bytes As Int32 = _Stream.Read(_DataBuffer, 0, _DataBuffer.Length)
            Dim message As String = System.Text.Encoding.ASCII.GetString(_DataBuffer, 0, bytes)
            If message.Length >= 2 AndAlso message.Substring(message.Length - 2, 2).Equals(vbCrLf) Then ' trim crlf
                message = message.Substring(0, message.Length - 2)
            End If
            _DebugWriter.AddMessage("Received '" & message & "' from client")
            Return message
        Catch ex As Exception
            CloseConnection()
            _ErrorWriter.Write("Failed to receive message from client: " & ex.Message)
            Return ""
        End Try
    End Function

    Private Sub SendDataRowFromStack()
        If Not _DataStack.IsEmpty Then
            Dim row As DataRow = Nothing
            _DataStack.TryPeek(row)
            Dim message = "DATA," & row.NumCols & "," & row.DataString
            _DebugWriter.AddMessage("Attempting to send row " & row.RowNum)

            If SendDataRow(message) Then
                _DataStack.TryPop(row)
                MarkRowSent(row.RowNum)
                _DebugWriter.AddMessage("Data row sent successfully")
            End If
        Else
            _DebugWriter.AddMessage("No data rows available to send")
        End If
    End Sub

    Private Function SendDataRow(ByVal row As String) As Boolean
        ' send row
        If Not Send(row) Then
            _DebugWriter.AddMessage("Failed to send row")
            Return False
        End If

        ' wait for ack
        Dim message As String = Receive(1000)
        If Not message.Equals("OK") Then
            _DebugWriter.AddMessage("Did not receive acknowledgement")
            Return False
        End If

        ' success
        Return True
    End Function

    Private Sub LoadDataRows(ByVal startRow As Int32, ByVal unsentOnly As Boolean)
        ' build query string
        Dim query = "SELECT * FROM tblHistory " & _
                    "WHERE RowNum>=" & startRow
        If unsentOnly Then
            query &= "AND Sent=0 "
        End If
        query &= "ORDER BY RowNum"
        _DebugWriter.AddMessage("Attempting to execute query: " & query)

        ' execute query
        Try
            Dim cmd As New SqlCommand()
            cmd.CommandText = query
            cmd.CommandType = CommandType.Text
            cmd.CommandTimeout = 0
            cmd.Connection = _SQLConn
            Dim dr = cmd.ExecuteReader()

            Do While dr.Read()
                _DataStack.Push(New DataRow(dr))
            Loop
            dr.Close()

            _DebugWriter.AddMessage("Loaded rows from database starting with row " & startRow)
        Catch ex As Exception
            _ErrorWriter.Write("Error loading rows from database: " & ex.Message)
        End Try
    End Sub

    Private Sub MarkRowSent(ByVal rowNum As Integer)
        ' build query string
        Dim query As String = "UPDATE tblHistory SET Sent=1 WHERE RowNum=" & rowNum
        _DebugWriter.AddMessage("Attempting to execute query: " & query)

        ' execute query
        Try
            Dim cmd As New SqlCommand()
            cmd.CommandText = query
            cmd.CommandType = CommandType.Text
            cmd.CommandTimeout = 0
            cmd.Connection = _SQLConn
            cmd.ExecuteNonQuery()

            _DebugWriter.AddMessage("Marked row " & rowNum & " as sent")
        Catch ex As Exception
            _ErrorWriter.Write("Error marking row as sent: " & ex.Message)
        End Try
    End Sub

    Private Sub StopListener()
        If _Listening Then
            _Listener.Stop()
            _Listening = False
            _DebugWriter.AddMessage("Stopped listener")
        End If
    End Sub

    Private Sub CloseConnection()
        If _ClientConnected Then
            _Client.Close()
            _ClientConnected = False
            _DebugWriter.AddMessage("Closed connection")
        End If
    End Sub

    Private Function GetIP() As IPAddress
        Dim addresses As IPAddress() = Nothing
        If My.Settings.CANBusConnected Then
            addresses = Dns.GetHostEntry(Dns.GetHostName()).AddressList
        Else
            addresses = Dns.GetHostEntry("localhost").AddressList
        End If
        Return SelectIP(addresses)
    End Function

    Private Function SelectIP(ByVal IPAddresses As IPAddress()) As IPAddress
        For Each address As IPAddress In IPAddresses
            If address.AddressFamily = AddressFamily.InterNetwork Then
                Return address
            End If
        Next
        Return Nothing  ' search failed
    End Function

    Private Function StateToString(ByVal State As ServerState) As String
        Select Case State
            Case ServerState.CreateListener
                Return "CREATE_LISTENER"
            Case ServerState.PostIP
                Return "POST_IP"
            Case ServerState.Listen
                Return "LISTEN"
            Case ServerState.Connected
                Return "CONNECTED"
            Case ServerState.DestroyListener
                Return "DESTROY_LISTENER"
        End Select
        Return "UNKNOWN_STATE"
    End Function
#End Region

End Class
