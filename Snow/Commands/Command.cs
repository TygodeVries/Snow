using Snow.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Commands
{
    public class Command
    {
        public virtual void Execute(Player player, string[] arguments)
        {
            
        }
    }

    public class CommandArgs
    {
        private Player player;
        public Player GetPlayer()
        {
            return player;
        }

        public CommandArgs(Player player)
        {
            this.player = player;
        }
    }
}
