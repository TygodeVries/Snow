using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Addons
{
    public abstract class Addon
    {
        public virtual void Start() { }

        public virtual void Stop() { }
    }
}
