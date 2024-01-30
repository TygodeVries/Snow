using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Formats.Nbt.Values
{
    internal class NbtIntTag : NbtTag
    {
        int value;

        public NbtIntTag(int value)
        {
            this.value = value;
            this.type = 0x03;
        }

        public override byte[] Encode()
        {
            byte[] bytes = BitConverter.GetBytes(value);
            
            if(BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytes);
            }

            return bytes;
        }
    }
}
