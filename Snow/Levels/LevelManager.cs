using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Text.Json;
namespace Snow.Levels
{
    public class LevelManager
    {
        private static Dictionary<string, Level> levels = new Dictionary<string, Level>();

        public static void LoadAllLevels()
        {
            foreach (string filename in Directory.GetDirectories("Levels"))
            {
                LoadLevel(filename);
            }
        }

        public static void LoadLevel(string folder)
        {
            try
            {
                Log.Send($"[Level] Loading level at {folder}.");
                Level level = new Level();

                string executableDirectory = Environment.CurrentDirectory;
                string levelDirectory = $"{executableDirectory}/{folder}";
                string levelFileData = File.ReadAllText($"{levelDirectory}/level.json");
                JsonDocument levelData = JsonDocument.Parse(levelFileData);

                string name = levelData.RootElement.GetProperty("name").GetString();
                level.SetName(name);

                levels.Add(name, level);

                Log.Send($"[Level] Loaded level named '{name}'.");
            } catch (Exception ex)
            {
                Log.Err($"[Level] Failed to load level at {folder} because {ex}");
            }
        }

        public static Level GetLevel(string name)
        {
            return levels[name];
        }
    }
}
