using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;

namespace Filament_Data.DataFilters
{
    /// <summary>
    /// Retrieves information from a text based file
    /// </summary>
    internal class FileInfo
    {
        //private readonly List<RegexPattern> regexPatterns = new List<RegexPattern>();
        //internal List<RegexPattern> Patterns { get => regexPatterns; }
        internal FileInfo()
        {
        }

        //internal void Add(string pattern)
        //{
        //    if (regexPatterns.Where(pat => pat.Pattern == pattern).Count() == 0)
        //        regexPatterns.Add(new RegexPattern(pattern));
        //}

        internal FindType FindMatches<FindType>(string fileName) where FindType:ParsedObjects.BaseParsedObject,new()
        {
            using (System.IO.StreamReader reader = new System.IO.StreamReader(fileName))
            {
                var parsedObject = new FindType();
                StringBuilder builder = new StringBuilder();
                string curLine;
                do
                {
                    curLine = reader.ReadLine();
                    builder.AppendLine(curLine);
                } while (!curLine.Contains(parsedObject.EndOfHeaderTag));

                if (parsedObject.InitWithParseableString(builder.ToString()))
                    return parsedObject;
                else
                    return default;

                //foreach (var test in regexPatterns)
                //{
                //    var testResult = test.Regex.Matches(curLine);
                //    AddMatchesToResults(testResult, result, test.Regex.Names);
                //}
            }
        }

        //private void AddMatchesToResults(MatchCollection testResult, Dictionary<string, string> result, string[] names)
        //{
        //    foreach (Match item in testResult)
        //    {
        //        foreach (var name in names)
        //        {
        //            if (!string.IsNullOrEmpty(item.Groups[name].Value) && !result.ContainsKey(name))
        //                result.Add(name, item.Groups[name].Value);
        //        }
        //    }
        //}
    }
}
