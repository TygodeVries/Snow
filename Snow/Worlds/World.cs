using Snow.Entities;
using Snow.Events;
using Snow.Events.Arguments;
using Snow.Formats;
using Snow.Levels;
using Snow.Network;
using Snow.Network.Packets.Play.Clientbound;
using Snow.Servers;
using Snow.Worlds.Generator;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Snow.Worlds
{
    public class World
    {

        /// <summary>
        /// Create a new world.
        /// </summary>
        /// <param name="name">The name of the world</param>
        /// <param name="server">The server this world is going to be used on</param>
        /// <param name="worldHeight">Default world height is 384</param>
        public World(string name, Server server, int worldHeight)
        {
            this._server = server;
            this._name = name;
            this._worldHeight = worldHeight;

            Directory.CreateDirectory(GetFolder());
        }

        public string GetFolder()
        {
            return $"{GetServer().GetWorkPath()}/Worlds/{GetName()}";
        }

        private List<Entity> _entities = new List<Entity>();
        public List<Entity> GetEntities()
            { return _entities; }

        private Server _server;
        public Server GetServer()
            { return _server; }


        private string _name;
        public string GetName()
            { return _name; }

        private WorldGenerator _worldGenerator = null;
        public void SetWorldGenerator(WorldGenerator worldGenerator)
            { this._worldGenerator = worldGenerator; }

        private bool hasWarned = false;
        public WorldGenerator GetWorldGenerator()
        {
            if(_worldGenerator == null)
            {
                if(!hasWarned)
                    Log.Warn($"The world generator in world {GetName()} is invalid. Switching to 'Flat'");
                
                hasWarned = true;

                _worldGenerator = new FlatGenerator();
            }

            return _worldGenerator;
        }

        int _worldHeight = 24 * 16;
        public int GetWorldHeight()
        {
            return _worldHeight;
        }

        private int currentEntityId = 0;

        /// <summary>
        /// Spawn an entity in the world
        /// </summary>
        /// <param name="entity"></param>
        public void SpawnEntity(Entity entity)
        {
            entity.SetServer(_server);

            entity.SetWorld(this);

            entity.SetUUID(UUID.Random());
            entity.SetId(currentEntityId);
            currentEntityId++;

            _entities.Add(entity);


            List<Connection> con = new List<Connection>();
            if (entity.GetType() == typeof(Player))
            {
                con.Add(((Player) entity).GetConnection());
            }

            entity.Spawn();
        }
        
        public void TickEntities()
        {
            foreach(Entity entity in  _entities)
            {
                entity.OnEntityTick?.Invoke(this, new EventArgs());
            }
        }

        internal void RemoveEntity(Entity entity)
        {
            _entities.Remove(entity);
        }

        private Dictionary<(int, int), Chunk> loadedChunks = new Dictionary<(int, int), Chunk>();
        public Chunk GetChunk((int, int) location)
        {
            if (loadedChunks.ContainsKey(location))
            {
                return loadedChunks[location];
            }

            Log.Err($"The chunk at {location} is not loaded. Please make sure to load the chunk before getting it.");
            return null;
        }

        public bool IsChunkLoaded((int, int) location)
        {
            return loadedChunks.ContainsKey(location);
        }

        public async Task LoadChunkAsync((int, int) location)
        {
            Chunk chunk = new Chunk(this, location.Item1, location.Item2);

            if(File.Exists(GetFolder() + "/"))
            {

            }

            await Task.Run(() => GetWorldGenerator().Generate(chunk));
            lock (loadedChunks)
            {
                loadedChunks.Add(location, chunk);
            }
        }

        public void UnloadChunk((int, int) location)
        {
            loadedChunks.Remove(location);
        }

        /// <summary>
        /// Broadcast a packet in this world
        /// </summary>
        /// <param name="packet">The packet to broadcast</param>
        public void BroadcastPacket(ClientboundPacket packet)
        {
            BroadcastPacket(packet, new List<Connection>());
        }

        /// <summary>
        /// Broadcast a message but exludes a few clients
        /// </summary>
        /// <param name="packet">The packet</param>
        /// <param name="exclude">A list of connections to ignore</param>
        public void BroadcastPacket(ClientboundPacket packet, List<Connection> exclude)
        {
            foreach (Player player in GetPlayers())
            {
                if (!exclude.Contains(player.GetConnection()))
                {
                    player.GetConnection().SendPacket(packet);
                }
            }
        }

        /// <summary>
        /// Get a list of all players in this world
        /// </summary>
        /// <returns></returns>
        public List<Player> GetPlayers()
        {
            List<Player> players = new List<Player>();

            lock (_entities)
            {
                foreach (Entity entity in _entities)
                {
                    if (entity.GetType() == typeof(Player))
                    {
                        Player player = (Player)entity;

                        players.Add(player);
                    }
                }
            }

            return players;
        }

        /// <summary>
        /// Set a block to a posistion
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <param name="blockType"></param>
        public void SetBlockAt(int x, int y, int z, BlockType blockType)
        {
            SetBlockAt(x, y, z, blockType, null);
        }

        public BlockType? GetBlockAt(int x, int y, int z)
        {
            int chunkX = (int)Math.Floor(x / 16f);
            int chunkZ = (int)Math.Floor(z / 16f);

            if (!IsChunkLoaded((chunkX, chunkZ)))
            {
                return null;
            }

            Chunk chk = GetChunk((chunkX, chunkZ));

            BlockType blockType = chk.GetBlockAt(new Position(ModuloNegative(x, 16), y, ModuloNegative(z, 16)));

            return blockType;
        }

        public void SetBlockAt(int x, int y, int z, BlockType blockType, Player player)
        {
            EventHandler<OnBlockPlaceArgs> eventHandler = OnBlockPlace;

            if(OnBlockPlace != null)
                OnBlockPlace.Invoke(this, new OnBlockPlaceArgs(player, new Position(x, y, z), blockType));

            int chunkX = (int) Math.Floor(x / 16f);
            int chunkZ = (int)Math.Floor(z / 16f);

            if (!IsChunkLoaded((chunkX, chunkZ)))
            {
                Log.Err($"Attempted to place a block at ({x}, {y}, {z}) in chunk that is unloaded. Make sure to load the chunk first.");
                return;
            }

            // Update chunk to have block
            Chunk chk = GetChunk((chunkX, chunkZ));

            chk.SetBlockAt(new Position(ModuloNegative(x, 16), y, ModuloNegative(z, 16)), blockType);

            // Create packet

            BlockUpdatePacket packet = new BlockUpdatePacket(new Position(x, y, z), (int) blockType);
            chk.BroadcastPacket(packet);
        }

        private int ModuloNegative(int a, int b)
        {
            if (a < 0)
                return b-(-a % b);

            return a % b;
        }
        
        public EventHandler<OnBlockPlaceArgs> OnBlockPlace;

        /// <summary>
        /// Returns null if nothing is hit.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="direction"></param>
        /// <param name="lenght"></param>
        /// <param name="stepSize"></param>
        /// <param name="ignore"></param>
        /// <returns></returns>
        public RaycastResult Raycast(Vector3d start, Vector3d direction, int lenght, float stepSize, BlockType[] ignore)
        {
            Vector3d nDirection = direction.Normalized();

            Vector3d p = start.Clone();

            for(int i = 0; i < (float) lenght * (1f / stepSize); i++)
            {
                p += nDirection / stepSize;

                BlockType? blockType = GetBlockAt((int)p.x, (int)p.y, (int)p.z);
                if(blockType == null)
                {
                    // Chunk not loaded.
                    break;
                }

                if(!ignore.Contains((BlockType) blockType))
                {
                    return new RaycastResult(p);
                }
            }

            return null;
        }
    }
}
