using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Snow.Addons
{
    internal class AddonManager
    {
        public void Load()
        {
            string executableDirectory = Environment.CurrentDirectory;

            foreach (string filename in Directory.GetFiles("Addons"))
            {


                Assembly assembly = Assembly.LoadFile(executableDirectory + "/" + filename);
                Type type = assembly.GetType("SnowTestDLL.Main");

                Addon addon = (Addon) Activator.CreateInstance(type);
                addon.Start();
            }
        }
    }
}
