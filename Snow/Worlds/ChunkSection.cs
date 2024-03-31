using Snow.Levels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Worlds
{
    public abstract class ChunkSection
    {
        public abstract BlockType Get(int x, int y, int z);
    }

    public class SolidChunkSection : ChunkSection
    {
        BlockType type;

        public SolidChunkSection(BlockType type)
        {
            this.type = type;
        }

        public override BlockType Get(int x, int y, int z)
        {
            return type;
        }
    }

    public class DetailedChunkSection : ChunkSection
    {
        BlockType[,,] types;

        public DetailedChunkSection()
        {
            types = new BlockType[16, 16, 16];
        }

        public override BlockType Get(int x, int y, int z)
        {
            return types[x, y, z];
        }

        public void Set(BlockType type, int x, int y, int z)
        {
            types[x, y, z] = type;
        }

        public ChunkSection Optimize()
        {
            if(same())
            {
                return new SolidChunkSection(types[0, 0, 0]);
            }

            return this;
        }

        private bool same()
        {

            Type firstType = types[0, 0, 0].GetType();

            for (int i = 0; i < types.GetLength(0); i++)
            {
                for (int j = 0; j < types.GetLength(1); j++)
                {
                    for (int k = 0; k < types.GetLength(2); k++)
                    {
                        if (types[i, j, k] != null && types[i, j, k].GetType() != firstType)
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }
    }

}
