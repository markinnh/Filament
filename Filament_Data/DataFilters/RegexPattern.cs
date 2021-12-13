using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Filament_Data.DataFilters
{
    internal class RegexPattern
    {
        internal string Pattern { get; set; }
        internal RegexSearch Regex { get; set; }
        internal RegexPattern(string pattern)
        {
            Pattern = pattern;
            Regex = new RegexSearch(pattern);
        }
        internal class RegexSearch
        {
            protected readonly Regex regex;
            internal string[] Names => regex.GetGroupNames();
            internal RegexSearch(string pattern)
            {
                regex = new Regex(pattern);
            }
            internal MatchCollection Matches(string target)
            {
                if (regex.IsMatch(target))
                    return regex.Matches(target);
                else
                    return null;
            }
        }

    }
}
