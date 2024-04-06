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
                    int worldX = chunk.x * 16 + bx;
                    int worldZ = chunk.z * 16 + bz;

                    double xHillsLenght = 7;
                    double zHillsLenght = 7;

                    double ampl = 7;
                    double offset = 100;

                    double height = Math.Sin(worldX / xHillsLenght) * Math.Sin(worldZ / zHillsLenght) * ampl;
                    height += offset;

                    for(int y = 0; y < 200; y++)
                    {
                        if(y == 0)
                        {
                            chunk.SetBlockAt(new Position(bx, y, bz), BlockType.DIORITE);
                        }
                        else if (y < height)
                        {
                            chunk.SetBlockAt(new Position(bx, y, bz), BlockType.STONE);
                        }
                        else if (y < height + 3)
                        {
                            chunk.SetBlockAt(new Position(bx, y, bz), BlockType.DIRT);
                        }
                        else if (y < height + 4)
                        {
                            chunk.SetBlockAt(new Position(bx, y, bz), BlockType.GRASS_BLOCK);
                        }
                    }
                    
                }
            }
        }
    }
}
