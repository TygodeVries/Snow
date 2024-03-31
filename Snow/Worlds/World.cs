using Snow.Entities;
using Snow.Events;
using Snow.Formats;
using Snow.Levels;
using Snow.Network;
using Snow.Network.Packets.Play.Clientbound;
using Snow.Servers;
using Snow.Worlds.Generator;
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
        List<Entity> entities = new List<Entity>();

        public List<Entity> GetEntities()
        {
            return entities;
        }

        private Server server;

        public World(string name, Server server)
        {
            this.server = server;
            this.name = name;

            server.AddWorld(this);
            Directory.CreateDirectory(GetFolder());
        }

        private string name;
        public string GetName() { return name; }

        private WorldGenerator worldGenerator = null;
        public void SetWorldGenerator(WorldGenerator worldGenerator)
        {
            this.worldGenerator = worldGenerator;
        }

        public WorldGenerator GetWorldGenerator()
        {
            if(worldGenerator == null)
            {
                Log.Err("No world generator set. Using flat.");
                worldGenerator = new FlatGenerator();
            }

            return worldGenerator;
        }

        int worldHeight = 24 * 16;
        public int GetWorldHeight()
        {
            return worldHeight;
        }

        private int currentEntityId = 0;
        public void SpawnEntity(Entity entity)
        {
            entity.SetServer(server);

            entity.SetWorld(this);

            entity.SetUUID(UUID.Random());
            entity.SetId(currentEntityId);
            currentEntityId++;
            
            entities.Add(entity);


            List<Connection> con = new List<Connection>();
            if (entity.GetType() == typeof(Player))
            {
                con.Add(((Player) entity).GetConnection());
            }

            BroadcastPacket(new SpawnEntityPacket(entity), con);
        }
        
        internal void RemoveFromEntities(Entity entity)
        {
            entities.Remove(entity);
        }


        public Dictionary<(int, int), Chunk> loadedChunks = new Dictionary<(int, int), Chunk>();


        public async Task<Chunk> GetChunkAsync((int, int) location)
        {
            if (loadedChunks.ContainsKey(location))
            {
                return loadedChunks[location];
            }

            // Load chunk asynchronously
            await LoadChunkAsync(location);

            // After loading, return the loaded chunk
            return loadedChunks[location];
        }

        public async Task LoadChunkAsync((int, int) location)
        {
            Chunk chunk = await Task.Run(() => GetWorldGenerator().Generate(this, location.Item1, location.Item2));
            loadedChunks.Add(location, chunk);
        }
        public void UnloadChunk((int, int) location)
        {
            loadedChunks.Remove(location);
        }

        public string GetFolder()
        {
            return $"{server.GetWorkPath()}/Worlds/{name}";
        }

        public void BroadcastPacket(ClientboundPacket packet)
        {
            BroadcastPacket(packet, new List<Connection>());
        }

        public void BroadcastPacket(ClientboundPacket packet, List<Connection> exclude)
        {
            foreach (Entity entity in entities)
            {
                if (entity.GetType() == typeof(Player))
                {
                    Player player = (Player)entity;

                    if (!exclude.Contains(player.GetConnection()))
                    {
                        player.GetConnection().SendPacket(packet);
                    }
                }
            }
        }

        public void SetBlockAt(int x, int y, int z, BlockType blockType)
        {
            SetBlockAt(x, y, z, blockType, null);
        }

        public void SetBlockAt(int x, int y, int z, BlockType blockType, Player player)
        {
            EventHandler<OnBlockPlaceArgs> eventHandler = OnBlockPlace;

            if(OnBlockPlace != null)
                OnBlockPlace.Invoke(this, new OnBlockPlaceArgs(player, new Position(x, y, z), blockType));

        }

        public EventHandler<OnBlockPlaceArgs> OnBlockPlace;

    }
}
