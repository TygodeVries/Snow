using Snow.Entities;
using Snow.Formats;
using Snow.Servers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Worlds
{
    public class World
    {
        private Server server;

        public World(string name, Server server)
        {
            this.server = server;
            this.name = name;
        }

        private string name;
        public string GetName() { return name; }

        private WorldGenerator worldGenerator;
        public void SetWorldGenerator(WorldGenerator worldGenerator)
        {
            this.worldGenerator = worldGenerator;
        }

        int worldHeight = 24 * 16;
        public int GetWorldHeight()
        {
            return worldHeight;
        }

        public void SpawnEntity(Entity entity)
        {
            entity.SetServer(server);
            entity.SetWorld(this);
            entity.SetUUID(UUID.Random());
        }

        public Dictionary<(int, int), Chunk> chunks = new Dictionary<(int, int), Chunk>();

        public Chunk GetChunk((int, int) loc)
        {
            if(!chunks.ContainsKey(loc))
            {
                Chunk chunk = worldGenerator.Generate(this, loc.Item1, loc.Item2);
                chunks.Add(loc, chunk);
            }

            return chunks[loc];
        }
       
    }
}
