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
using Snow.Server;
using Snow.Commands;
using Snow.Events;
using System.Reflection;
using Snow.Events.Args;
using Snow.Items;
namespace Snow
{
    public class Lobby
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

        public Lobby(int port)
        {
            tcpListener = new TcpListener(IPAddress.Any, port);
            playerConnections = new List<Connection>();
            entities = new List<Entity>();
            eventManager = new EventManager();

            Thread thread = new Thread(this.LobbyThread);
            thread.Start();
        }

        private BlockMask blockMask;
        public void SetBlockMask(BlockMask blockMask)
        {
            this.blockMask = blockMask;
        }

        public void SetBlockAt(int x, int y, int z, BlockType blockType)
        {
            SetBlockAt(x, y, z, blockType, null);
        }

        public void SetBlockAt(int x, int y, int z, BlockType blockType, Player player)
        {
            GetEventManager().ExecuteBlockPlace(new BlockPlaceEventArgs(player, new Position(x, y, z)));

            GetBlockMask().SetBlock(blockType, x, y, z);
        }

        public BlockType GetBlockAt(int x, int y, int z)
        {
            BlockType? block = GetBlockMask().GetBlockAt(x, y, z);
            if(block == null)
            {
                return GetCurrentLevel().GetBlockAt(x, y, z);
            }

            return block.Value;
        }

        public BlockMask GetBlockMask()
        {
            return blockMask;
        }

        public void Stop()
        {
            addonManager.StopAll();
        }
        private void LobbyThread()
        {
            tcpListener.Start();

            commandManager = new CommandManager();
            SetLevel(LevelManager.GetLevel(Configuration.GetDefaultLevelName()));

            GetEventManager().RightClickBlock += BlockPlaceAttempt;

            Log.Send("Loading addons...");
            addonManager = new AddonManager(this);
            addonManager.LoadAllAddons();

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

            TickAddons();
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
                    player.SendPacket(new UpdateTime());
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
        internal void TickAddons()
        {

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

        private Level level;
        public Level GetCurrentLevel()
        {
            return level;
        }
        public void SetLevel(Level level)
        {
            this.level = level;

            foreach(Connection connection in playerConnections)
            {
                connection.SendLevel(level);
            }
        }

        private List<Entity> entities;
        public List<Entity> GetEntities()
        {
            return entities;
        }
        public void SpawnEntity(Entity entity)
        {
            entity.SetLobby(this);
            entity.SetUUID(UUID.Random());
            entity.SetId(lastEntityId);

            entities.Add(entity);
            lastEntityId++;
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
