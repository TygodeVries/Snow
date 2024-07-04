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

        public override Vector3d GetGoalLocation()
        {
            Random random = new Random();

            Vector3d goal = new Vector3d(entity.GetLocation().x + random.Next(-distance, distance), entity.GetLocation().y + random.Next(-distance, distance), entity.GetLocation().z + random.Next(-distance, distance));
            return goal;
        }
    }
}
