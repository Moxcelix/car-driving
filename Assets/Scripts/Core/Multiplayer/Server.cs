using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Core.Multiplayer
{
    public class Server
    {
        private readonly int _port;
        private readonly IPAddress _address;
        private readonly TcpListener _listener;

        private readonly Thread _listeningThread;

        public Server(int port, IPAddress address)
        {
            _port = port;
            _address = address;

            _listener = new TcpListener(_address, _port);

            _listeningThread = new Thread(Listening);
        }

        public void Start()
        {
            _listeningThread.Start();
        }

        public void Stop()
        {
            _listeningThread.Abort();
        }

        private void Listening()
        {
            while (_listeningThread.IsAlive)
            {

            }
        }
    }
}