using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Filament_Data
{
    public static class Extensions
    {
        public static IEnumerable<TValue> Lookup<TKey,TValue>(this Dictionary<TKey,TValue> target,Func<TValue, bool> filter)
        {
            return target.Values.Where(filter);
        }
        public static bool IsModified<TValue>(this IEnumerable<TValue> target) where TValue:IChangeTrackItem
        {
            return target.Where(t => t.IsModified).Count() > 0;
        }
    }
}
