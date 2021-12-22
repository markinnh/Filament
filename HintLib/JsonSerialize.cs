using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace HintLib
{
    public class JsonSerialize
    {
        internal static TSerialize? Deserialize<TSerialize>(string fileName) where TSerialize : class, new()
        {
            TSerialize? result = default(TSerialize);
            using (TextReader reader = new StreamReader(fileName))
            {
                var contents = reader.ReadToEnd();
                if (!string.IsNullOrEmpty(contents))
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                    result = JsonSerializer.Deserialize<TSerialize>(contents);
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
            }
            return result;
        }
        public static void Serialize(object contents,string fileName)
        {
            using(FileStream writer = File.Open(fileName,FileMode.Create))
            {
                JsonSerializerOptions options = new JsonSerializerOptions();
                options.WriteIndented = true;
                JsonSerializer.Serialize(writer, contents, contents.GetType(),options);
            }
        }
        public static HintProject? LoadHintData(string fileName)
        {
            BaseObject.InDataOps = true;
            //HintData.InDataOps = true;
            var result = Deserialize<HintProject>(fileName);
            BaseObject.InDataOps = false;
            //HintData.InDataOps = false;
            return result;
        }
    }
}
