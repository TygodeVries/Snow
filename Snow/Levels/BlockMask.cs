using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Levels
{
    /// <summary>
    /// A block mask is a mask that overwrites the blocks on the static level.
    /// These masks are ment to be used for players placing blocks during minigames.
    /// </summary>
    public class BlockMask
    {
        private Dictionary<(int x, int y, int z), BlockType> blocks = new Dictionary<(int x, int y, int z), BlockType>();

        public BlockType? GetBlockAt(int x, int y, int z)
        {
            if(!blocks.ContainsKey((x, y, z)))
            {
                return null;
            }

            return blocks[(x, y, z)];
        }

        public void SetBlock(BlockType type, int x, int y, int z)
        {
            blocks[(x,y, z)] = type;
        }
    }
}
