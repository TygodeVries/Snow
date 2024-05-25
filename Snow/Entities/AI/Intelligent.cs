using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Entities.AI
{
    public abstract class Intelligent : Entity
    {
        public virtual Goal FindGoal()
        {
            return null;
        }
    }
}
