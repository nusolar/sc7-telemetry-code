Imports System.Net
Imports System.Net.Sockets

Module Main
    Sub Main()
        Dim addr As IPAddress = IPAddress.Any
        Dim port As Int32 = 2000
        Dim listener As New TcpListener(addr, port)

        listener.Start()
        Console.WriteLine("Start listening.")

        Dim client As TcpClient = listener.AcceptTcpClient()
        Console.WriteLine("Connected.")

        Dim bytes(1024) As Byte
        Dim data As String = Nothing
        Dim stream As NetworkStream = client.GetStream()
        Dim i As Int32 = stream.Read(bytes, 0, bytes.Length)
        data = System.Text.Encoding.ASCII.GetString(bytes, 0, i)
        Console.WriteLine("Received {0}", data)

        client.Close()
        listener.Stop()
        Console.WriteLine("Closed.")

        Console.Write("Press enter to continue...")
        Console.Read()
    End Sub
End Module
