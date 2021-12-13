using System;
using System.Collections.Generic;
using System.Text;

namespace Filament_Data.DataFilters
{
    /// <summary>
    /// contains all regex patterns to extract data for a certain type of slicer
    /// </summary>
    internal class PatternContainer
    {
        private static readonly Dictionary<string, List<string>> data = new Dictionary<string, List<string>>()
        {
            { "Cura",new List<string>()
            { 
                @";Filament used: (?<Filament>[0-9]*\.[0-9]*)" , 
                @";TIME:(?<Time>[0-9]*)" } 
            }
        };
        internal static string[] Patterns(string key)
        {
            if (data.TryGetValue(key, out List<string> value))
                return value.ToArray();
            else
                throw new KeyNotFoundException();
        }
    }
}
