using Snow.Levels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Worlds.Blocks
{
    public abstract class ProgrammableBlock
    {
        public BlockType blockType;

        /// <summary>
        /// Runs every tick aslong as the chunk is loaded.
        /// Not recommended for big operations as every block will run this code every tick.
        /// </summary>
        public virtual void Tick()
        {

        }
    }
}
