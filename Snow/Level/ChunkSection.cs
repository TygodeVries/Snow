using Snow.Formats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Level
{
    public class ChunkSection
    {
        /// <summary>
        /// Get the blockstates of a chunk section
        /// </summary>
        /// <returns></returns>
        public byte[] GetBlockStates()
        {
            byte[] bitsPerEntry = new byte[] { (byte) 0x0F }; // 16 bits (2 bytes) per entry.

            // No palette

            short[] blocks = new short[4096];

            // Create some cool block patern
            for(int i = 0; i <  blocks.Length; i++) 
                blocks[i] = (short) (i % 20);

            int longsInArray = blocks.Length / 4;
            byte[] longsInArrayAsBytes = VarInt.ToByteArray((uint) longsInArray);

            byte[] blocksAsBytes = BlocksToBytes(blocks);

            return bitsPerEntry.Concat(longsInArrayAsBytes).Concat(blocksAsBytes).ToArray();
        }

        public byte[] GetSingleBlockChunkData()
        {
            byte[] bitsPerEntry = new byte[] { (byte)0x0 };

            return bitsPerEntry.Concat(VarInt.ToByteArray(0)).Concat(VarInt.ToByteArray(0)).ToArray();
        }

        byte[] BlocksToBytes(short[] blocks)
        {
            byte[] bytes = new byte[blocks.Length * sizeof(short)];

            for (int i = 0; i < blocks.Length; i++)
            {
                BitConverter.GetBytes(blocks[i]).CopyTo(bytes, i * sizeof(short));
            }

            return bytes;
        }
        /// <summary>
        /// Get the biomes of a chunk
        /// </summary>
        /// <returns></returns>
        public byte[] GetBiomes()
        {
            byte[] bitsPerEntry = new byte[] { (byte)0x0 };

            return bitsPerEntry.Concat(VarInt.ToByteArray(0)).Concat(VarInt.ToByteArray(0)).ToArray();
        }
    }
}
