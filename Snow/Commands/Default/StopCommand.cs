using Snow.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Commands.Default
{
    public class StopCommand : Command
    {
        public override void Execute(Player player, string[] arguments)
        {
            player.GetConnection().GetServer().Stop();
        }
    }
}
