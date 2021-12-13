using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Reflection;

namespace Filament_Data.JsonModel
{
    public static class Extensions
    {
        public static IEnumerable<TValue> Lookup<TKey,TValue>(this Dictionary<TKey,TValue> target,Func<TValue, bool> filter)
        {
            return target.Values.Where(filter);
        }
        /// <summary>
        /// Determines whether this items in this instance are modified.  Requires the collection to implement the ITrackModified interface
        /// </summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="target">The target.</param>
        /// <returns>
        ///   <c>true</c> if the specified target has modified elements; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsModified<TValue>(this IEnumerable<TValue> target) where TValue:ITrackModified
        {
            return target.Where(t => t.IsModified).Count() > 0;
        }
        /// <summary>
        /// Gets the properties with an defined attribute.
        /// </summary>
        /// <typeparam name="FilterAttribute">The type of the ilter attribute.</typeparam>
        /// <param name="target">The target.</param>
        /// <returns></returns>
        public static IEnumerable<PropertyInfo> GetPropertiesFilteredByAttribute<FilterAttribute>(this Type target) where FilterAttribute : Attribute
        {

            var queryResult = target.GetProperties(BindingFlags.Instance).Where(pi => pi.GetCustomAttributes<FilterAttribute>(true).Count() > 0);
            return queryResult;
        }
    }
}
