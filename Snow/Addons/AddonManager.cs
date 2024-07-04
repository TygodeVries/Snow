using Snow.Items;
using Snow.Servers;
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
        private Server server;
        public Server GetServer()
        {
            return server;
        }

        public void LoadAllAddons()
        {
            string addonFolder = $"{server.GetWorkPath()}/Addons";
            Directory.CreateDirectory(addonFolder);

            foreach (string filename in Directory.GetDirectories(addonFolder))
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
                string addonDirectory = $"{Environment.CurrentDirectory}/{folder}";
                string addonFileData = File.ReadAllText($"{addonDirectory}/addon.json");

                JsonDocument addonData = JsonDocument.Parse(addonFileData);

                name = addonData.RootElement.GetProperty("details").GetProperty("name").GetString();
                string version = addonData.RootElement.GetProperty("details").GetProperty("version").GetString();
                string author = addonData.RootElement.GetProperty("details").GetProperty("author").GetString();
                string startingFileName = addonData.RootElement.GetProperty("code").GetProperty("file").GetString();
                string startingClassName = addonData.RootElement.GetProperty("code").GetProperty("class").GetString();


                if(!File.Exists($"{addonDirectory}/{startingFileName}"))
                {
                    Log.Err($"DLL for addon {name} was not found.");
                    return;
                }
                Assembly assembly = Assembly.LoadFile($"{addonDirectory}/{startingFileName}");
                Type type = assembly.GetType(startingClassName);

                Addon addon = (Addon)Activator.CreateInstance(type);
                addon.dataPath = folder;
                addon.assembly = assembly;
                addon.SetAddonManager(this);
                addons.Add(addon);
                addon.Start();

                LoadAddonItems(addon);

                Log.Send($"[Addons] Loaded addon '{name}' version '{version}' by '{author}'.");
               
                
            
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

        public void LoadAddonItems(Addon addon)
        {
            string itemDataPath = addon.GetDataPath() + "/item";
            if (!Directory.Exists(itemDataPath))
            {
                return;
            }

            string[] files = Directory.GetFiles(itemDataPath);
            Log.Send($"Loading {files.Length} items...");

            foreach (string file in files)
            {
                try
                {
                    string addonFileData = File.ReadAllText(file);
                    JsonDocument addonData = JsonDocument.Parse(addonFileData);
                    string id = addonData.RootElement.GetProperty("id").GetString();

                    string material = addonData.RootElement.GetProperty("style").GetProperty("material").GetString();
                    string name = addonData.RootElement.GetProperty("style").GetProperty("name").GetString();
                    int modelData = addonData.RootElement.GetProperty("style").GetProperty("customModelData").GetInt32();

                    string behaviour = addonData.RootElement.GetProperty("code").GetProperty("behaviour").GetString();

                    string places = addonData.RootElement.GetProperty("places").GetString();

                    ItemBehaviour itemBehaviour = null;
                    if(behaviour != null)
                    {
                        Type type = addon.assembly.GetType(behaviour);
                        if(type == null)
                        {
                            throw new Exception("Could not find type " + behaviour);
                        }
                        itemBehaviour = (ItemBehaviour) Activator.CreateInstance(type);
                    }


                    if (places == null)
                    {
                        ItemType itemType = new ItemType((ItemMaterial)Enum.Parse(typeof(ItemMaterial), material), name, modelData, itemBehaviour);
                        GetServer().itemRegistry.RegisterItemType(id, itemType);
                    }
                } catch(Exception e)
                {
                    Log.Err("Could not load item in " + file + " for addon " + addon + " because: " + e);
                }
            }
        }

        

        public AddonManager(Server server)
        {
            this.server = server;
        }

        public void Tick()
        {
            foreach(Addon addon in addons)
            {
                addon.Update();
            }
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
