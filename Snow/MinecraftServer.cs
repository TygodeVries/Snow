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
using Snow.Tests;
namespace Snow
{
    public class MinecraftServer
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

        EntityMotionTest entityMotionTest;

        public void Start()
        {
            Console.WriteLine("Starting EntityMotionTest");
            entityMotionTest = new EntityMotionTest(world);
            tcpListener.Start();

            Console.WriteLine("Server is running!");
            while(isRunning)
            {
                if (tickCount % 20 == 0)
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
                else
                {
                    Tick();
                }
            }
        }

        public List<PlayerConnection> playerConnections = new List<PlayerConnection>();

        public int tickCount = 0;
        public void Tick()
        {
            tickCount++;
            AcceptNewClients();

            if(tickCount % 20 == 0)
            {
                SendKeepAlive();
            }

            entityMotionTest.Tick(tickCount);

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

                Console.WriteLine("A new player has joined!");

                playerConnections.Add(player);

                EntityPlayer entityPlayer = new EntityPlayer(player);
                world.SpawnEntity(entityPlayer); // Spawn entity into world

                player.SendConnectionPackets(entityPlayer);

                entityPlayer.GetInventory().content[36] = new ItemStack();

                entityPlayer.SpawnClient();
            }
        }
    }
}
