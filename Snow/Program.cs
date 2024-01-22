using Snow.Network.Entity;
using Snow.Network.Packets.Play.Serverbound;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow
{
    internal class Program
    {
        static void Main(string[] args)
        {
            PlayerConnection playerConnection = new PlayerConnection();

            playerConnection.SendConnectionPackets();
        }
    }
}
