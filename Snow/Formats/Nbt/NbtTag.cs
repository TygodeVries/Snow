using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Formats.Nbt
{
    internal abstract class NbtTag
    {
        public byte type;

        public abstract byte[] Encode();
    }
}
