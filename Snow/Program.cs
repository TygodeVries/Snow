using System;
using System.Threading;
using Snow.Addons;
using Snow.Level;
using Snow.Network.Mappings;
namespace Snow
{
    internal class Program
    {
        private static LevelManager levelManager = new LevelManager();

        static void Main(string[] args)
        {
            MappingsManager.Load();
            levelManager.LoadLevels();


            for (int i = 0; i < 10; i++)
            {
                Lobby lobby = new Lobby(4000 + i);
                lobby.Start();
            }
            Console.ReadLine();
        }
    }
}
