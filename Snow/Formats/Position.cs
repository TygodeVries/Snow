using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Formats
{
    public class Position
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

        public Position GetAdjacent(int face)
        {
            Position position = new Position(x, y, z);

            if (face == 0) position.y -= 1;
            if (face == 1) position.y += 1;

            if(face == 2) position.z -= 1;
            if (face == 3) position.y += 1;

            if (face == 4) position.x -= 1;
            if (face == 5) position.x += 1;

            return position;
        }

        public override string ToString()
        {
            return $"[{x}, {y}, {z}]";
        }

        public byte[] ToByteArray()
        {
            long encodedValue = ((x & 0x3FFFFFFL) << 38) | ((z & 0x3FFFFFFL) << 12) | (y & 0xFFF);

            

            byte[] byteArray = BitConverter.GetBytes(encodedValue);

            if (BitConverter.IsLittleEndian)
                Array.Reverse(byteArray);
            

            return byteArray;
        }

        public static Position FromLong(long l)
        {
            int x = (int)(l >> 38);
            int z = (int)((l >> 12) & 0x3FFFFFF);
            int y = (int)(l & 0xFFF);

            x = SignExtend(x, 26);
            z = SignExtend(z, 26);
            y = SignExtend(y, 12);

            return new Position(x, y, z);
        }

        static int SignExtend(int value, int bitCount)
        {
            int signBit = value & (1 << (bitCount - 1));
            return (signBit != 0) ? (value | (~0 << bitCount)) : value;
        }
    }
}
