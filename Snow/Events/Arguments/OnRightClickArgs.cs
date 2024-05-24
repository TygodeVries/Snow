using Snow.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Events.Arguments
{
    public class OnRightClickArgs
    {
        public Player player;

        public OnRightClickArgs(Player player)
        {
            this.player = player;
        }
    }
}
