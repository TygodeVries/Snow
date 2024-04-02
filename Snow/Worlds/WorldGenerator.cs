using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Worlds
{
    public abstract class WorldGenerator
    {
        public abstract void Generate(Chunk chunk);
    }
}
