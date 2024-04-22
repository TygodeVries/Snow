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
            for(int bx = 0; bx < 16; bx++)
            {
                for (int bz = 0; bz < 16; bz++)
                {
                    int worldx = bx + (chunk.x * 16);
                    int worldz = bz + (chunk.z * 16);

                    float height = 100;

                    chunk.SetBlockAt(new Position(bx, (int)height, bz), BlockType.COARSE_DIRT);
                }
            }
        }
    }
}
