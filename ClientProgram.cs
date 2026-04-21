using System.Net;
using System.Net.Sockets;

namespace client2;

public class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");

        var client = new TcpClient();
        var ip = IPAddress.Parse("192.168.1.72");
        var port = 27001;
        var ep = new IPEndPoint(ip, port);


        try {
        client.Connect(ep);
            if (client.Connected)
            {
                Console.WriteLine("Connected to server successfully!");
                var writer = Task.Run(() =>
                {
                    while (true)
                    {
                        var msg = Console.ReadLine();
                        var stream = client.GetStream();
                        var bw = new BinaryWriter(stream);
                        bw.Write(msg);

                    }
                });


                var reader = Task.Run(() =>
                {
                    while (true)
                    {
                        var stream = client.GetStream();
                        var br = new BinaryReader(stream);
                        var msg = br.ReadString();
                        Console.WriteLine($"Received: {msg}");
                    }
                });

                Task.WaitAll(writer, reader);
            }
        
        }
        catch (Exception e)
        {
            Console.WriteLine($"An error occurred: {e.Message}");
            Console.WriteLine(e);
        }

    }
}
