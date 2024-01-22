using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Formats
{
    internal class VarInt
    {
        static int SEGMENT_BITS = 0x7F;
        static int CONTINUE_BIT = 0x80;

        public static byte[] ToByteArray(uint value)
        {
            List<byte> bytes = new List<byte>();

            while ((value & ~SEGMENT_BITS) != 0)
            {
                bytes.Add((byte)((value & SEGMENT_BITS) | CONTINUE_BIT));
                value >>= 7;
            }

            bytes.Add((byte)value);

            return bytes.ToArray();
        }
    }
}
