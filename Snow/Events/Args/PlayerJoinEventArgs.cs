using Snow.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Events.Args
{
    public class PlayerJoinEventArgs
    {
        public PlayerJoinEventArgs(Player player)
        {
            this.player = player;
        }

        private Player player;
        public Player GetPlayer() { return player; }
    }
}
