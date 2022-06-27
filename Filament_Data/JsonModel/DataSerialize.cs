using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Filament_Data.JsonModel
{
    public class JsonTransfer
    {
        public static void SerializeObject(string fileName, object target)
        {
            if(System.IO.File.Exists(fileName))
                System.IO.File.Delete(fileName);

            using (System.IO.FileStream streamWriter =System.IO.File.Create(fileName))
            {
                //using (JsonTextWriter jsonTextWriter = new JsonTextWriter(streamWriter))
                //{
                    JsonSerializer.Serialize(streamWriter, target);
                //}
            }
        }
        public static T DeserializeFileContents<T>(string fileName)
        {
            if (System.IO.File.Exists(fileName))
                using (System.IO.StreamReader stream = new System.IO.StreamReader(fileName))
                {
                    //using (JsonTextReader reader = new JsonTextReader(stream))
                    //{
                        return JsonSerializer.Deserialize<T>(stream.ReadToEnd());

                    //}
                }
            else
                return default(T);
        }

    }
}
