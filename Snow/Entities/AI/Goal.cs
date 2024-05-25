using Snow.Formats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Entities.AI
{
    public abstract class Goal
    {
        public abstract Vector3 GetGoalLocation();

        public virtual void GoalReached()
        {

        }
    }
}
