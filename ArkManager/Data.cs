using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArkManager.CustomType;    // Folder containing our custom types
using Newtonsoft.Json;
using System.IO;

namespace ArkManager
{
    public static class Data
    {

        // CHANGED DICTONARY FOR BLUEPRINTS TO LIST AND FIX JSON READER CAUSE OF IT BACK TO RETURN LIST<BLUEPRINT>


        public static readonly Dictionary<string,Blueprint> Blueprints = ReadJsonFiles(@"DefaultArkBlueprints.json");
        public static readonly List<Blueprint> UserCreatedBlueprints = ReadJsonFiles(@"CustomBps.json");
        public static Blueprint userBlueprint;

        /// <summary>
        ///     Reading from a Json Files to create the program's list of blueprints
        /// </summary>
        static Dictionary<string, Blueprint> ReadJsonFiles(string _path)
        {
            // Side note, the program looks inside bin\debug for the files by default...
            List<Blueprint> blueprints = JsonConvert.DeserializeObject<List<Blueprint>>(File.ReadAllText());
            Dictionary<string, Blueprint> convertBlueprints = new Dictionary<string, Blueprint>(0);

            foreach (var bps in blueprints)
            {
                convertBlueprints.Add(bps.BlueprintType, bps);
            }

            return convertBlueprints;
        }
    }
}
