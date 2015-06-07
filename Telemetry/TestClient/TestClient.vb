Imports System.Net
Imports System.Net.Sockets
Module TestClient
    Dim data(2000) As Byte

    Sub Main()
        Dim writer As New LogWriter("client_log.txt")
        writer.ClearLog()

        Dim client As TcpClient = New TcpClient()
        Try
            ' connection
            client.Connect("127.0.0.1", 5000)
            Dim stream As NetworkStream = client.GetStream()

            ' authentication
            Write(stream, "HELLO")
            writer.Write(Read(stream))
            Write(stream, "USER: nalindquist")
            writer.Write(Read(stream))
            Write(stream, "VERSION 1.0")
            writer.Write(Read(stream))

            ' read data from server
            While True
                Dim message As String = Read(stream)
                writer.Write(message)
                Console.WriteLine(message)
                Write(stream, "OK")
            End While
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            client.Close()
        End Try
    End Sub

    Private Function Read(ByVal stream As NetworkStream) As String
        Dim responseData As String = String.Empty

        While Not stream.DataAvailable
            Threading.Thread.Sleep(10)
        End While

        Dim bytes As Int32 = stream.Read(data, 0, data.Length)
        responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes)
        Return responseData
    End Function

    Private Sub Write(ByVal stream As NetworkStream, ByVal message As String)
        Dim dataBytes As Byte() = Text.Encoding.ASCII.GetBytes(message)
        stream.Write(dataBytes, 0, dataBytes.Length)
    End Sub
End Module
