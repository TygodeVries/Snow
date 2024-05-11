using Snow.Servers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Worlds
{
    public class WorldManager
    {
        private Dictionary<string, World> _worlds;

        private Server server;
        public WorldManager(Server server)
        {
            _worlds = new Dictionary<string, World>();
            this.server = server;
        }

        /// <summary>
        /// Get the world set as default in the settings.json
        /// </summary>
        /// <returns></returns>
        public World GetDefaultWorld()
        {
            return 
                GetWorld(server.GetSettings().GetString("default-world"));
        }

        /// <summary>
        /// Create server/worlds
        /// </summary>
        private void CreateWorldsDir()
        {
            Directory.CreateDirectory($"{server.GetWorkPath()}/Worlds");
        }

        /// <summary>
        /// Create a new world on the server
        /// </summary>
        /// <param name="name">Name of the world</param>
        /// <returns>The world that was created or null if the world was already made</returns>
        public World CreateWorld(string name)
        {
            CreateWorldsDir();

            string workPath = server.GetWorkPath();
            string worldDir = $"{workPath}/Worlds/{name}";

            Log.Send($"Loading world {name}...");

            if (Directory.Exists(worldDir)) {

                // Create directory if it dousent exist yet
                Directory.CreateDirectory(worldDir);

                // #TODO Do more first world creating...
                Log.Send($"Creating world named {name} for the first time...");
            }

            World world = new World(name, server, 384);
            
            // #TODO
            // Load world data from files

            _worlds.Add(world.GetName(), world);
            return world;
        }
        
        /// <summary>
        /// Get a world by name
        /// </summary>
        /// <param name="name">The name of the world</param>
        /// <returns></returns>
        public World GetWorld(string name)
        {
            return _worlds[name];
        }

        /// <summary>
        /// Create all the worlds set in the settings.json
        /// </summary>
        public void CreateConfigurationWorlds()
        {
            foreach (string worldName in server.GetSettings().GetStringArray("worlds"))
            {
                CreateWorld(worldName);
            }
        }
    }
}
