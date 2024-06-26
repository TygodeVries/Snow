﻿using System;
using System.Threading;
using Snow.Addons;
using Snow.Levels;
using Snow.Network.Mappings;
using Snow.Servers;
using Snow.Tests;
namespace Snow
{
    internal class Program
    {
        static void Main(string[] args)
        {
            MappingsManager.Load();

            Server server = new Server(4041, "Server");
            server.Start();

            Console.ReadLine();
        }
    }
}
