using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Level
{
    internal class WorldManager
    {
        private List<World> worlds = new List<World>();

        public WorldManager()
        {
            World world = new World();
            world.SetBlockAt(0, -1, 0, 1);
            world.SetBlockAt(1, -1, 0, 1);
            world.SetBlockAt(2, -1, 0, 1);
            world.SetBlockAt(-2, -1, 2, 1);
            AddWorld(world);
        }

        public World GetWorld(int id)
        {
            return worlds[id];
        }

        public void AddWorld(World world)
        {
            worlds.Add(world);
        }
    }
}
