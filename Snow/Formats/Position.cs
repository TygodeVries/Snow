using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Formats
{
    internal class Position
    {
        public int x;
        public int y;
        public int z;

        public Position(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public byte[] ToByteArray()
        {
            long encodedValue = ((x & 0x3FFFFFFL) << 38) | ((z & 0x3FFFFFFL) << 12) | (y & 0xFFF);

            byte[] byteArray = BitConverter.GetBytes(encodedValue);

            if (BitConverter.IsLittleEndian)
                Array.Reverse(byteArray);
            

            return byteArray;
        }
    }
}
