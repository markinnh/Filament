using System;
using System.Collections.Generic;
using System.Text;

namespace Filament_Data
{
    public class PredefinedSpools
    {
        public Dictionary<string, SpoolDefinition> Definitions { get; }
        public PredefinedSpools()
        {
            FilamentDefn defn = new FilamentDefn();
            Definitions = new Dictionary<string, SpoolDefinition>()
            {
                {"3D Solutech",new SpoolDefinition(200,81,55,defn) },
                {"Hatchbox", new SpoolDefinition(200,80,55,defn) }
            };
        }
    }
}
