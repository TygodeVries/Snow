using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using Snow.Network;
using Snow.Network.Packets.Play.Clientbound;
using Snow.Addons;
using Snow.Levels;
using Snow.Formats;
using Snow.Entities;
using Snow.Commands;
using Snow.Events;
using Snow.Events.Args;
using Snow.Items;
using Snow.Worlds;
using Snow.Worlds.Generator;

namespace Snow.Servers
{
    public class Server
    {
        TcpListener tcpListener;

        private CommandManager commandManager;
        public CommandManager GetCommandManager()
        {
            return commandManager;
        }

        private List<Connection> playerConnections;
        public List<Connection> GetPlayerConnections()
        {
            return playerConnections;
        }

        public Server(int port)
        {
            tcpListener = new TcpListener(IPAddress.Any, port);
            playerConnections = new List<Connection>();
            entities = new List<Entity>();
            eventManager = new EventManager();

            Thread thread = new Thread(this.LobbyThread);
            thread.Start();
        }

        public void SetBlockAt(int x, int y, int z, BlockType blockType)
        {
            SetBlockAt(x, y, z, blockType, null);
        }

        public void SetBlockAt(int x, int y, int z, BlockType blockType, Player player)
        {
            GetEventManager().ExecuteBlockPlace(new BlockPlaceEventArgs(player, new Position(x, y, z)));

        }

        public void Stop()
        {
            addonManager.StopAll();
        }
        private void LobbyThread()
        {
            tcpListener.Start();

            commandManager = new CommandManager();

            world = new World("Test World", this);
            world.SetWorldGenerator(new FlatGenerator());

            GetEventManager().RightClickBlock += BlockPlaceAttempt;

            Log.Send("Server is running!");
            while (true)
            {
                DateTime startTime = DateTime.Now;

                // Tick all things in this server
                TickLobby();

                // Wait until the next tick can be run.
                double mspt = DateTime.Now.Subtract(startTime).TotalMilliseconds;
                if ((int)(50 - mspt) > 0)
                    Thread.Sleep((int)(50 - mspt));
            }
        }

        private void BlockPlaceAttempt(object sender, RightClickBlockEventArgs e)
        {
            ItemStack itemStack = e.GetPlayer().GetItemMainInHand();
            if (itemStack == null)
                return;

            if (e.insideBlock)
                return;

            if(itemStack.GetItemType().IsBlock)
            {
                int face = e.GetClickedFace();

                Position blockPos = e.GetClickedBlock().GetAdjacent(face);
                GetEventManager().ExecuteBlockPlace(new BlockPlaceEventArgs(e.GetPlayer(), blockPos));
            }

        }

        private EventManager eventManager;
        public EventManager GetEventManager()
        {
            return eventManager;
        }

        long tickCount = 0;
        public long GetTick()
        {
            return tickCount;
        }
        
        internal void TickLobby()
        {
            tickCount++;

            AcceptNewClients();
            SendKeepAlive();
            ReadPackets();
        }
        internal void SendKeepAlive()
        {
            if (GetTick() % 40 == 0)
            {
                foreach (Connection player in playerConnections)
                {
                    player.SendPacket(new UpdateTimePacket());
                }
            }
        }
        internal void AcceptNewClients()
        {
            if (tcpListener.Pending())
            {
                TcpClient tcpClient = tcpListener.AcceptTcpClient();
                Connection player = new Connection(this, tcpClient);
                playerConnections.Add(player);
            }
        }

        internal void ReadPackets()
        {
            foreach (Connection connection in playerConnections)
            {
                connection.ReadPackets();
            }
        }

        private AddonManager addonManager;
        public AddonManager GetAddonManager()
        {
            return addonManager;
        }

        private World world;
        public World GetWorld()
        {
            return world;
        }

        private List<Entity> entities;
        public List<Entity> GetEntities()
        {
            return entities;
        }

        public Entity GetEntityWithId(int id)
        {
            foreach (Entity entity in GetEntities())
            {
                if (entity.GetId() == id)
                {
                    return entity;
                }
            }

            return null;
        }

        private int lastEntityId = 0;

        public void BroadcastPacket(ClientboundPacket clientboundPacket)
        {
            foreach(Connection connection in GetPlayerConnections())
            {
                connection.SendPacket(clientboundPacket);
            }
        }
    }
}
