using System;
using System.Collections.Generic;
using System.Text;

namespace DataDefinitions
{
    public class FilamentMath
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="density">in gms per cc</param>
        /// <param name="weight">in grams</param>
        /// <param name="diameter">in millimeters</param>
        /// <returns>Length in millimeters</returns>
        public static double LengthFromWeightBasedOnDensity(IDensity density, double weight, double diameter)
        {
            return weight / (density.Density * (Math.Pow(diameter / 2, 2) * Math.PI))/ConvertFromCubicMillimetersToCubicCentimeters;
        }
        public static double ConvertTo(double measurement, SupportedVolume volume, ConvertToVolume convert)
        {
            double result = double.NaN;
            if (!double.IsNaN(measurement))
            {
                switch (convert)
                {
                    case ConvertToVolume.CubicCentimeter:
                        switch (volume)
                        {
                            case SupportedVolume.CubicCentimeter:
                                result = measurement;
                                break;
                            case SupportedVolume.CubicMillimeter:
                                result = measurement * Constants.CubicMillimetersToCubicCentimeter;
                                break;
                            case SupportedVolume.CubicInches:
                                result = measurement * Constants.CubicInchesToCubicCentimeters;
                                break;
                            default:
                                System.Diagnostics.Debug.WriteLine($"{volume} not supported.");
                                break;
                        }
                        break;
                    case ConvertToVolume.CubicMillimeter:
                        switch (volume)
                        {
                            case SupportedVolume.CubicMillimeter:
                                result = measurement;
                                break;
                            case SupportedVolume.CubicCentimeter:
                                result = measurement * Constants.CubicInchesToCubicCentimeters;
                                break;
                            case SupportedVolume.CubicInches:
                                result = measurement * Constants.CubicInchesToCubicMillimeters;
                                break;
                            default:
                                System.Diagnostics.Debug.WriteLine($"{volume} not supported.");
                                break;
                        }
                        break;
                    default:
                        System.Diagnostics.Debug.WriteLine($"Conversion for {convert} is not supported.");
                        break;
                }
                return result;
            }
            return measurement;
        }
        public static double ConvertLength(double measurement, SupportedLength length, ConvertToLength convertTo)
        {
            double result = double.NaN;
            if (!double.IsNaN(measurement))
            {
                switch (convertTo)
                {
                    case ConvertToLength.Millimeter:
                        switch (length)
                        {
                            case SupportedLength.Millimeter:
                                result = measurement;
                                break;
                            case SupportedLength.Meter:
                                result = measurement * 1e3;
                                break;
                            case SupportedLength.Inches:
                                result = measurement * Constants.InchToMillimeters;
                                break;
                            case SupportedLength.Centimeter:
                                result = measurement * 10;
                                break;
                            default:
                                System.Diagnostics.Debug.WriteLine($"{length} not supported.");
                                break;
                        }
                        break;
                    case ConvertToLength.Meter:
                        switch (length)
                        {
                            case SupportedLength.Millimeter:
                                result = measurement * 1e-3;
                                break;

                            case SupportedLength.Inches:
                                result = measurement * Constants.InchToMillimeters * Constants.MillimetersToMeters;
                                break;
                            case SupportedLength.Meter:
                                result = measurement;
                                break;
                            case SupportedLength.Centimeter:
                                result = measurement * 1e-2;
                                break;
                            default:
                                System.Diagnostics.Debug.WriteLine($"{length} not supported.");
                                break;
                        }
                        break;
                    case ConvertToLength.Centimeter:
                        switch (length)
                        {
                            case SupportedLength.Millimeter:
                                result = measurement * 0.1;
                                break;
                            case SupportedLength.Meter:
                                result = measurement * 100;
                                break;
                            case SupportedLength.Inches:
                                result = measurement * 2.54;
                                break;
                            case SupportedLength.Centimeter:
                                result = measurement;
                                break;
                            default:
                                System.Diagnostics.Debug.WriteLine($"{length} not supported.");
                                break;
                        }
                        break;
                    default:
                        break;
                }
                return result;
            }
            return measurement;
        }
        public static double ConvertWeight(double measurement, SupportedWeight supported, ConvertToWeight convertTo)
        {
            double result = double.NaN;

            if (!double.IsNaN(measurement))
            {

                switch (supported)
                {
                    case SupportedWeight.Gram:
                        switch (convertTo)
                        {
                            case ConvertToWeight.Grams:
                                result = measurement;
                                break;
                            case ConvertToWeight.Millgrams:
                                result = measurement * 1000;
                                break;
                            case ConvertToWeight.Kilograms:
                                result = measurement * 1e-3;
                                break;
                            default:
                                System.Diagnostics.Debug.WriteLine($"{supported} is not supported.");
                                break;

                        }
                        break;
                    case SupportedWeight.Kilogram:
                        switch (convertTo)
                        {
                            case ConvertToWeight.Grams:
                                result = measurement * 1000;
                                break;
                            case ConvertToWeight.Millgrams:
                                result = measurement * 1e6;
                                break;
                            case ConvertToWeight.Kilograms:
                                result = measurement;
                                break;
                            default:
                                System.Diagnostics.Debug.WriteLine($"{supported} is not supported.");
                                break;
                        }
                        break;
                    case SupportedWeight.Ounce:
                        switch (convertTo)
                        {
                            case ConvertToWeight.Grams:
                                result = measurement * Constants.OunceToGram;
                                break;
                            case ConvertToWeight.Millgrams:
                                result = measurement * Constants.OunceToGram * 1000;
                                break;
                            case ConvertToWeight.Kilograms:
                                result = measurement * Constants.OunceToGram / 1000;
                                break;
                            default:
                                System.Diagnostics.Debug.WriteLine($"{supported} is not supported.");
                                break;
                        }
                        break;
                    case SupportedWeight.Pound:
                        switch (convertTo)
                        {
                            case ConvertToWeight.Grams:
                                result = measurement * Constants.PoundToGram;
                                break;
                            case ConvertToWeight.Millgrams:
                                result = measurement * Constants.PoundToGram * 1000;
                                break;
                            case ConvertToWeight.Kilograms:
                                result = measurement * Constants.PoundToKg;
                                break;
                            default:
                                System.Diagnostics.Debug.WriteLine($"{supported} is not supported.");
                                break;
                        }
                        break;
                    default:
                        break;
                }
                return result;
            }
            return measurement;
        }
        /// <summary>
        /// returns supported weight from shorthand weight specifier, default is kilogram if inputUnits is empty
        /// </summary>
        /// <param name="inputUnits">shorthand units for SupportedWeight</param>
        /// <returns>SupportedWeight</returns>
        public static SupportedWeight SupportedWeightAlias(string inputUnits)
        {

            SupportedWeight result;
            switch (inputUnits)
            {
                case "lb":
                    result = SupportedWeight.Pound;
                    break;
                case "oz":
                    result = SupportedWeight.Ounce;
                    break;
                case "g":
                    result = SupportedWeight.Gram;
                    break;
                case "kg":
                    result = SupportedWeight.Kilogram;
                    break;
                default:
                    result = SupportedWeight.Kilogram;
                    break;
            }
            return result;
        }
        /// <summary>
        /// Returns a SupportedLength from a commonly recognized shorthand name default is millimeters
        /// </summary>
        /// <param name="inputUnits">shorthand units to convert</param>
        /// <returns>SupportedLength</returns>
        public static SupportedLength SupportedLengthAlias(string inputUnits)
        {
            SupportedLength result;
            switch (inputUnits)
            {
                case "mm":
                    result = SupportedLength.Millimeter;
                    break;
                case "m":
                    result = SupportedLength.Meter;
                    break;
                case "in":
                case "\"":
                    result = SupportedLength.Inches;
                    break;
                case "cm":
                    result = SupportedLength.Centimeter;
                    break;
                default:
                    result = SupportedLength.Millimeter;
                    break;
            }
            return result;
        }
    }

}
