using Snow.Network;
using Snow.Network.Packets.Play.Clientbound;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Servers
{
    internal class ConnectionListener
    {
        private TcpListener _tcpListener;
        private int _port;
        private Server _server;

        private List<Connection> _playerConnections;
        internal List<Connection> GetConnections()
        {
            return _playerConnections;
        }

        public void DisconnectInactiveClients()
        {
            for(int i = 0; i < _playerConnections.Count; i++)
            {
                Connection con = _playerConnections[i];
                if(!con.IsConnected())
                {
                    _playerConnections.RemoveAt(i);
                    i--;
                }
            }
        }

        public ConnectionListener(int port, Server server)
        {
            _playerConnections = new List<Connection>();
            _port = port;
            _server = server;
        }

        public void Start()
        {
            _tcpListener = new TcpListener(IPAddress.Any, _port);
            _tcpListener.Start();
        }

        internal void Tick()
        {
            AcceptNewClients();
            ReadPackets();
            SendKeepAlive();
        }

        private void AcceptNewClients()
        {
            if (_tcpListener.Pending())
            {
                TcpClient tcpClient = _tcpListener.AcceptTcpClient();
                Connection player = new Connection(_server, tcpClient);
                _playerConnections.Add(player);
            }
        }

        private void ReadPackets()
        {
            foreach (Connection connection in _playerConnections)
            {
                connection.ReadPackets();
            }
        }

        private void SendKeepAlive()
        {
            if (_server.GetTick() % 40 == 0)
            {
                foreach (Connection player in _playerConnections)
                {
                    player.SendPacket(new KeepAlivePacket(10));
                }
            }
        }
    }
}
