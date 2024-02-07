using System;
using System.Threading;
using Snow.Network.Mappings;
namespace Snow
{
    internal class Program
    {
        static void Main(string[] args)
        {

            MappingsManager.Load();

            MinecraftServer minecraftServer = new MinecraftServer(4041);
            Thread thread = new Thread(minecraftServer.Start);
            thread.Start();


            Console.ReadLine();
        }
    }
}
