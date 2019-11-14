using System;
using System.Collections.Generic;
using System.Text;

namespace Filament_Data
{
    public class LibraryMath
    {
        public const double ConvertFromCubicMillimetersToCubicCentimeters = 0.001;
        /// <summary>
        /// Volume in cubic centimeters.
        /// </summary>
        /// <param name="radius">The radius in millimeters.</param>
        /// <param name="length">The length in millimeters.</param>
        /// <returns></returns>
        public static float FilamentVolumeInCubicCentimeters(float radius,float length)
        {
            return (float)(Math.Pow((double)radius, 2) * Math.PI * (double)length * ConvertFromCubicMillimetersToCubicCentimeters);
        }
    }
}
