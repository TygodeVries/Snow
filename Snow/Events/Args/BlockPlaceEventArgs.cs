using Snow.Entities;
using Snow.Formats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Events.Args
{
    public class BlockPlaceEventArgs
    {
        Player player;
        public Player GetPlayer() { return player; }

        Position blockPosition;
        public Position GetBlockPosition() { return blockPosition; }

        public BlockPlaceEventArgs(Player player, Position blockPosition)
        {
            this.player = player;
            this.blockPosition = blockPosition;
        }
    }
}
