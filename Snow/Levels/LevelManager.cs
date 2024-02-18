using Snow.Formats;
using Snow.Formats.Nbt;
using Snow.Network.Packets.Play.Clientbound;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
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

        public static void CreateDefaults()
        {
            byte[] bytes = new byte[0];
            for (int i = 0; i < 24; i++)
            {
                byte[] blockCount = BitConverter.GetBytes((short)999);

                if (BitConverter.IsLittleEndian)
                    Array.Reverse(blockCount);

                bytes = bytes.Concat(blockCount).ToArray();


                int block = 0;
                bytes = bytes.Concat(new byte[] { 0 }).Concat(VarInt.ToByteArray((uint)block)).Concat(VarInt.ToByteArray(0)).ToArray();

                // biome
                bytes = bytes.Concat(new byte[] { 0 }).Concat(VarInt.ToByteArray((uint)block)).Concat(VarInt.ToByteArray(0)).ToArray();
            }


            NbtCompoundTag heightmap = new NbtCompoundTag();

            NbtLongArrayTag MOTION_BLOCKING = new NbtLongArrayTag();
            NbtLongArrayTag WORLD_SURFACE = new NbtLongArrayTag();
            MOTION_BLOCKING.values = new long[37];
            WORLD_SURFACE.values = new long[37];
            heightmap.AddField("MOTION_BLOCKING", MOTION_BLOCKING);
            heightmap.AddField("WORLD_SURFACE", WORLD_SURFACE);


            defaultChunkPacket = new ChunkDataAndUpdateLight(0, 0, heightmap, bytes);
        }

        static ChunkDataAndUpdateLight defaultChunkPacket;

        internal static ChunkDataAndUpdateLight GetFallbackChunkPacket(int x, int z)
        {
            defaultChunkPacket.x = x;
            defaultChunkPacket.z = z;

            return defaultChunkPacket;
        }

        public static void LoadLevel(string folder)
        {
            try
            {
                Log.Send($"[Level] Loading level at {folder}.");
                Level level = new Level(folder);

                string executableDirectory = Environment.CurrentDirectory;
                string levelDirectory = $"{executableDirectory}/{folder}";
                string levelFileData = File.ReadAllText($"{levelDirectory}/level.json");
                JsonDocument levelData = JsonDocument.Parse(levelFileData);

                string name = levelData.RootElement.GetProperty("name").GetString();
                level.SetName(name);

                levels.Add(name, level);

                long start = GC.GetTotalMemory(true);
                level.Bake();
                long end = GC.GetTotalMemory(true);


                Log.Send($"[Level] Loaded level named '{name}'.");
            } catch (Exception ex)
            {
                Log.Err($"[Level] Failed to load level at {folder} because {ex}");
            }
        }

        public static Level GetLevel(string name)
        {
            if(!levels.ContainsKey(name))
            {
                Log.Err($"Could not find world named '{name}'");
            }
            return levels[name];
        }
    }
}
