using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Formats.Nbt.Values
{
    internal class NbtDoubleTag : NbtTag
    {
        double value;

        public NbtDoubleTag(double value)
        {
            this.type = 0x06;
            this.value = value;
        }

        public override byte[] Encode()
        {
            byte[] bytes = BitConverter.GetBytes(value);

            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytes);
            }

            return bytes;
        }
    }
}
