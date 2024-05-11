﻿using System;
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
            itemManager = new ItemManager();
            commandManager = new CommandManager();
            connectionListener = new ConnectionListener(port, this);
            worldManager = new WorldManager(this);
        }

        private ItemManager itemManager;
        public ItemManager GetItemManager() => itemManager;

        private CommandManager commandManager;
        public CommandManager GetCommandManager() => commandManager;

        public List<Connection> GetPlayerConnections() => GetConnectionListener().GetConnections();
        private Configuration settings;
        public Configuration GetSettings() => settings;

        private Configuration language;
        public Configuration GetLanguage() => language;

        private EventManager eventManager;
        public EventManager GetEventManager() => eventManager;

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

            Thread thr = new Thread(ServerThread);
            thr.Start();
            TestCode();
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

                Console.Title = $"MSPT (Over {pastTickTime.Length / 20} second(s)): {tot / (double) pastTickTime.Length}";
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

        /// <summary>
        /// Broadcast a packet server wide
        /// </summary>
        /// <param name="clientboundPacket"></param>
        public void BroadcastPacket(ClientboundPacket clientboundPacket)
        {
            foreach(Connection connection in GetPlayerConnections())
            {
                connection.SendPacket(clientboundPacket);
            }
        }


        // #TODO
        // REMOVE THIS IN LATER VERSION
        public void TestCode()
        {
            GetItemManager().RegisterItemType(new ItemType("snow:test", 1, 0, "Test Item", Levels.BlockType.STONE));
            GetEventManager().PlayerJoinEvent += OnPlayerJoinEvent;
        }

        public void OnPlayerJoinEvent(object sender, OnPlayerJoinArgs args)
        {
            args.player.GetInventory().SetItem(36, new ItemStack(GetItemManager().GetNamespace("snow:test")));
            
            args.player.GetInventory().SetItem(37, new ItemStack(new ItemType("1", 2, 0, "", BlockType.OAK_PLANKS), (byte) 40));
        }
    }
}
