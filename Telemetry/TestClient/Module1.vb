Imports System.Net
Imports System.Net.Sockets
Module Module1

    Sub Main()
        Dim client As TcpClient = New TcpClient()
        Try
            client.Connect("169.254.167.110", 5000)
            Dim stream As NetworkStream = client.GetStream()

            While True
                Dim responseData As String = String.Empty
                Dim data(255) As [Byte]
                Dim output As String = ""

                Dim bytes As Int32 = stream.Read(data, 0, data.Length)
                responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes)
                output = "Received: " + responseData
                Console.WriteLine(output)
            End While
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            client.Close()
        End Try

    End Sub

End Module
