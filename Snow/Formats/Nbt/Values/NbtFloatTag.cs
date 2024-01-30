using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Formats.Nbt.Values
{
    internal class NbtFloatTag : NbtTag
    {
        float value;

        public NbtFloatTag(float value) {
            this.value = value;
            this.type = 0x05;
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
