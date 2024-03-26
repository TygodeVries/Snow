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
        public override Chunk Generate(World world, int x, int z)
        {
            Chunk chunk = new Chunk(world, x, z);

            for(int bx = 0; bx < 16; bx++)
            {
                for (int bz = 0; bz < 16; bz++)
                {
                    chunk.SetBlockAt(new Position(bx, 80, bz), BlockType.STONE);
                    chunk.SetBlockAt(new Position(bx, 81, bz), BlockType.STONE);
                    chunk.SetBlockAt(new Position(bx, 82, bz), BlockType.STONE);
                    
                    chunk.SetBlockAt(new Position(bx, 83, bz), BlockType.DIRT);
                    chunk.SetBlockAt(new Position(bx, 84, bz), BlockType.DIRT);

                    chunk.SetBlockAt(new Position(bx, 85, bz), BlockType.OAK_PLANKS);
                }
            }

            return chunk;
        }
    }
}
