using Snow.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Servers.Registries
{
    public abstract class Registry
    {
        public abstract void SendPacketToConnection(Connection connection);
    }
}
