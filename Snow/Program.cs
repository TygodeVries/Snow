using Snow.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Snow
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ServerboundPacketMappings.Load();

            MinecraftServer minecraftServer = new MinecraftServer(4041);
            Thread thread = new Thread(minecraftServer.Start);
            thread.Start();


            Console.ReadLine();
        }
    }
}
