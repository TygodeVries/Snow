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
using Snow.Items;
using Snow.Worlds;
using Snow.Worlds.Generator;
using System.IO;
using System.Diagnostics.Tracing;

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

        private Configuration configuration;
        public Configuration GetConfiguration()
        {
            return configuration;
        }

        string path;
        public string GetWorkPath()
        {
            return path;
        }

        public Server(int port, string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            this.path = path;
            tcpListener = new TcpListener(IPAddress.Any, port);
            playerConnections = new List<Connection>();
            entities = new List<Entity>();

            configuration = new Configuration($"{GetWorkPath()}/Server.json", "Data/SettingsTemplates/server.json");

            CreateWorlds();

            Thread thread = new Thread(this.LobbyThread);
            thread.Start();
        }

        public void BroadcastMessage(TextComponent text)
        {
            foreach (Connection con in playerConnections)
            {
                Player player = con.GetPlayer();
                if(player != null)
                {
                    player.SendSystemMessage(text);
                }
            }
        }

        private void CreateWorlds()
        {
            Directory.CreateDirectory($"{GetWorkPath()}/Worlds");

            foreach (string worldName in GetConfiguration().GetStringArray("worlds"))
            {
                string worldDir = $"{GetWorkPath()}/Worlds/{worldName}";

                if (!Directory.Exists(worldDir))
                {
                    Directory.CreateDirectory(worldDir);
                }

                new World(worldName, this);
            }
        }

        public void Stop()
        {
            addonManager.StopAll();
        }


        private void LobbyThread()
        {

            addonManager = new AddonManager(this);
            addonManager.LoadAllAddons();

            tcpListener.Start();

            commandManager = new CommandManager();

            Log.Send("Server is running!");
            while (true)
            {
                DateTime startTime = DateTime.Now;

                // Tick all things in this server
                TickLobby();

                // Wait until the next tick can be run.
                double mspt = DateTime.Now.Subtract(startTime).TotalMilliseconds;
                if ((int)(50 - mspt) > 0)
                {
                    Console.Title = $"MSPT: {mspt}";
                    Thread.Sleep((int)(50 - mspt));
                }
            }
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
            GetAddonManager().Tick();
        }


        internal void SendKeepAlive()
        {
            if (GetTick() % 40 == 0)
            {
                foreach (Connection player in playerConnections)
                {
                    player.SendPacket(new KeepAlivePacket(10));
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


        internal void AddWorld(World world)
        {
            worlds.Add(world.GetName(), world);
        }

        private Dictionary<string, World> worlds = new Dictionary<string, World>();
        public World GetWorld(string name)
        {
            return worlds[name];
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

        public World GetDefaultWorld()
        {
            return GetWorld(GetConfiguration().GetString("default-world"));
        }

        public void BroadcastPacket(ClientboundPacket clientboundPacket)
        {
            foreach(Connection connection in GetPlayerConnections())
            {
                connection.SendPacket(clientboundPacket);
            }
        }

        public EventHandler<OnPlayerJoinArgs> OnPlayerJoin;
        public EventHandler<OnPlayerJoinArgs> OnPlayerPreJoin;
    }
}
