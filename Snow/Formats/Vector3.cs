using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Formats
{
    public class Vector3
    {
        public double x;
        public double y;
        public double z;

        public Vector3(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public double GetMagnitude()
        {
            return Math.Sqrt(x * x + y * y + z * z);
        }

        public Vector3 Clone()
        {
            return new Vector3(x, y, z);
        }

        public Vector3 Normalized()
        {
            return (this.Clone() / GetMagnitude());
        }

        public static Vector3 operator /(Vector3 a, double b)
        {
            return new Vector3(a.x / b, a.y / b, a.z / b);
        }

        public static Vector3 operator +(Vector3 a, Vector3 b)
        {
            return new Vector3(a.x + b.x, a.y + b.y, a.z + b.z);
        }

        public static double Distance(Vector3 a, Vector3 b)
        {
            return Math.Sqrt(Math.Pow(b.x - a.x, 2) + Math.Pow(b.y - a.y, 2) + Math.Pow(b.z - a.z, 2));
        }
    }
}
