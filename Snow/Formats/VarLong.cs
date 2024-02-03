using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Formats
{
    internal class VarLong
    {
        static int SEGMENT_BITS = 0x7F;
        static int CONTINUE_BIT = 0x80;

        public static byte[] ToByteArray(long value)
        {
            List<byte> bytes = new List<byte>();

            while (true)
            {
                if ((value & ~((long)SEGMENT_BITS)) == 0)
                {
                    bytes.Add((byte)value);
                    break;
                }

                bytes.Add((byte)((value & SEGMENT_BITS) | CONTINUE_BIT));

                // Note: >> means that the sign bit is shifted with the rest of the number rather than being left alone
                value >>= 7;
            }

            return bytes.ToArray();
        }

        public static long FromByteArray(byte[] bytes, out int bytesRead)
        {
            long value = 0;
            int position = 0;
            bytesRead = 0;

            while (true)
            {
                byte currentByte = bytes[bytesRead++];
                value |= (long)(currentByte & SEGMENT_BITS) << position;

                if ((currentByte & CONTINUE_BIT) == 0)
                    break;

                position += 7;

                if (position >= 64)
                    throw new Exception("VarLong is too big");
            }

            return value;
        }
    }
}
