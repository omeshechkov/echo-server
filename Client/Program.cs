using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Client
{
    public class Program
    {
        public static void Main(string[] args)
        {
            const string address = "127.0.0.1";
            const int port = 9000;

            var buffer = new byte[1024];

            using (var clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
            {
                clientSocket.Connect(new IPEndPoint(IPAddress.Parse(address), port));
                Console.WriteLine($"Connected to {address}:{port}");

                while (true)
                {
                    Console.Write("Enter text:");
                    var text = Console.ReadLine();

                    if (string.IsNullOrEmpty(text))
                        break;

                    var bytes = Encoding.UTF8.GetBytes(text);

                    clientSocket.Send(bytes);

                    var readBytes = clientSocket.Receive(buffer);

                    var receivedText = Encoding.UTF8.GetString(buffer, 0, readBytes);

                    Console.WriteLine($"Echo: {receivedText}");
                }
            }
        }
    }
}
