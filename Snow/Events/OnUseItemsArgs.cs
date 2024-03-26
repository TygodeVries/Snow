using Snow.Entities;
using Snow.Formats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Events
{
    public class OnUseItemsArgs
    {
        //connection.GetPlayer(), location, face, hand, insideBlock

        public Player player;
        public Position position;
        public int face;

        public bool insideblock;

        public OnUseItemsArgs(Player player, Position position, int face, bool insideblock)
        {
            this.player = player;
            this.position = position;
            this.face = face;
            this.insideblock = insideblock;
        }
    }
}
