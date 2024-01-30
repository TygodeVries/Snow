using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Level
{
    internal class World
    {
        public List<Chunk> chunks = new List<Chunk>();
        public int SectionsPerColumn = 24;
    }
}
