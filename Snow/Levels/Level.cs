using Snow.Formats;
using Snow.Formats.Nbt;
using Snow.Levels;
using Snow.Network;
using Snow.Network.Packets.Play.Clientbound;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Levels
{
    /// <summary>
    /// A level is a static world that multiple servers can read from at the same time.
    /// A level is  read only.
    /// </summary>
    public class Level
    {
        public int SectionsPerColumn = 24;

        private string name;
        public void SetName(string name)
        {
            this.name = name;
        }
        public string GetName()
        {
            return name;
        }

        private string folder;

        Dictionary<(int, int), ChunkDataAndUpdateLight> packets = new Dictionary<(int, int), ChunkDataAndUpdateLight>();

        public void SendChunkToConnection(Connection connection, int x, int z)
        {
            if(!packets.ContainsKey((x, z)))
            {
                connection.SendPacket(LevelManager.GetFallbackChunkPacket(x, z));
            }
            else
            {
                connection.SendPacket(packets[(x, z)]);
            }
        }


        private void BakeChunk(int x, int z)
        {
            string executableDirectory = Environment.CurrentDirectory;
            string chunksDirectory = $"{executableDirectory}/{folder}/Chunks";
            string chunkPath = $"{chunksDirectory}/{x}.{z}.chunk";

            if (!File.Exists(chunkPath))
            {
                return;
            }

            NbtCompoundTag heightmap = new NbtCompoundTag();

            NbtLongArrayTag MOTION_BLOCKING = new NbtLongArrayTag();
            NbtLongArrayTag WORLD_SURFACE = new NbtLongArrayTag();
            MOTION_BLOCKING.values = new long[37];
            WORLD_SURFACE.values = new long[37];
            heightmap.AddField("MOTION_BLOCKING", MOTION_BLOCKING);
            heightmap.AddField("WORLD_SURFACE", WORLD_SURFACE);

            byte[] data;
        
            data = File.ReadAllBytes(chunkPath);

            ChunkDataAndUpdateLight packet = new ChunkDataAndUpdateLight(x, z, heightmap, data);
            packets.Add((x, z), packet);
        }

        public void Bake()
        {
            int size = 7;

            int chunkCount = size * size;

            for (int x = -size; x < size; x++)
            {
                for (int z = -size; z < size; z++)
                {
                    BakeChunk(x, z);
                }
            }
        }

        public Level(string path)
        {
            this.folder = path;
        }
    }
}
