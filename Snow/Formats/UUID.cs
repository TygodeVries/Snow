using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Formats
{
    internal class UUID
    {
        // A UUID is a 16 byte value (or an Int128)
        byte[] value = new byte[16];

        public void SetBytes(byte[] bytes)
        {
            value = bytes;
        }

        public byte[] GetBytes()
        {
            return value;
        }

        public UUID(byte[] bytes)
        {
            this.value = bytes;
        }

        public static UUID Random()
        {
            Random random = new Random();

            byte[] bytes = new byte[16];

            random.NextBytes(bytes);
            UUID uuid = new UUID(bytes);

            return uuid;
        }
    }
}
