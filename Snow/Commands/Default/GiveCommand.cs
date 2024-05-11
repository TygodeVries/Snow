using Snow.Entities;
using Snow.Formats;
using Snow.Items;
using Snow.Levels;
using Snow.Servers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Commands.Default
{
    internal class GiveCommand : Command
    {
        public override void Execute(Player player, string arguments)
        {
            try
            {
                Server server = player.GetConnection().GetServer();
                ItemManager itemManager = server.GetItemManager();

                player.GetInventory().AddItem(new ItemStack(itemManager.GetNamespace(arguments)));

                TextComponent textComponent = new TextComponent($"Gave you an {arguments}");
                player.SendSystemMessage(textComponent);

            }
            catch
            {
                TextComponent textComponent = new TextComponent($"Gamemode '{arguments}' is unknown.");
                player.SendSystemMessage(textComponent);
            }
        }
    }
}
