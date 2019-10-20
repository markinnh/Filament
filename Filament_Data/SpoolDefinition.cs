using System;

namespace Filament_Data
{
    public class SpoolDefinition
    {
        public decimal SpoolDiameter { get; set; }
        public decimal MinimumDiameter { get; set; }
        public FilamentDefn Filament { get; set; }
        public decimal SpoolWidth { get; set; }
        public bool Verified { get; set; }
        //internal SpoolDefinition(decimal spoolDiameter, decimal mimumumDiameter, decimal filamentDiameter, decimal spoolWidth)
        //{
        //    SpoolDiameter = spoolDiameter;
        //    MinimumDiameter = mimumumDiameter;
        //    FilamentDiameter = filamentDiameter;
        //    SpoolWidth = spoolWidth;
        //    Verified = true;
        //}
        internal SpoolDefinition(decimal spoolDiameter, decimal minimumDiameter, decimal spoolWidth,FilamentDefn filament, bool verified = true)
        {
            SpoolDiameter = spoolDiameter;
            MinimumDiameter = minimumDiameter;
            //FilamentDiameter = filamentDiameter;
            Filament = filament;
            SpoolWidth = spoolWidth;
            Verified = verified;
        }
    }
}
