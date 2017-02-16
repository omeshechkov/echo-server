using System;
using System.Net;
using System.Net.Sockets;

namespace Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            const string listeningAddress = "127.0.0.1";
            const int port = 9000;

            using (var serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
            {
                serverSocket.Bind(new IPEndPoint(IPAddress.Parse(listeningAddress), port));
                serverSocket.Listen(1);

                Console.WriteLine($"Listening on {listeningAddress}:{port}");

                var server = new Server(serverSocket);
                server.Start();

                server.ClientConnected += OnClientConnected;

                Console.ReadLine();
            }
        }

        private static void OnClientConnected(object sender, ClientConnectedEventArgs args)
        {
            Console.WriteLine($"Client '{args.Socket.RemoteEndPoint}' connected");

            var acceptedClient = new AcceptedClient(args.Socket);
            acceptedClient.ClientDisconnected += OnClientDisconnected;

            acceptedClient.Start();
        }

        private static void OnClientDisconnected(object sender, EventArgs args)
        {
            var acceptedClient = (AcceptedClient)sender;
            Console.WriteLine($"Client '{acceptedClient.Socket.RemoteEndPoint}' disconnected");
        }
    }
}
