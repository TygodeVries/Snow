using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Snow.Servers
{
    internal class Configuration
    {
        public static void Reload()
        {
            string executableDirectory = Environment.CurrentDirectory;
            string levelFileData = File.ReadAllText($"{executableDirectory}/server.json");
            JsonDocument levelData = JsonDocument.Parse(levelFileData);

            defaultLevelName = levelData.RootElement.GetProperty("default_level").GetString();
        }


        static string defaultLevelName = "world";
        public static string GetDefaultLevelName()
        {
            return defaultLevelName;
        }
    }
}
