using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Filament_Data.JsonModel
{
    public class JsonTransfer
    {
        public static void SerializeObject(string fileName, object target)
        {
            using (System.IO.StreamWriter streamWriter = new System.IO.StreamWriter(fileName))
            {
                using (JsonTextWriter jsonTextWriter = new JsonTextWriter(streamWriter))
                {
                    JsonSerializer.Create().Serialize(jsonTextWriter, target);
                }
            }
        }
        public static T DeserializeFileContents<T>(string fileName)
        {
            if (System.IO.File.Exists(fileName))
                using (System.IO.StreamReader stream = new System.IO.StreamReader(fileName))
                {
                    using (JsonTextReader reader = new JsonTextReader(stream))
                    {
                        return JsonSerializer.Create().Deserialize<T>(reader);

                    }
                }
            else
                return default(T);
        }

    }
}
