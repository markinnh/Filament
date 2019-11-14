using System;
using System.Collections.Generic;
using System.Text;

namespace Filament_Data
{
    public class CounterProvider:Dictionary<string,int>
    {
        const int startIndex = 1;
        internal bool IsModified { get; set; }
        public int NextID(Type type,int startingID = startIndex)
        {
            if (ContainsKey(type.FullName))
            {
                ++this[type.FullName];
                //this[type.FullName] = result;
                IsModified = true;
                return this[type.FullName];
            }
            else
            {

                Add(type.FullName, startingID);
                IsModified = true;
                return this[type.FullName];
            }
        }
    }
}
