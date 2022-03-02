using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Data;
using DataDefinitions;

namespace Filament.WPF6.Helpers
{

    //public enum ConvertToLengthEnum
    //{
    //    Millimeters,
    //    Centimeter,
    //    Meters
    //}
    internal class LengthWithUnitsConverter : IValueConverter
    {
        static string[] shortHandnames = new string[] { "mm", "cm", "m" };

        //const string regexFindFraction = @"(?<whole>\d+)? (?<numerator>\d+)/(?<denominator>\d+) *(?<units>[a-z\x22]+)?";
        // commented out so only using one pattern to find a number with units
        // do not use this one : const string regexFindNumberAndUnit = @"(?<number>\d*(\.\d+)?) ?(?<units>[a-z\x22]+)";
        const char space = ' ';
        const string inchesShorthand = "\"";
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ConvertToLength convertTo = ConvertToLength.Millimeter;
            if (parameter is string paraString)
            {
                if (value is double d)
                {
                    if (!double.IsNaN(d))
                    {
                        if (Enum.TryParse(paraString, out ConvertToLength result))
                            return $"{value:#.###} {shortHandnames[(int)result]}";
                        else
                            return $"{value:#.###} {shortHandnames[(int)convertTo]}";
                    }
                    else
                        return value;
                }
                else
                    return value;
            }
            return value;

            //if (value is string str)
            //{
            //    if (str.Contains(space))
            //    {
            //        var args = str.Split(space);
            //        if (args.Length == 2 && args.Count(a => a.Length > 0) == 2)
            //        {
            //            System.Diagnostics.Debug.WriteLine($"recieved {args[0]}, {args[1]}");
            //            if (double.TryParse(args[0], out double result))
            //                return result;
            //        }
            //    }
            //}
            //else if (value is double d)
            //    return d;

            //return value;
            //throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ConvertToLength convertTo = ConvertToLength.Millimeter;
            string defaultUnits;
            if (parameter is string paraString)
            {
                if (Enum.TryParse(paraString, out ConvertToLength result))
                {
                    convertTo = result;
                }
                defaultUnits = shortHandnames[(int)convertTo];
            }
            else
                defaultUnits = shortHandnames[(int)convertTo];

            if (value is string str)
            {
                if (CompoundFractionWithUnits.TryParse(str, out CompoundFractionWithUnits compoundFractionWithUnits))
                {
                    //var fracMatch = Regex.Match(str, regexFindFraction);
                    SupportedLength supported = FilamentMath.SupportedLengthAlias(!string.IsNullOrEmpty(compoundFractionWithUnits.Units) ? compoundFractionWithUnits.Units : "in");
                    return FilamentMath.ConvertLength(compoundFractionWithUnits.Value, supported, convertTo);
                    //double number;
                    //double numerator;
                    //double denominator;
                    //if (double.TryParse(fracMatch.Groups["numerator"].Value, out numerator) && double.TryParse(fracMatch.Groups["denominator"].Value, out denominator))
                    //{
                    //    if (!string.IsNullOrEmpty(fracMatch.Groups["whole"].Value))
                    //    {
                    //        if (double.TryParse(fracMatch.Groups["whole"].Value, out number))
                    //        {
                    //            return (number + (numerator / denominator)) * ConversionFactor(string.IsNullOrEmpty(fracMatch.Groups["units"].Value) ? "in" : fracMatch.Groups["units"].Value.ToLower(), convertTo);
                    //        }
                    //        else
                    //            return double.NaN;
                    //    }
                    //    else
                    //    {
                    //        return (numerator / denominator) * ConversionFactor(string.IsNullOrEmpty(fracMatch.Groups["units"].Value) ? "in" : fracMatch.Groups["units"].Value.ToLower(), convertTo);
                    //    }
                
                }
                else if (ValueWithUnits.TryParse(str, out ValueWithUnits valueWithUnits))
                {
                    //var match = Regex.Match(str, WeightWithUnitsConverter.regexFindNumberAndUnit);
                    SupportedLength supported = FilamentMath.SupportedLengthAlias(string.IsNullOrEmpty(valueWithUnits.Units) ? defaultUnits : valueWithUnits.Units);

                    return FilamentMath.ConvertLength(valueWithUnits.Value, supported, convertTo);

                }
                
                else
                    return double.NaN;
            }
            else
                return double.NaN;
        }
        //protected double ConversionFactor(string units, ConvertToLengthEnum convertTo)
        //{
        //    double result = 0;
        //    switch (units)
        //    {
        //        case "mm":
        //            switch (convertTo)
        //            {
        //                case ConvertToLengthEnum.Millimeters:
        //                    result = 1;
        //                    break;
        //                case ConvertToLengthEnum.Centimeter:
        //                    result = 0.1;
        //                    break;
        //                case ConvertToLengthEnum.Meters:
        //                    result = 1.0e-3;
        //                    break;
        //            }
        //            break;
        //        case "cm":
        //            switch (convertTo)
        //            {
        //                case ConvertToLengthEnum.Millimeters:
        //                    result = 10;
        //                    break;
        //                case ConvertToLengthEnum.Centimeter:
        //                    result = 1;
        //                    break;
        //                case ConvertToLengthEnum.Meters:
        //                    result = .01;
        //                    break;
        //            }
        //            break;
        //        case "m":
        //            switch (convertTo)
        //            {
        //                case ConvertToLengthEnum.Millimeters:
        //                    result = 1000;
        //                    break;
        //                case ConvertToLengthEnum.Centimeter:
        //                    result = 100;
        //                    break;
        //                case ConvertToLengthEnum.Meters:
        //                    result = 1;
        //                    break;
        //            }
        //            break;
        //        case "\"":
        //        case "in":
        //            switch (convertTo)
        //            {
        //                case ConvertToLengthEnum.Millimeters:
        //                    result = 25.4;
        //                    break;
        //                case ConvertToLengthEnum.Centimeter:
        //                    result = 2.54;
        //                    break;
        //                case ConvertToLengthEnum.Meters:
        //                    result = .0254;
        //                    break;
        //            }
        //            break;
        //        default:
        //            System.Diagnostics.Debug.WriteLine($"{units} not recognized.");
        //            result = double.NaN;
        //            break;
        //    }
        //    return result;
        //}
    }
    
}

