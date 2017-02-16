using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Server
{
    public class AcceptedClient
    {
        public Socket Socket { get; }

        private readonly Thread _thread;
        private bool _stop;

        public event EventHandler ClientDisconnected;

        public AcceptedClient(Socket socket)
        {
            Socket = socket;
            _thread = new Thread(Run);
        }

        public void Start()
        {
            _thread.Start();
        }

        public void Stop()
        {
            _stop = true;
            _thread.Join();
        }

        private void Run()
        {
            var buffer = new byte[1024];

            while (!_stop)
            {
                try
                {
                    var readCount = Socket.Receive(buffer);

                    if (readCount == 0)
                        break;

                    var text = Encoding.UTF8.GetString(buffer, 0, readCount);

                    Console.WriteLine($"Client sent: {text}");

                    Socket.Send(buffer, 0, readCount, SocketFlags.None);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    break;
                }

                Thread.Sleep(1);
            }

            ClientDisconnected?.Invoke(this, EventArgs.Empty);
        }
    }
}