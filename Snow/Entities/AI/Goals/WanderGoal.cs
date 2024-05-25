using Snow.Formats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Entities.AI.Goals
{
    public class WanderGoal : Goal
    {
        Entity entity;
        int distance;

        public WanderGoal(Entity entity, int distance)
        {
            this.entity = entity;
            this.distance = distance;
        }

        public override Vector3 GetGoalLocation()
        {
            Random random = new Random();

            Vector3 goal = new Vector3(entity.GetLocation().x + random.Next(-distance, distance), entity.GetLocation().y + random.Next(-distance, distance), entity.GetLocation().z + random.Next(-distance, distance));
            return goal;
        }
    }
}
