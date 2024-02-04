using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Threading;
using Snow.Level;
using Snow.Network.Entity;
using Snow.Entities;
using Snow.Network;
namespace Snow
{
    internal class MinecraftServer
    {
        TcpListener tcpListener;

        public MinecraftServer(int port)
        {
            tcpListener = new TcpListener(IPAddress.Any, port);
        }

        bool isRunning = true;

        public void Stop()
        {
            isRunning = false;
        }

        World world = new World();

        public void Start()
        {
            tcpListener.Start();

            while(isRunning)
            {
                DateTime startTime = DateTime.Now;
                Tick();
                double mspt = DateTime.Now.Subtract(startTime).TotalMilliseconds;

                Console.Title = $"MSPT: {mspt}";

                if (50 - mspt > 0)
                {
                    Thread.Sleep((int)(50 - mspt));
                }
                else
                {
                    // Cant keep up
                    Console.Title = $"MSPT: {mspt} | NOT KEEPING UP!";
                }
            }
        }

        public List<PlayerConnection> playerConnections = new List<PlayerConnection>();

        public void Tick()
        {
            AcceptNewClients();
        }

        void AcceptNewClients()
        {
            if(tcpListener.Pending())
            {
                TcpClient tcpClient = tcpListener.AcceptTcpClient();
                PlayerConnection player = new PlayerConnection(tcpClient);

                playerConnections.Add(player);

                EntityPlayer playerEntity = new EntityPlayer(player);
                world.SpawnEntity(playerEntity);

                player.SendConnectionPackets();

                playerEntity.SpawnClient();
            }
        }
    }
}
