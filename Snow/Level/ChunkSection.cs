using Snow.Formats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Level
{
    internal class ChunkSection
    {
        /// <summary>
        /// Get the blockstates of a chunk section
        /// </summary>
        /// <returns></returns>
        public byte[] GetBlockStates()
        {
            byte[] bitsPerEntry = new byte[] { (byte) 0x0F }; // 16 bits (2 bytes) per entry.

            byte[] pallet = new byte[] { 0x00 };
            byte[] palletData = VarInt.ToByteArray((uint)pallet.Length).Concat(pallet).ToArray();

            short[] blocks = new short[4096];

            // Create some cool block patern
            for(int i = 0; i <  blocks.Length; i++) 
                blocks[i] = (short) (i % 20);

            byte[] chunkSection = new byte[0];

            int longsInArray = blocks.Length / 8;
            byte[] longsInArrayAsBytes = VarInt.ToByteArray((uint) longsInArray);

            chunkSection = chunkSection.Concat(bitsPerEntry)


            bytes = bytes.Concat(palletData).Concat(.Concat(blocks).ToArray();

            return chunkSection;
        }

        /// <summary>
        /// Get the biomes of a chunk
        /// </summary>
        /// <returns></returns>
        public byte[] GetBiomes()
        {
            return new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00 };
        }
    }
}
