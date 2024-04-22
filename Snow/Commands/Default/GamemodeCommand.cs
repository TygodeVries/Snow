using Snow.Entities;
using Snow.Formats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Commands.Default
{
    public class GamemodeCommand : Command
    {
        public override void Execute(Player player, string arguments)
        {
            try
            {
                Gamemode gamemode = (Gamemode)Enum.Parse(typeof(Gamemode), arguments.ToUpper());
                player.SetGamemode(gamemode);

                TextComponent textComponent = new TextComponent($"Updated gamemode to {gamemode.ToString().ToLower()}");
                player.SendSystemMessage(textComponent);

            }
            catch {
                TextComponent textComponent = new TextComponent($"Gamemode '{arguments}' is unknown.");
                player.SendSystemMessage(textComponent);
            }
        }
    }
}
