using Snow.Formats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Worlds
{
    public class RaycastResult
    {
        public RaycastResult(Vector3 hitPoint) { this.hitPoint = hitPoint; }

        public Vector3 hitPoint;


    }
}
