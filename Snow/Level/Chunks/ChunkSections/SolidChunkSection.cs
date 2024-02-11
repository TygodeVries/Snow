using Snow.Formats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Level.Chunks.ChunkSections
{
    internal class SolidChunkSection : ChunkSection
    {
        public int blockType = 0;
        public int biomeType = 0;

        public SolidChunkSection(int blockType, int biomeType)
        {
            this.blockType = blockType;
            this.biomeType = biomeType;
        }

        public override byte[] GetBlocks()
        {
            byte[] bitsPerEntry = new byte[] { (byte)0x0 };
            return bitsPerEntry.Concat(VarInt.ToByteArray((uint) blockType)).Concat(VarInt.ToByteArray(0)).ToArray();
        }

        public override byte[] GetBiomes()
        {
            byte[] bitsPerEntry = new byte[] { (byte)0x0 };
            return bitsPerEntry.Concat(VarInt.ToByteArray((uint) biomeType)).Concat(VarInt.ToByteArray(0)).ToArray();
        }

    }
}
