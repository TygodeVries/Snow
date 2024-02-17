using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.AccessControl;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Snow.Addons
{
    public class AddonManager
    {
        private Lobby lobby;
        public Lobby GetLobby()
        {
            return lobby;
        }

        public void LoadAllAddons()
        {
            foreach (string filename in Directory.GetDirectories("Addons"))
            {
                LoadAddon(filename);
            }
        }

        private List<Addon> addons = new List<Addon>();

        public void LoadAddon(string folder)
        {
            string name = null;

            try
            {
                Log.Send($"[Addons] Loading addon at {folder}.");
                string executableDirectory = Environment.CurrentDirectory;
                string addonDirectory = $"{executableDirectory}/{folder}";
                string addonFileData = File.ReadAllText($"{addonDirectory}/addon.json");

                JsonDocument addonData = JsonDocument.Parse(addonFileData);

                name = addonData.RootElement.GetProperty("details").GetProperty("name").GetString();
                string version = addonData.RootElement.GetProperty("details").GetProperty("version").GetString();
                string author = addonData.RootElement.GetProperty("details").GetProperty("author").GetString();
                string startingFileName = addonData.RootElement.GetProperty("code").GetProperty("file").GetString();
                string startingClassName = addonData.RootElement.GetProperty("code").GetProperty("class").GetString();

                Assembly assembly = Assembly.LoadFile($"{addonDirectory}/scripts/{startingFileName}");
                Type type = assembly.GetType(startingClassName);

                Addon addon = (Addon)Activator.CreateInstance(type);
                Log.Send($"[Addons] Loaded addon '{name}' version '{version}' by '{author}'.");
                addons.Add(addon);
                addon.Start();
            
            } catch(Exception e)
            {
                if (name == null)
                {
                    Log.Err($"Failed to load addon in folder {folder} because: \n" + e);
                }
                else
                {
                    Log.Err($"Failed to load addon {name} because: \n" + e);
                }
            }
        }

        public AddonManager(Lobby server)
        {

        }

        public void StopAll()
        {
            for(int i = 0; i < addons.Count; i++)
            {
                Addon addon = addons[i];
                addon.Stop();
            }
        }
    }
}
