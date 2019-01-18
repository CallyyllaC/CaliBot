using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace CaliBotCore.Config
{
    class Json
    {
        //Creates a Json file with the given object at a specific location
        public static void CreateJson(string name, string location, Object yourClassObject)
        {
            if (yourClassObject == null)
            {
                return;
            }
            string output = JsonConvert.SerializeObject(yourClassObject);
            File.WriteAllText($"{location}\\{name}.json", output);
        }

        //Creates an object from a stored Json file at the given location
        public static T CreateObject<T>(string jsonFilename)
        {
            string jsonFile = File.ReadAllText(jsonFilename);
            T returnObject = default(T);
            returnObject = JsonConvert.DeserializeObject<T>(jsonFile);
            return returnObject;
        }
    }
}
