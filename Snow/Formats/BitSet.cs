using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Formats
{
    internal class BitSet
    {
        private readonly ulong[] bits;
        public int Length { get; private set; }

        public BitSet(int length)
        {
            this.Length = length;
            int arrayLength = (length + 63) / 64;
            this.bits = new ulong[arrayLength];
        }

        public void Set(int index, bool value)
        {
            if (index < 0 || index >= Length)
            {
                throw new ArgumentOutOfRangeException(nameof(index), "Index out of range");
            }

            int arrayIndex = index / 64;
            int bitIndex = index % 64;

            if (value)
            {
                bits[arrayIndex] |= (ulong)1 << bitIndex;
            }
            else
            {
                bits[arrayIndex] &= ~((ulong)1 << bitIndex);
            }
        }

        public byte[] ToByteArray()
        {
            byte[] result = new byte[(Length + 7) / 8];

            for (int i = 0; i < bits.Length; i++)
            {
                BitConverter.GetBytes(bits[i]).CopyTo(result, i * 8);
            }

            return result;
        }
    }
}
