using Snow.Entities;
using Snow.Formats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Items
{
    public abstract class ItemBehaviour
    {
        public virtual void OnRightClick(Player player)
        {

        }

        public virtual void OnRightClickBlock(Player player, Position position)
        {

        }
    }
}
