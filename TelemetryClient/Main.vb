﻿Imports System.Net.Sockets

Module Main
    Sub Main()
        Dim addr As String = "169.254.199.17"
        Dim port As Int32 = 2000
        Dim client As TcpClient = New TcpClient(addr, port)
        Console.WriteLine("Connected.")

        Dim message As String = "hello from client"
        Dim data As Byte() = System.Text.Encoding.ASCII.GetBytes(message)
        Dim stream As NetworkStream = client.GetStream()
        stream.Write(data, 0, data.Length)
        Console.WriteLine("Sent {0}", message)

        Dim bytes(1024) As Byte
        stream.Read(bytes, 0, bytes.Length)
        Dim response As String = System.Text.Encoding.ASCII.GetString(bytes)
        Console.WriteLine("Received {0}", response)

        stream.Close()
        client.Close()
        Console.WriteLine("Closed.")

        Console.Write("Press enter to continue...")
        Console.Read()
    End Sub
End Module
