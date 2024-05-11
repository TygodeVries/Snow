using Snow.Entities;
using Snow.Formats;
using Snow.Worlds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Events.Arguments
{
    public class OnEntityMoveArgs
    {
        public World toWorld;
        public Vector3 to;
        public Entity entity;


        public OnEntityMoveArgs(World toWorld, Vector3 to, Entity entity)
        {
            this.to = to;
            this.toWorld = toWorld;
            this.entity = entity;
        }
    }
}
