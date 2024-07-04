using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Formats.Nbt.Values
{
    public class NbtListTag : NbtTag
    {
        public List<NbtTag> tags = new List<NbtTag>();

        public NbtListTag()
        {
            this.type = 0x09;
        }

        public override byte[] Encode()
        {
            byte[] bytes = new byte[] { tags[0].type };

            byte[] lenght = BitConverter.GetBytes((uint)tags.Count);
            if (BitConverter.IsLittleEndian) Array.Reverse(lenght);

            bytes = bytes.Concat(lenght).ToArray();


            foreach (NbtTag tag in tags)
            {
                byte[] data = tag.Encode();
                bytes = bytes.Concat(data).ToArray();
            }

            return bytes;
        }
    }
}
