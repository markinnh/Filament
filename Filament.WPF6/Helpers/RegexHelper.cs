using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Filament.WPF6.Helpers
{
    internal class RegexHelper
    {
        internal static bool IsMatch(string search, string pattern, out Match match)
        {
            if (Regex.IsMatch(search, pattern))
            {
                match = Regex.Match(search, pattern);
                return true;
            }
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            match = default;
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
            return false;

        }
    }
}
