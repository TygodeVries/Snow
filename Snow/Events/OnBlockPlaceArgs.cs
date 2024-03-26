using Snow.Entities;
using Snow.Formats;
using Snow.Levels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Events
{
    public class OnBlockPlaceArgs
    {
        public Player player;
        public Position position;

        public BlockType type;

        public OnBlockPlaceArgs(Player player, Position position, BlockType type)
        {
            this.player = player;
            this.position = position;
            this.type = type;
        }
    }
}
