using System;
using System.Collections;
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
            if (face == 3) position.z += 1;

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

        public static Position FromByteArray(byte[] bytes)
        {
            Array.Reverse(bytes);

            long val = BitConverter.ToInt64(bytes, 0);
            int x, y, z;
            DecodePosition(val, out x, out y, out z);

            return new Position(x, y, z);
        }

        static void DecodePosition(long val, out int x, out int y, out int z)
        {
            x = (int)(val >> 38);
            y = (int)((val & 0xFFF) << 52 >> 52);
            z = (int)((val << 26) >> 38);

            // Adjust for sign extension
            if (x >= 1 << 25) x -= 1 << 26;
            if (y >= 1 << 11) y -= 1 << 12;
            if (z >= 1 << 25) z -= 1 << 26;
        }
    }
}
