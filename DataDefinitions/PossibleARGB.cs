using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace DataDefinitions
{
    public class PossibleARGB
    {
        public byte A { get; set; }
        public byte B { get; set; }
        public byte G { get; set; }
        public byte R { get; set; }

        public PossibleARGB(string test, out bool success)
        {
            var match = Regex.Match(test, Constants.regexARGB);
            if (match.Success)
            {
                if(byte.TryParse(match.Groups["alpha"].Value,out byte alpha) && 
                    byte.TryParse(match.Groups["red"].Value,out byte red)&& 
                    byte.TryParse(match.Groups["blue"].Value,out byte blue)&&
                    byte.TryParse(match.Groups["green"].Value,out byte green))
                {
                    A= alpha;
                    R= red;
                    B= blue;
                    G= green;
                }
            }
            success = match.Success;
        }

        public static bool TryParse(string test,out PossibleARGB possibleARGB)
        {
            possibleARGB = new PossibleARGB(test,out bool success);
            return success;
        }
    }
}
