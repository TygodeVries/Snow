using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Formats.Nbt.Values
{
    internal class NbtByteTag : NbtTag
    {
        byte value;

        public NbtByteTag(byte value) {
            this.type = 0x01;
            this.value = value;
        }

        public override byte[] Encode()
        {
            return new byte[] { value };
        }
    }
}
