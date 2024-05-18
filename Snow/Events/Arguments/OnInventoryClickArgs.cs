using Snow.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Events.Arguments
{
    public class OnInventoryClickArgs
    {
        public Player player;

        public byte windowId;
        public int stateId;
        public short slot;
        public byte button;
        public int mode;

        public OnInventoryClickArgs(Player player, byte windowId, int stateId, short slot, byte button, int mode)
        {
            this.player = player;
            this.windowId = windowId;
            this.stateId = stateId;
            this.slot = slot;
            this.button = button;
            this.mode = mode;
        }
    }
}
