using Snow.Formats;
using Snow.Level;
using Snow.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Entities
{
    internal class Entity
    {
        public int Id { get; set; }
        public UUID uuid;

        public double x;
        public double y;
        public double z;

        public int type;

        public void Teleport(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public World world;

    }
}
