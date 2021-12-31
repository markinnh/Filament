using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace DataDefinitions
{
    public class ValueWithUnits
    {
        public double Value { get; set; }
        public string Units { get; set; }
        public bool ValidEntry { get; set; }
        public ValueWithUnits(string contents)
        {
            Match match1 = Regex.Match(contents, Constants.regexFindNumberAndUnit);
            ValidEntry = match1.Success;
            if (match1.Success)
            {
                if (double.TryParse(match1.Groups["number"].Value, out double result))
                {
                    Value = result;
                    Units = match1.Groups["units"].Value;
                }

            }
        }
        public static bool TryParse(string contents, out ValueWithUnits valueWithUnits)
        {
            ValueWithUnits withUnits = new ValueWithUnits(contents);
            if (withUnits.ValidEntry)
            {
                valueWithUnits = withUnits;
                return true;
            }
            else
            {
                valueWithUnits = default;
                return false;
            }
        }
    }
}
