using Snow.Entities;
using Snow.Formats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Events.Arguments
{
    public class OnRightClickBlockArgs
    {
        //connection.GetPlayer(), location, face, hand, insideBlock

        public Player player;
        public Position position;
        public int face;

        public bool insideblock;

        public OnRightClickBlockArgs(Player player, Position position, int face, bool insideblock)
        {
            this.player = player;
            this.position = position;
            this.face = face;
            this.insideblock = insideblock;
        }
    }
}
