using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Formats
{
    public class Vector3d
    {
        public double x;
        public double y;
        public double z;

        public Vector3d(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public double GetMagnitude()
        {
            return Math.Sqrt(x * x + y * y + z * z);
        }

        public Vector3d Clone()
        {
            return new Vector3d(x, y, z);
        }

        public Vector3d Normalized()
        {
            return (this.Clone() / GetMagnitude());
        }

        public static Vector3d operator /(Vector3d a, double b)
        {
            return new Vector3d(a.x / b, a.y / b, a.z / b);
        }

        public static Vector3d operator +(Vector3d a, Vector3d b)
        {
            return new Vector3d(a.x + b.x, a.y + b.y, a.z + b.z);
        }

        public static double Distance(Vector3d a, Vector3d b)
        {
            return Math.Sqrt(Math.Pow(b.x - a.x, 2) + Math.Pow(b.y - a.y, 2) + Math.Pow(b.z - a.z, 2));
        }
    }
}
