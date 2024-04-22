using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using Snow.Network;
using Snow.Network.Packets.Play.Clientbound;
using Snow.Addons;
using Snow.Formats;
using Snow.Entities;
using Snow.Commands;
using Snow.Events;
using Snow.Worlds;
using System.IO;

namespace Snow.Servers
{
    public class Server
    {
        private CommandManager _commandManager;
        public CommandManager GetCommandManager()
        {
            return _commandManager;
        }

        public List<Connection> GetPlayerConnections()
        {
            return GetConnectionListener().GetConnections();
        }

        private Configuration _settings;
        public Configuration GetSettings()
        {
            return _settings;
        }

        private Configuration _language;
        public Configuration GetLanguage()
        {
            return _language;
        }

        string _workPath;
        public string GetWorkPath()
        {
            return _workPath;
        }

        WorldManager _worldManager;
        public WorldManager GetWorldManager()
        {
            return _worldManager;
        }

        int _port;
        public int GetPort()
        {
            return _port;
        }

        bool _running;
        public bool IsRunning()
        {
            return _running;
        }

        long tickCount = 0;
        public long GetTick()
        {
            return tickCount;
        }

        private ConnectionListener _connectionListener;
        internal ConnectionListener GetConnectionListener()
        {
            return _connectionListener;
        }

        public Server(int port, string workPath)
        {
            _running = false;
            _workPath = workPath;
            _commandManager = new CommandManager();
            _connectionListener = new ConnectionListener(port, this);
            _worldManager = new WorldManager(this);

            // Load configs
            _settings = new Configuration($"{GetWorkPath()}/Settings.json", "Data/SettingsTemplates/Settings.json");
            _language = new Configuration($"{GetWorkPath()}/Language.json", "Data/SettingsTemplates/Language.json");
        }

        /// <summary>
        /// Start the server
        /// </summary>
        public void Start()
        {
            Log.Send($"Starting server on port {GetPort()}...");

            GetCommandManager().RegisterBuildIn();
            GetWorldManager().CreateConfigurationWorlds();
            GetConnectionListener().Start();

            Thread thr = new Thread(ServerThread);
            thr.Start();
        }

        /// <summary>
        /// Stop the server
        /// </summary>
        public void Stop()
        {
            addonManager.StopAll();

            BroadcastPacket(new DisconnectPacket(new TextComponent(GetLanguage().GetString("disconned.stop"))));

            Log.Send("Stopped the server.");
            _running = false;
        }

        /// <summary>
        /// Broadcast a message across the server
        /// </summary>
        /// <param name="text"></param>
        public void BroadcastMessage(TextComponent text)
        {
            foreach (Connection con in GetPlayerConnections())
            {
                Player player = con.GetPlayer();
                if(player != null)
                {
                    player.SendSystemMessage(text);
                }
            }
        }


        /// <summary>
        /// Thread hosting the accual server.
        /// I don't recommend editing this unless you know what your doing.
        /// </summary>
        private void ServerThread()
        {
            addonManager = new AddonManager(this);
            addonManager.LoadAllAddons();

            _running = true;

            Log.Send("Server is running!");
            while (_running)
            {
                DateTime startTime = DateTime.Now;

                // Tick all things in this server
                Tick();

                // Wait until the next tick can be run.
                double mspt = DateTime.Now.Subtract(startTime).TotalMilliseconds;
                if ((int)(50 - mspt) > 0)
                {
                    Console.Title = $"MSPT: {mspt}";
                    Thread.Sleep((int)(50 - mspt));
                }
            }
        }
        
        /// <summary>
        /// Runs every 50 ms to progress the server
        /// </summary>
        public void Tick()
        {
            tickCount++;

            GetConnectionListener().Tick();
            GetAddonManager().Tick();
        }


        private AddonManager addonManager;
        public AddonManager GetAddonManager()
        {
            return addonManager;
        }

        public void BroadcastPacket(ClientboundPacket clientboundPacket)
        {
            foreach(Connection connection in GetPlayerConnections())
            {
                connection.SendPacket(clientboundPacket);
            }
        }
    }
}
