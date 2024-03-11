using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Worlds.Generator
{
    public class VoidGenerator : WorldGenerator
    {
        public override Chunk Generate(World world,int x, int z)
        {
            Chunk chunk = new Chunk(world, x, z);
            return chunk;
        }
    }
}
