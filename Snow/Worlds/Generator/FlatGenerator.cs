using Snow.Formats;
using Snow.Levels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Worlds.Generator
{
    internal class FlatGenerator : WorldGenerator
    {
        public override void Generate(Chunk chunk)
        {
            Random rng = new Random();

            for(int bx = 0; bx < 16; bx++)
            {
                for (int bz = 0; bz < 16; bz++)
                {
                    chunk.SetBlockAt(new Position(bx, 70, bz), BlockType.COARSE_DIRT);
                }
            }
        }
    }
}
