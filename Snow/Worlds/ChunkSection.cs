using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Worlds
{
    public abstract class ChunkSection
    {
        public abstract short GetBlockAt(int x, int y, int z);
    }
}
