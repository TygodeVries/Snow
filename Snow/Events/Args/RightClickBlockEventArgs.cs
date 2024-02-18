using Snow.Entities;
using Snow.Formats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Events.Args
{
    public class RightClickBlockEventArgs
    {
        private Position clickedBlock;
        public Position GetClickedBlock() { return clickedBlock; }

        private int clickedface;
        public int GetClickedFace()
        {
            return clickedface;
        }

        private Player player;
        public Player GetPlayer() { return player; }

        private int hand;
        public int GetHand() { return hand; }

        public RightClickBlockEventArgs(Player player, Position clickedBlock, int clickedface, int hand)
        {
            this.player = player;
            this.clickedBlock = clickedBlock;
            this.hand = hand;
            this.clickedface = clickedface;
        }
    }
}
