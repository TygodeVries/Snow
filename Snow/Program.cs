using Snow.Network.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Snow
{
    internal class Program
    {
        static void Main(string[] args)
        {
            TcpListener listener = new TcpListener(4030);
            listener.Start();


            while (true)
            {
                PlayerConnection playerConnection = new PlayerConnection(listener.AcceptTcpClient());

                playerConnection.SendConnectionPackets();
                Console.WriteLine("Connected player!");
            }
        }
    }
}
