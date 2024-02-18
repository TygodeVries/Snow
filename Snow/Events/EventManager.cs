using Snow.Commands;
using Snow.Events.Args;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Events
{
    public class EventManager
    {
        public event EventHandler<RightClickBlockEventArgs> RightClickBlock;
        public void ExecuteRightClickBlock(RightClickBlockEventArgs eventArgs)
        {
            if (RightClickBlock != null)
                RightClickBlock.Invoke(this, eventArgs);
        }

        public event EventHandler<BlockPlaceEventArgs> BlockPlace;
        public void ExecuteBlockPlace(BlockPlaceEventArgs eventArgs)
        {
            if(BlockPlace != null)
                BlockPlace.Invoke(this, eventArgs);
        }

        public event EventHandler<PlayerJoinEventArgs> PlayerJoin;
        public void ExecutePlayerJoin(PlayerJoinEventArgs eventArgs)
        {
            if(PlayerJoin != null)
                PlayerJoin.Invoke(this, eventArgs);
        }
    }
}
