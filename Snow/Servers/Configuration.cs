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

        private string path;

        /// <summary>
        /// If template is 'null' then the file will not be filed in.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="template"></param>
        public Configuration(string path, string template)
        {
            this.path = path;
            if (template != null && !File.Exists(path))
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
            try
            {
                return configFile.RootElement.GetProperty(field).GetString();
            } catch(Exception e)
            {
                Log.Err("Could not load string field " + field + " from configuration at " + path + "\n" + e);
                return String.Empty;
            }
        }

        public int GetInt(string field)
        {
            try
            {
                return configFile.RootElement.GetProperty(field).GetInt32();
            }
            catch (Exception e)
            {
                Log.Err("Could not load int field " + field + " from configuration at " + path + "\n" + e);
                return -1;
            }
        }

        public double GetDouble(string field)
        {
            return configFile.RootElement.GetProperty(field).GetDouble();
        }
    }
}
