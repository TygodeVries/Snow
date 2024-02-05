using Snow.Formats.Nbt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Level
{
    public class Chunk
    {
        public int x;
        public int z;

        public World world;

        public NbtCompoundTag GetHeightmaps()
        {
            NbtCompoundTag rootTag = new NbtCompoundTag();
            rootTag.AddFileHeader = false;

            NbtLongArrayTag MOTION_BLOCKING = new NbtLongArrayTag();
            NbtLongArrayTag WORLD_SURFACE = new NbtLongArrayTag();

            MOTION_BLOCKING.values = new long[37];
            WORLD_SURFACE.values = new long[37];

            rootTag.AddField("MOTION_BLOCKING", MOTION_BLOCKING);
            rootTag.AddField("WORLD_SURFACE", WORLD_SURFACE);

            return rootTag;
        }

        public ChunkSection[] chunkSections;

        public Chunk(int x, int z, World world)
        {
            chunkSections = new ChunkSection[world.SectionsPerColumn];
            for (int i = 0; i < world.SectionsPerColumn; i++)
            {
                chunkSections[i] = new ChunkSection();
            }

            this.x = x;
            this.z = z;
            this.world = world;
        }

        public byte[] GetChunkData()
        {
            byte[] bytes = new byte[0];

            byte[] blockCount = BitConverter.GetBytes((short)9999);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(blockCount);

            // Not length prefixed
            foreach (ChunkSection chunkSection in chunkSections)
            {
                // Blocks in chunk
                bytes = bytes.Concat(blockCount).ToArray();

                // Blockstates
                bytes = bytes.Concat(chunkSection.GetSingleBlockChunkData()).ToArray();

                // Biomes
                bytes = bytes.Concat(chunkSection.GetBiomes()).ToArray();
            }

            return bytes;
        }
    }
}
