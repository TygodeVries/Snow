using Snow.Network;
using Snow.Servers;
using Snow.Worlds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Tests
{
    public class ChunkPerformanceTest
    {
        public static void Test(Server server)
        {
            Log.Send("Started performance test");

            DateTime start = DateTime.Now;
            int chunks = 0;

            int count = 20;

            Log.Send($"Generating {count * count} chunks...");

            for (int x = 0; x < count; x++)
                for (int y = 0; y < count; y++)
                {
                    chunks++;
                    Chunk chunk = new Chunk(server.GetDefaultWorld(), x, y);
                    chunk.CreatePacket();
                }

            double totalTime = DateTime.Now.Subtract(start).TotalMilliseconds;

            Log.Send("Finished performance test! Processing results...");
            Log.Send($"took {totalTime / chunks}ms per chunk. Total time: {totalTime}ms");
            Console.ReadLine();
        }
    }
}
