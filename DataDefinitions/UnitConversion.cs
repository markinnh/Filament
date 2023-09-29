using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataDefinitions
{
    /// <summary>
    /// Converts units from imperial to metric
    /// </summary>
    public class UnitConversion
    {
        static string[] shortHandLengthNames = new string[] { "mm", "cm", "m" };
        static string[] shortHandWeightNames = new string[] { "g", "mg", "kg" };

        /// <summary>
        /// doesn't really convert anything, just formats it with the expected units, default is millimeters
        /// </summary>
        /// <param name="value">value to format</param>
        /// <param name="parameter">expected units</param>
        /// <returns>formatted result</returns>
        public static object FormatLengthWithUnits(object value,object parameter)
        {
            ConvertToLength convertTo = ConvertToLength.Millimeter;
            if (parameter is string paraString)
            {
                if (value is double d)
                {
                    if (!double.IsNaN(d))
                    {
                        if (Enum.TryParse(paraString, out ConvertToLength result))
                            return $"{value:#.###} {shortHandLengthNames[(int)result]}";
                        else
                            return $"{value:#.###} {shortHandLengthNames[(int)convertTo]}";
                    }
                    else
                        return value;
                }
                else
                    return value;
            }
            return value;

            //throw new NotImplementedException();
        }
        public static object ConvertWithUnitsOfLength(object value,object parameter)
        {
            ConvertToLength convertTo = ConvertToLength.Millimeter;
            string defaultUnits;
            if (parameter is string paraString)
            {
                if (Enum.TryParse(paraString, out ConvertToLength result))
                {
                    convertTo = result;
                }
                defaultUnits = shortHandLengthNames[(int)convertTo];
            }
            else
                defaultUnits = shortHandLengthNames[(int)convertTo];

            if (value is string str)
            {
                if (CompoundFractionWithUnits.TryParse(str, out CompoundFractionWithUnits compoundFractionWithUnits))
                {
                    //var fracMatch = Regex.Match(str, regexFindFraction);
                    SupportedLength supported = FilamentMath.SupportedLengthAlias(!string.IsNullOrEmpty(compoundFractionWithUnits.Units) ? compoundFractionWithUnits.Units : "in");
                    return FilamentMath.ConvertLength(compoundFractionWithUnits.Value, supported, convertTo);

                }
                else if (ValueWithUnits.TryParse(str, out ValueWithUnits valueWithUnits))
                {
                    SupportedLength supported = FilamentMath.SupportedLengthAlias(string.IsNullOrEmpty(valueWithUnits.Units) ? defaultUnits : valueWithUnits.Units);

                    return FilamentMath.ConvertLength(valueWithUnits.Value, supported, convertTo);

                }

                else
                    return double.NaN;
            }
            else
                return double.NaN;
        }
        public static object FormatWeightWithUnits(object value, object parameter)
        {
            if (parameter is string str)
                if (Enum.TryParse(str, out ConvertToWeight convertTo))
                    if (value is double d)
                        if (double.IsNaN(d))
                            return d;
                        else
                            return $"{value:0.######} {shortHandWeightNames[(int)convertTo]}";



            return value;

            //throw new NotImplementedException();
        }
        public static object ConvertWithUnitsOfWeight(object value, object parameter)
        {
            ConvertToWeight convertTo = ConvertToWeight.Kilograms;
            string defaultUnits;
            if (parameter is string paraString)
            {
                if (Enum.TryParse(paraString, out ConvertToWeight result))
                {
                    convertTo = result;
                    defaultUnits = shortHandWeightNames[(int)result];
                }
                else
                    defaultUnits = shortHandWeightNames[(int)convertTo];
            }
            else
                defaultUnits = shortHandWeightNames[(int)convertTo];
            if (value is string str)
            {
                if (ValueWithUnits.TryParse(str, out ValueWithUnits valueWithUnits))
                {
                    return FilamentMath.ConvertWeight(valueWithUnits.Value,
                        FilamentMath.SupportedWeightAlias(string.IsNullOrEmpty(valueWithUnits.Units) ? defaultUnits : valueWithUnits.Units),
                        convertTo);
                }
                else
                    return double.NaN;
            }
            if (value is double d)
                return d;
            else
                return double.NaN;


            //throw new NotImplementedException();
        }
    }
}
