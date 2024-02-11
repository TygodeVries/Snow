using Snow.Formats.Nbt;
using Snow.Level.Chunks.ChunkSections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Level
{
    public class Chunk
    {
        public World world;

        public ChunkSection[] chunkSections;

        public Chunk(World world)
        {
            chunkSections = new ChunkSection[world.SectionsPerColumn];
            for (int i = 0; i < world.SectionsPerColumn; i++)
            {
                chunkSections[i] = new SolidChunkSection(0, 0);
            }

            this.world = world;
        }

        public void SetBlock(int x, int y, int z, short type)
        {
            int chunkSectionIndex = (int) Math.Floor((double) y / 16);
            int sectionY = y % 16;

            ChunkSection cs = chunkSections[chunkSectionIndex];

            if(cs.GetType() == typeof(SolidChunkSection))
            {
                chunkSections[chunkSectionIndex] = new DetailedChunkSection();
            }

            DetailedChunkSection detailedChunkSection = (DetailedChunkSection) chunkSections[chunkSectionIndex];
            detailedChunkSection.SetBlock(x, sectionY, z, type);
        }


        byte[] bytes;
        NbtCompoundTag heightMap = new NbtCompoundTag();

        public NbtCompoundTag GetHeightmaps()
        {
            return heightMap;
        }

        public byte[] GetChunkData()
        {
            return bytes;
        }

        /// <summary>
        /// Build the chunk based on the chunk sections
        /// </summary>
        public void Bake()
        {
            bytes = new byte[0];

            byte[] blockCount = BitConverter.GetBytes((short)9999);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(blockCount);

            foreach (ChunkSection chunkSection in chunkSections)
            {
                bytes = bytes.Concat(blockCount).ToArray();

                bytes = bytes.Concat(chunkSection.GetBlocks()).ToArray();

                bytes = bytes.Concat(chunkSection.GetBiomes()).ToArray();
            }
        }


    }
}
