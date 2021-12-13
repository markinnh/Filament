using System;
using System.Collections.Generic;
using System.Text;

namespace Filament_Db
{
    public class Constants
    {
        public const double BasicPLADensity = 1.24;
        public const double BasicABSDensity = 1.04;
        public const double BasicPETGDensity = 1.27;
        public const double BasicNylonDensity = 1.04;
        public const double BasicPolycarbonateDensity = 1.19;
        public const double BasicWoodPLA = 1.4;

        // spool size definitions, for spools I have personally used.
        public const double StandardSpoolLoad = 1.0;
        public const short HatchBox1KgSpoolOuterDiameter = 199;
        public const short HatchBox1KgSpoolDrumDiameter = 77;
        public const short HatchBox1KgSpoolWidth = 63;
        public const short Solutech1KgSpoolOuterDiameter = 200;
        public const short Solutech1KgSpoolDrumDiameter = 80;
        public const short Solutech1KgSpoolWidth = 55;

        // Vendor names, the list is incomplete        
        public const string _3DSolutechName = "3D Solutech";
        public const string HatchBoxName = "HatchBox";
        public const string SunluName = "Sunlu";
        public const string DefaultVendorName = "Generic";

    }
}
