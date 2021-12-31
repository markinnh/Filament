using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filament.UWP.Helpers
{
    public class EnumListProvider<TEnum> : List<TEnum> where TEnum : Enum
    {
        public EnumListProvider()
        {
            foreach (var item in Enum.GetValues(typeof(TEnum)))
                Add((TEnum)item);
            
        }
    }
}
