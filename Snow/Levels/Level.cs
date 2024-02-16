using Snow.Formats;
using Snow.Levels;
using Snow.Network;
using Snow.Network.Packets.Play.Clientbound;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Levels
{
    /// <summary>
    /// A level is a static world that multiple servers can read from at the same time.
    /// A level is  read only.
    /// </summary>
    public class Level
    {
        public int SectionsPerColumn = 24;
        Dictionary<(int x, int z), Chunk> chunks = new Dictionary<(int x, int z), Chunk>();

        public Chunk GetChunkAt(int x, int z)
        {
            return chunks[(x, z)];
        }
    }
}
