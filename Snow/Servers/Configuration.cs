using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Snow.Servers
{


    public class Configuration
    {
        private JsonDocument configFile;


        /// <summary>
        /// If template is 'null' then the file will not be filed in.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="template"></param>
        public Configuration(string path, string template)
        {
            if(template != null && !File.Exists(path))
            {
                TextWriter stream = File.CreateText(path);
                stream.Write(File.ReadAllText(template));
                stream.Close();
            }
            string serverConfigFile = File.ReadAllText(path);
            configFile = JsonDocument.Parse(serverConfigFile);
        }

        public string[] GetStringArray(string field)
        {
            JsonElement element = configFile.RootElement.GetProperty(field);
            var a = element.EnumerateArray();
            string[] strings = new string[element.GetArrayLength()];

            int indx = 0;
            foreach( var item in a ) {
                strings[indx++] = (string)item.GetString();
            }

            return strings;
        }

        public string GetString(string field)
        {
            return configFile.RootElement.GetProperty(field).GetString();
        }

        public int GetInt(string field)
        {
            return configFile.RootElement.GetProperty(field).GetInt32();
        }

        public double GetDouble(string field)
        {
            return configFile.RootElement.GetProperty(field).GetDouble();
        }
    }
}
