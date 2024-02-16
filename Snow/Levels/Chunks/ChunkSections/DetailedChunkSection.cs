using Snow.Formats;
using Snow.Levels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Levels
{
    internal class DetailedChunkSection : ChunkSection
    {
        short[,,] blocks = new short[16, 16, 16];

        public override byte[] GetBiomes()
        {
            //#TODO implement correct biomes
            byte[] bitsPerEntry = new byte[] { (byte)0x0 };
            return bitsPerEntry.Concat(VarInt.ToByteArray((uint) 0)).Concat(VarInt.ToByteArray(0)).ToArray();
        }

        public override byte[] GetBlocks()
        {
            byte bitsPerBlock = 15;
            int dataLength = (16 * 16 * 16) * bitsPerBlock / 64;

            long[] data = new long[dataLength];

            int HEIGHT = 16;
            int WIDTH = 16;

            uint individualValueMask = (uint)((1 << bitsPerBlock) - 1);

            for (int y = 0; y < HEIGHT; y++)
            {
                for(int z = 0; z < WIDTH; z++)
                {
                    for(int x =  0; x < WIDTH; x++)
                    {
                        int blockNumber = (((y * HEIGHT) + z) * WIDTH) + x;
                        int startLong = (blockNumber * bitsPerBlock) / 64;
                        int startOffset = (blockNumber * bitsPerBlock) % 64;
                        int endLong = ((blockNumber + 1) * bitsPerBlock - 1) / 64;

                        long value = blocks[x, y, z];

                        value &= individualValueMask;

                        data[startLong] |= (value << startOffset);

                        if (startLong != endLong)
                        {
                            data[endLong] = (value >> (64 - startOffset));
                        }
                    }
                }
            }

            IEnumerable<byte> output = new byte[] { bitsPerBlock }.Concat(VarInt.ToByteArray((uint)dataLength));
            output.Concat(LongArrayToByteArray(data));

            return output.ToArray();
        }

        private byte[] LongArrayToByteArray(long[] longs)
        {
            byte[] data = new byte[longs.Length * 8];
            for(int i = 0; i < longs.Length; i++)
            {
                byte[] bytes = BitConverter.GetBytes(longs[i]);
                if(BitConverter.IsLittleEndian)
                {
                    Array.Reverse(bytes);
                }

                bytes.CopyTo(data, i * 8);
            }

            return data;
        }

        public void SetBlock(int x, int y, int z, short type)
        {
            blocks[x, y, z] = type;
        }
    }
}
