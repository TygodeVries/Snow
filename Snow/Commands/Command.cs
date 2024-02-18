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
        private string command;
        public string GetCommand()
        {
            return command;
        }

        public Command(string command)
        {
            this.command = command;
        }

        public void Execute()
        {
            OnExecute.Invoke(this, new CommandArgs(null));
        }

        public void ExecuteAs(Player player)
        {
            OnExecute.Invoke(this, new CommandArgs(player));
        }

        public event EventHandler<CommandArgs> OnExecute;
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
