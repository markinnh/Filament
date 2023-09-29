using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataDefinitions
{
    public class ArrayToTuple
    {
        public static object ObjectArrayToTuple(object[] values)
        {
            if (values != null)
            {
                switch (values.Length)
                {
                    case 1:
                        return Tuple.Create(values[0]);
                    //break;
                    case 2:
                        return Tuple.Create(values[0], values[1]);
                    //break;
                    case 3:
                        return Tuple.Create(values[0], values[1], values[2]);
                    //break;
                    case 4:
                        return Tuple.Create(values[0], values[1], values[2], values[3]);
                    //break;
                    case 5:
                        return Tuple.Create(values[0], values[1], values[2], values[3], values[4]);
                    //break;
                    case 6:
                        return Tuple.Create(values[0], values[1], values[2], values[3], values[4], values[5]);
                    case 7:
                        return Tuple.Create(values[0], values[1], values[2], values[3], values[4], values[5], values[6]);
                    default:
                        throw new ArgumentOutOfRangeException($"No support for {values.Length} arguments");
                }
            }
            return null;
        }

    }
}
