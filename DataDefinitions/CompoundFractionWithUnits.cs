using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace DataDefinitions
{
    /// <summary>
    /// returns a compound fraction with units, if no units are specified it defaults to "in" since this was the primary purpose for this class
    /// </summary>
    public class CompoundFractionWithUnits
    {
        public double Value { get; set; }
        public string Units { get; set; }

        public CompoundFractionWithUnits(string contents)
        {
            Match match = Regex.Match(contents, Constants.regexFindFraction);
            if (match.Success)
            {
                double fraction = 0;
                if (double.TryParse(match.Groups["numerator"].Value, out double numerator) && double.TryParse(match.Groups["denominator"].Value, out double denominator))
                {
                    if (denominator > 0)
                        fraction = numerator / denominator;
                    else
                        fraction = double.NaN;


                }
                if (!string.IsNullOrEmpty(match.Groups["whole"].Value))
                {
                    if (double.TryParse(match.Groups["whole"].Value, out double whole))
                    {
                        Value = whole + fraction;
                    }
                }
                else
                {
                    Value = fraction;
                }
                Units = string.IsNullOrEmpty(match.Groups["units"].Value) ? "in" : match.Groups["units"].Value;
            }
        }
        public static bool TryParse(string contents, out CompoundFractionWithUnits compoundFractionWithUnits)
        {
            var success = Regex.IsMatch(contents, Constants.regexFindFraction);
            compoundFractionWithUnits = success ? new CompoundFractionWithUnits(contents) : default;
            return success;
            //if (Regex.IsMatch(contents, Constants.regexFindFraction))
            //{
            //    compoundFractionWithUnits = new CompoundFractionWithUnits(contents);
            //    return true;
            //}
            //else
            //{
            //    compoundFractionWithUnits = default;
            //    return false;
            //}
        }
    }
}
