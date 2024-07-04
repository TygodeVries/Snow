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
        public override void Execute(Player player, string[] arguments)
        {
            try
            {
                Server server = player.GetConnection().GetServer();
                ItemRegistry nameSpace = server.itemRegistry;

                player.GetInventory().AddItem(new ItemStack(nameSpace.GetItemType(arguments[0])));

                TextComponent textComponent = new TextComponent($"Gave you an {arguments[0]}");
                player.SendSystemMessage(textComponent);

            }
            catch
            {
                TextComponent textComponent = new TextComponent($"Item '{arguments[0]}' is unknown.");
                player.SendSystemMessage(textComponent);
            }
        }
    }
}
