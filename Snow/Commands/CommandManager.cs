using Snow.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Commands
{
    public class CommandManager
    {
        private Dictionary<string, Command> commands = new Dictionary<string, Command>();

        public Command CreateCommand(string command)
        {
            command = command.ToLower();
            Command cmd = new Command(command);
            commands.Add(command, cmd);
            return cmd;
        }

        public string[] GetSuggestions(string text, Player player)
        {
            List<string> strings = new List<string>();

            foreach(string cmd in commands.Keys)
            {
                if(cmd.StartsWith(text))
                {
                    strings.Add(cmd);
                }
            }

            return strings.ToArray();
;        }

        public void RunCommand(Player player, string command)
        {

            string baseCommand = command.Split(' ')[0].ToLower();

            if(!commands.ContainsKey(baseCommand))
            {
                Log.Err($"Command '{baseCommand} not found'");
                return;
            }

            commands[baseCommand].ExecuteAs(player);
        }
    }
}
