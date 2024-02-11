using Snow.Formats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Level
{
    public abstract class ChunkSection
    {

        public abstract byte[] GetBlocks();
        public abstract byte[] GetBiomes();
    }
}
