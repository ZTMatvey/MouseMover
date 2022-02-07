using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MouseMover
{
    public sealed class Rules
    {
        public readonly int MinX;
        public readonly int MinY;
        public readonly int MaxX;
        public readonly int MaxY;
        public readonly int MinDelay;
        public readonly int MaxDelay;
        private static Rules Instance { get; set; }

        public Rules(int minX, int maxX, int minY, int maxY, int minDelay, int maxDelay)
        {
            MaxDelay = maxDelay;
            MinDelay = minDelay;
            MaxY = maxY;
            MaxX = maxX;
            MinY = minY;
            MinX = minX;
        }
        public static Rules GetRules()
        {
            if (Instance == null)
            {
                try
                {
                    var rules = LoadRules();
                    Instance = rules;
                }
                catch
                {
                    var rules = new Rules(0, 1600, 0, 900, 500, 1000);
                    SaveRules(rules);
                }
            }
            return Instance;
        }
        private static Rules LoadRules()
        {
            var dataPath = GetDataPath();
            if (!File.Exists(dataPath))
                throw new FileNotFoundException();
            using (var streamReader = new StreamReader(dataPath))
            {
                var json = streamReader.ReadToEnd();
                var data = JsonConvert.DeserializeObject<Rules>(json);
                return data;
            }
        }
        public static void SaveRules(Rules rules)
        {
            var dataPath = GetDataPath();
            var folder = Path.GetDirectoryName(dataPath);
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);
            using (var streamWriter= new StreamWriter(dataPath, false))
            {
                var json = JsonConvert.SerializeObject(rules);
                streamWriter.Write(json);
                Instance = rules;
            }
        }
        private static string GetDataPath()
        {
            var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            return $@"{appDataPath}\Dorset\MouseMover\Rules.json";
        }
    }
}
