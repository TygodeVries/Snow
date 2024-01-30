using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Formats.Nbt
{
    internal class NbtLongArrayTag : NbtTag
    {
        public NbtLongArrayTag()
        {
            this.type = 0xc;
        }

        public long[] values;

        public override byte[] Encode()
        {
            byte[] bytes = new byte[0];

            byte[] le = BitConverter.GetBytes((uint) values.Length);
            if (BitConverter.IsLittleEndian) Array.Reverse(le);

            bytes = bytes.Concat(le).ToArray();

            foreach(long l in values)
            {
                byte[] va = BitConverter.GetBytes(l);
                if (BitConverter.IsLittleEndian) Array.Reverse(va);

                bytes = bytes.Concat(va).ToArray();
            }

            return bytes;
        }
    }
}
