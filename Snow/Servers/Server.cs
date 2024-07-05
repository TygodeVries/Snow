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
using Snow.Items;
using Snow.Events.Arguments;
using Snow.Levels;
using System.Diagnostics;
using Snow.Servers.Registries;
using System.Security.Cryptography.X509Certificates;

namespace Snow.Servers
{
    public class Server
    {
        public Server(int port, string workPath)
        {
            this.port = port;
            running = false;
            this.workPath = workPath;
            eventManager = new EventManager();
            itemRegistry = new ItemRegistry();
            commandManager = new CommandManager();
            connectionListener = new ConnectionListener(port, this);
            worldManager = new WorldManager(this);
        }


        private CommandManager commandManager;
        public CommandManager GetCommandManager() => commandManager;

        public List<Connection> GetPlayerConnections() => GetConnectionListener().GetConnections();
        private Configuration settings;
        public Configuration GetSettings() => settings;

        private Configuration language;
        public Configuration GetLanguage() => language;

        private EventManager eventManager;
        public EventManager GetEventManager() => eventManager;

        public ItemRegistry itemRegistry;

        string workPath;
        public string GetWorkPath() => workPath;

        WorldManager worldManager;
        public WorldManager GetWorldManager() => worldManager;

        int port;
        public int GetPort() => port;

        bool running;
        public bool IsRunning() => running;

        long tickCount = 0;
        public long GetTick() => tickCount;

        private ConnectionListener connectionListener;
        internal ConnectionListener GetConnectionListener() => connectionListener;

        private AddonManager addonManager;
        public AddonManager GetAddonManager() => addonManager;

        /// <summary>
        /// Start the server
        /// </summary>
        public void Start()
        {
            Log.Send($"Starting server on port {GetPort()}...");

            // Create work folder
            if(!Directory.Exists(GetWorkPath()))
            {
                Log.Send("Creating working directory...");
                Directory.CreateDirectory(GetWorkPath());
            }

            // Create configurations
            settings = new Configuration($"{GetWorkPath()}/Settings.json", "Data/SettingsTemplates/Settings.json");
            language = new Configuration($"{GetWorkPath()}/Language.json", "Data/SettingsTemplates/Language.json");

            GetCommandManager().RegisterBuildIn();
            GetWorldManager().CreateConfigurationWorlds();
            GetConnectionListener().Start();

            registryManager.CreateRegistries();
            RegisterTempTestRegistries();

            Thread thr = new Thread(ServerThread);
            thr.Start();

        }

        /// <summary>
        /// Stop the server
        /// </summary>
        public void Stop()
        {
            GetAddonManager().StopAll();

            BroadcastPacket(new DisconnectPacket(new TextComponent(GetLanguage().GetString("disconnect.stop"))));

            Log.Send("Stopped the server.");
            running = false;
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

            running = true;

            double[] pastTickTime = new double[20 * 5];

            Process currentProcess = System.Diagnostics.Process.GetCurrentProcess();

            Log.Send("Server is running!");
            while (running)
            {
                DateTime startTime = DateTime.Now;

                // Tick all things in this server
                Tick();

                // Wait until the next tick can be run.
                double mspt = DateTime.Now.Subtract(startTime).TotalMilliseconds;
                if ((int)(50 - mspt) > 0)
                {
                    Thread.Sleep((int)(50 - mspt));
                }

                pastTickTime[GetTick() % pastTickTime.Length] = mspt;

                double tot = 0;
                for(int i = 0; i < pastTickTime.Length; i++)
                {
                    tot = pastTickTime[i];
                }

                currentProcess.Refresh();
                long memoryUsage = currentProcess.WorkingSet64;

                string format = "B";
                if(memoryUsage >= 1024)
                {
                    format = "KB";
                    memoryUsage /= 1024;
                }

                if (memoryUsage >= 1024)
                {
                    format = "MB";
                    memoryUsage /= 1024;
                }

                if (memoryUsage >= 1024)
                {
                    format = "GB";
                    memoryUsage /= 1024;
                }

                Console.Title = $"Snow Server | MSPT ({pastTickTime.Length / 20} Seconds): {tot / (double) pastTickTime.Length} | RAM: {memoryUsage}{format}";
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
            GetConnectionListener().DisconnectInactiveClients();
            GetWorldManager().TickEntities();
            if(tickCount % 5 == 0)
            {
                foreach(Connection connection in GetPlayerConnections())
                {
                    connection.SendEntities();
                }
            }
        }

        /// <summary>
        /// Broadcast a packet server wide
        /// </summary>
        /// <param name="clientboundPacket"></param>
        public void BroadcastPacket(ClientboundPacket clientboundPacket)
        {
            foreach (Connection connection in GetPlayerConnections())
            {
                connection.SendPacket(clientboundPacket);
            }
        }

        public List<Registry> registries = new List<Registry>();
        public List<Registry> GetRegistries()
        {
            return registries;
        }

        public RegistryManager registryManager { get; } = new RegistryManager();


        public void RegisterTempTestRegistries()
        {

            string[] damages = new string[] { 
                "in_fire", "campfire", "lightning_bolt", "on_fire", "lava", "hot_floor", "in_wall", "cramming", 
                "drown", "starve", "cactus", "fall", "fly_into_wall", "out_of_world", "generic", "magic",
                "wither", "dragon_breath", "dry_out", "sweet_berry_bush", "freeze", "stalagmite", "outside_border",
                "generic_kill"};
            foreach(string damage in damages)
            {
                registryManager.AddDamageType(new DamageType(new Identifier("minecraft", damage), "onFire", "never", 0.0f));
            }

            registryManager.AddDimensionType(new DimensionType(new Identifier("snow", "world"), 0x01, 0x00, 0x00, 0x01, 1.0, 0x01, 0x00, 0, GetWorldManager().GetDefaultWorld().GetWorldHeight(), 10, "#", "minecraft:overworld", 0.0f, 0x00, 0x01, 1, 0));
            string[] biomes = new string[] { "plains" };
            foreach(string biome in biomes)
            {
                registryManager.AddBiome(new Biome(new Identifier("minecraft", biome), 0x01, 1, 1, new BiomeEffects(16711680, 0, 0, 65280, 0, 0)));
            }


            registryManager.AddWolfVariant(new WolfVariant(new Identifier("minecraft", "ashen"), "minecraft:entity/wolf/wolf_spotted", "minecraft:entity/wolf/wolf_spotted_tame", "minecraft:entity/wolf/wolf_spotted_angry", "#minecraft:is_savanna"));

            registryManager.AddPaintingVariant(new PaintingVariant(new Identifier("minecraft", "featherfall"), "minecraft:featherfall", 1, 1));
        }

    }
}   
