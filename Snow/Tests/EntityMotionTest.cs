using Snow.Level;
using Snow.Level.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Tests
{
    internal class EntityMotionTest
    {
        World world;

        EntityChicken[] entityChickens = new EntityChicken[5];

        public EntityMotionTest(World world) {
            this.world = world;

            for (int x = 0; x < entityChickens.Length; x++)
            {
                EntityChicken chicken = new EntityChicken();
                world.SpawnEntity(chicken);
                chicken.Teleport(x, 0, 0);

                entityChickens[x] = chicken;   
            }
        }

        public void Tick(int tick)
        {
            for(int i = 0; i < entityChickens.Length; i++) { 
                double y = Math.Cos((double) tick / 80d);
                entityChickens[i].Teleport(i, y, 0);
            }
        }
    }
}
