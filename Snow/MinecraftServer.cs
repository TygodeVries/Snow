using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using Snow.Level;
using Snow.Network;
using Snow.Network.Packets.Play.Clientbound;
using Snow.Admin;
namespace Snow
{
    public class MinecraftServer
    {


        private LevelSpace levelSpace;
        public LevelSpace GetLevelSpace()
        {
            return levelSpace;
        }

        TcpListener tcpListener;

        public MinecraftServer(int port)
        {
            tcpListener = new TcpListener(IPAddress.Any, port);
            playerConnections = new List<Connection>();
        }

        public List<Connection> playerConnections;


        double totalMSTP = 0;
        public void Start()
        {
            tcpListener.Start();

            Log.Send("Server is running!");
            while (true)
            {
                DateTime startTime = DateTime.Now;
                Tick();


                // Calculate time it took to run tick
                double mspt = DateTime.Now.Subtract(startTime).TotalMilliseconds;

                // Wait till end of tick
                if ((int)(50 - mspt) > 0)
                    Thread.Sleep((int)(50 - mspt));

                totalMSTP += mspt;

                if (tickCount % 20 == 0)
                {
                    double avarageMSPT = totalMSTP / 20;
                    totalMSTP = 0;

                    Console.Title = $"MSPT (20 ticks): {avarageMSPT}";
                    if (avarageMSPT > 50)
                    {
                        Log.Send($"Failed to complete tick in time, running {avarageMSPT - 50}ms behind!");
                    }
                }
            }
        }

        public void Stop()
        {
           
        }

        long tickCount = 0;
        public long GetTick()
        {
            return tickCount;
        }
        
        public void Tick()
        {
            tickCount++;
            AcceptNewClients();

            if(tickCount % 20 == 0)
            {
                SendKeepAlive();
            }

            foreach(Connection connection in playerConnections)
            {
                connection.ReadPackets();
            }

        }

        void SendKeepAlive()
        {
            foreach(Connection player in playerConnections)
            {
                player.SendPacket(new UpdateTime());
            }
        }

        void AcceptNewClients()
        {
            if(tcpListener.Pending())
            {
                TcpClient tcpClient = tcpListener.AcceptTcpClient();
                Connection player = new Connection(tcpClient, this);

                playerConnections.Add(player);
            }
        }
    }
}
