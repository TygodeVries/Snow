using Snow.Formats;
using Snow.Level.Chunks.ChunkSections;
using Snow.Level.Entities;
using Snow.Network;
using Snow.Network.Packets.Play.Clientbound;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Level
{
    public class World
    {
        public int SectionsPerColumn = 24;
       
        public static World LoadFromFile(string path)
        {
            World world = new World();

            byte[] data = File.ReadAllBytes(path);
            
            for(int i = 0; i < data.Length; i++)
            {
                int x = BitConverter.ToInt32(data, i);
                int z = BitConverter.ToInt32(data, (i += 4));
                i += 4;

                Chunk chunk = new Chunk(world);
                world.chunks.Add((x, z), chunk);
                for (int m = 0; m < world.SectionsPerColumn; m++)
                {
                    if (data[i] == 0x00)
                    {
                        int blockType = BitConverter.ToInt32(data, i++);
                        int biomeType = BitConverter.ToInt32(data, i += 4);
                        i += 4;

                        SolidChunkSection solidChunkSection = new SolidChunkSection(blockType, biomeType);
                        chunk.chunkSections[m] = solidChunkSection;
                    }
                }
            }

            return world;
        }

        Dictionary<(int x, int z), Chunk> chunks = new Dictionary<(int x, int z), Chunk>();

        public Chunk GetChunkAt(int x, int z)
        {
            return chunks[(x, z)];
        }

        public void SetBlockAt(int x, int y, int z, short type)
        {
            Chunk chunk = GetChunkAt(x / 16, z / 16);
            chunk.SetBlock(x % 16, y, z % 16, type);
        }

    }
}
