using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Formats.Nbt
{
    class NbtCompoundTagEntry
    {
        public NbtTag tag;
        public string name;

        public NbtCompoundTagEntry(string name, NbtTag tag)
        {
            this.tag = tag;
            this.name = name;
        }
    }
}
