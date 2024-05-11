using Snow.Commands.Default;
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

        public void RegisterCommand(string command, Command instance)
        {
            commands.Add(command.ToLower(), instance);
        }

        public void UnregisterCommand(string command)
        {
            commands.Remove(command.ToLower());
        }

        public void Execute(string command, string[] arguments)
        {
            Command instance = commands[command.ToLower()];
            instance.Execute(null, arguments);
        }

        public void Execute(string command, string[] arguments, Player player)
        {
            if (!commands.ContainsKey(command))
            {
                player.SendSystemMessage(new Formats.TextComponent($"Unkown command '{command}'."));
                return;
            }

            Command instance = commands[command.ToLower()];
            instance.Execute(player, arguments);
        }

        public string[] GetSuggestionFor(string current)
        {
            return new string[] { "#TODO" };
        }

        internal void RegisterBuildIn()
        {
            RegisterCommand("gamemode", new GamemodeCommand());
            RegisterCommand("give", new GiveCommand());
            RegisterCommand("stop", new StopCommand());
        }
    }
}
