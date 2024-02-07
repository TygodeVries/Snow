using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Threading;
using Snow.Level;
using Snow.Entities;
using Snow.Network;
using Snow.Level.Entities;
using Snow.Network.Packets.Play.Clientbound;
using Snow.Containers;
using Snow.Admin;
using Snow.Tests;
namespace Snow
{
    public class MinecraftServer
    {
        TcpListener tcpListener;
        bool isRunning;

        public bool IsRunning()
        {
            return isRunning;
        }

        public MinecraftServer(int port)
        {
            tcpListener = new TcpListener(IPAddress.Any, port);
            playerConnections = new List<PlayerConnection>();
            isRunning = true;
        }

        public List<PlayerConnection> playerConnections;

        double totalMSTP = 0;
        public void Start()
        {
            tcpListener.Start();

            Log.Send("Server is running!");
            while (isRunning)
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
            isRunning = false;
        }

        World world = new World();

        public World GetWorld()
        {
            return world;
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

            foreach(PlayerConnection connection in playerConnections)
            {
                connection.ReadPackets();
            }

            world.Clean();
        }

        void SendKeepAlive()
        {
            foreach(PlayerConnection player in playerConnections)
            {
                player.SendPacket(new UpdateTime());
            }
        }

        void AcceptNewClients()
        {
            if(tcpListener.Pending())
            {
                TcpClient tcpClient = tcpListener.AcceptTcpClient();
                PlayerConnection player = new PlayerConnection(tcpClient, this);

                playerConnections.Add(player);
            }
        }
    }
}
