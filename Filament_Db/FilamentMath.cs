using System;
using System.Collections.Generic;
using System.Text;

namespace Filament_Db
{
    internal class FilamentMath
    {
        public const double ConvertFromCubicMillimetersToCubicCentimeters = 0.001;
        /// <summary>
        /// Volume in cubic centimeters.
        /// </summary>
        /// <param name="radius">The radius in millimeters.</param>
        /// <param name="length">The length in millimeters.</param>
        /// <returns>volume in cubic centimeters</returns>
        public static float FilamentVolumeInCubicCentimeters(float radius, float length)
        {
            return (float)(radius * radius * Math.PI * length * ConvertFromCubicMillimetersToCubicCentimeters);
        }
        /// <summary>
        /// Volume in cubic centimeters.
        /// </summary>
        /// <param name="radius">The radius in millimeters.</param>
        /// <param name="length">The length in millimeters.</param>
        /// <returns>volume in cubic centimeters</returns>
        public static double FilamentVolumeInCubicCentimeters(double radius, double length) =>
            radius * radius * Math.PI * length * ConvertFromCubicMillimetersToCubicCentimeters;
    }
}
