using System;
using System.Threading;
using Snow.Addons;
using Snow.Levels;
using Snow.Network.Mappings;
namespace Snow
{
    internal class Program
    {
        static void Main(string[] args)
        {
            MappingsManager.Load();
            LevelManager.LoadAllLevels();

            Lobby lobby = new Lobby(25565);

            Console.ReadLine();
        }
    }
}
