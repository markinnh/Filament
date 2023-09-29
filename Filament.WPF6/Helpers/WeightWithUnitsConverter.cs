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

    //internal enum ConvertToWeightEnum
    //{
    //    Grams,
    //    Milligrams,
    //    Kilograms
    //}
    internal class WeightWithUnitsConverter : IValueConverter
    {
        internal const string regexFindNumberAndUnit = @"(?<number>\d*(\.\d+)?) *(?<units>[a-z\""]+)";

        static string[] shortHandnames = new string[] { "g", "mg", "kg" };
        const char space = ' ';
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return UnitConversion.FormatWeightWithUnits(value, parameter);
            //if (parameter is string str)
            //    if (Enum.TryParse(str, out ConvertToWeightEnum convertTo))
            //        if (value is double d)
            //            if (double.IsNaN(d))
            //                return d;
            //            else
            //                return $"{value:0.######} {shortHandnames[(int)convertTo]}";



            //return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return UnitConversion.ConvertWithUnitsOfWeight(value, parameter);
            //ConvertToWeight convertTo = ConvertToWeight.Kilograms;
            //string defaultUnits;
            //if (parameter is string paraString)
            //{
            //    if (Enum.TryParse(paraString, out ConvertToWeight result))
            //    {
            //        convertTo = result;
            //        defaultUnits = shortHandnames[(int)result];
            //    }
            //    else
            //        defaultUnits = shortHandnames[(int)convertTo];
            //}
            //else
            //    defaultUnits = shortHandnames[(int)convertTo];
            //if (value is string str)
            //{
            //    //if (str.Contains(space))
            //    //{
            //    //    var args = str.Split(space);
            //    //    if (args.Length ==2 && args.Count(a => a.Length > 0) == 2)
            //    //    {
            //    //        if (double.TryParse(args[0], out double result))
            //    //            return result * ConversionFactor(args[1].ToLower(), convertTo);
            //    //        else
            //    //            return double.NaN;
            //    //    }
            //    //}
            //    //else if (str.Count(ch => char.IsLetter(ch)) > 0)
            //    //{
            //    //    var num = new string(str.Where(ch=>char.IsDigit(ch)||ch== '.').ToArray());
            //    //    var units = new string(str.Where(ch => char.IsLetter(ch)).ToArray());
            //    //    if(double.TryParse(num, out double result))
            //    //        return result * ConversionFactor(units.ToLower(),convertTo);
            //    //}
            //    //if (Regex.IsMatch(str, regexFindNumberAndUnit))
            //    //{
            //    //    var match = Regex.Match(str, regexFindNumberAndUnit);
            //    //    if (double.TryParse(match.Groups["number"].Value, out double result))
            //    //        return result * ConversionFactor(String.IsNullOrEmpty(match.Groups["units"].Value) ? defaultUnits : match.Groups["units"].Value, convertTo);
            //    //    else
            //    //        return double.NaN;
            //    //}
            //    if(ValueWithUnits.TryParse(str,out ValueWithUnits valueWithUnits))
            //    {
            //        return FilamentMath.ConvertWeight(valueWithUnits.Value, 
            //            FilamentMath.SupportedWeightAlias(string.IsNullOrEmpty(valueWithUnits.Units) ? defaultUnits : valueWithUnits.Units), 
            //            convertTo);
            //    }
            //    else
            //        return double.NaN;
            //}
            //if (value is double d)
            //    return d;
            //else
            //    return double.NaN;

            //throw new NotImplementedException();
        }
        //protected double ConversionFactor(string units, ConvertToWeightEnum convertTo)
        //{
        //    double result = 0;
        //    switch (units)
        //    {
        //        case "kg":
        //            result = convertTo switch
        //            {
        //                ConvertToWeightEnum.Grams => 1000,
        //                ConvertToWeightEnum.Milligrams => 1e6,
        //                ConvertToWeightEnum.Kilograms => 1,
        //                _ => double.NaN,
        //            };
                    
        //            break;
        //        case "g":
        //            result = convertTo switch
        //            {
        //                ConvertToWeightEnum.Grams => 1,
        //                ConvertToWeightEnum.Milligrams => 1e3,
        //                ConvertToWeightEnum.Kilograms => 1e-3,
        //                _=>double.NaN
        //            };
                    
        //            break;
        //        case "mg":
        //            result = convertTo switch
        //            {
        //                ConvertToWeightEnum.Grams => 1e-3,
        //                ConvertToWeightEnum.Milligrams => 1,
        //                ConvertToWeightEnum.Kilograms => 1e-6,
        //                _ => double.NaN
        //            };
        //            break;
        //        case "oz":
        //            result = convertTo switch
        //            {
        //                ConvertToWeightEnum.Grams => 28.3495231,
        //                ConvertToWeightEnum.Milligrams => 28349.5231,
        //                ConvertToWeightEnum.Kilograms => 0.02834952,
        //                _ => double.NaN
        //            };
        //            break;
        //        case "lb":
        //            result = FromPound(convertTo);
        //            break;
        //    }
        //    return result;
        //}
        //protected static double FromKilogram(ConvertToWeightEnum convertTo) => convertTo switch
        //{
        //    ConvertToWeightEnum.Kilograms => 1,
        //    ConvertToWeightEnum.Grams => 1000,
        //    ConvertToWeightEnum.Milligrams => 1e6,
        //    _ => double.NaN
        //};
        //protected static double FromPound(ConvertToWeightEnum convertTo) => convertTo switch
        //{
        //    ConvertToWeightEnum.Kilograms => 0.45359237,
        //    ConvertToWeightEnum.Grams => 453.59237,
        //    ConvertToWeightEnum.Milligrams => 453.59237 * 1000,
        //    _ => double.NaN
        //};
        //protected static double FromGram(ConvertToWeightEnum convertTo) => convertTo switch
        //{
        //    ConvertToWeightEnum.Kilograms => 1e-3,
        //    ConvertToWeightEnum.Grams => 1,
        //    ConvertToWeightEnum.Milligrams => 1000,
        //    _ => double.NaN

        //};
        //protected static double FromOunce(ConvertToWeightEnum convertTo) => convertTo switch
        //{
        //    ConvertToWeightEnum.Kilograms => 0.02834952,
        //    ConvertToWeightEnum.Grams => 28.3495231,
        //    ConvertToWeightEnum.Milligrams => 28.3495231 * 1e3,
        //    _ => double.NaN
        //};
        //protected static double FromMilligram(ConvertToWeightEnum convertTo) => convertTo switch
        //{
        //    ConvertToWeightEnum.Kilograms => 1e-6,
        //    ConvertToWeightEnum.Grams => 1e-3,
        //    ConvertToWeightEnum.Milligrams => 1,
        //    _ => double.NaN

        //};
    }
}
