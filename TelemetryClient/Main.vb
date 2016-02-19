Imports System.Net.Sockets

Module Main
    Sub Main()
        Dim addr As String = "169.254.199.17"
        Dim port As Int32 = 2000
        Dim message As String = "Hello server!"
        Dim data As Byte() = System.Text.Encoding.ASCII.GetBytes(message)
        Dim client As TcpClient = New TcpClient(addr, port)
        Console.WriteLine("Connected.")

        Dim stream As NetworkStream = client.GetStream()
        stream.Write(data, 0, data.Length)
        Console.WriteLine("Sent {0}", message)

        stream.Close()
        client.Close()
        Console.WriteLine("Closed.")

        Console.Write("Press enter to continue...")
        Console.Read()
    End Sub
End Module
