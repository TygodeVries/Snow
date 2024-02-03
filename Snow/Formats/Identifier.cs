using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Formats
{
    internal class Identifier
    {
        public string nameSpace;
        public string value;

        public string GetString()
        {
            return $"{nameSpace}:{value}".Replace("\u0000", string.Empty);
        }

        public Identifier(string space, string value)
        {
            this.nameSpace = space;
            this.value = value;
        }
    }
}
