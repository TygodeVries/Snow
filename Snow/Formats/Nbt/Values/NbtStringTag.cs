using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Formats.Nbt.Values
{
    internal class NbtStringTag : NbtTag
    {
        string value;

        public NbtStringTag(string value)
        {
            this.value = value;
            this.type = 0x08;
        }

        public override byte[] Encode()
        {
            ushort lenght = (ushort) value.Length;

            byte[] LB = BitConverter.GetBytes(lenght);

            if(BitConverter.IsLittleEndian)
            {
                LB = LB.Reverse().ToArray();
            }


            byte[] data = Encoding.UTF8.GetBytes(value);

            return LB.Concat(data).ToArray();
        }
    }
}
