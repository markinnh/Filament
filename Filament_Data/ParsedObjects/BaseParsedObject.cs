using System;
using System.Collections.Generic;
using System.Text;
using Filament_Data.DataFilters;
using System.Linq;
using static System.Diagnostics.Debug;
using System.Text.RegularExpressions;
using ml = MyLibraryStandard;
using Filament_Data.JsonModel;

namespace Filament_Data.ParsedObjects
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    internal sealed class ParsedAttribute : Attribute
    {
        private string groupName;
        public string GroupName { get => groupName ?? PatternGroupName(); set => groupName = value; }
        public string ParsePattern { get; set; }
        public ParsedAttribute(string parsePattern)
        {
            ParsePattern = parsePattern;
        }
        private string PatternGroupName()
        {
            var re = new Regex(ParsePattern);
            return re.GetGroupNames().First();
        }
    }
    /// <summary>
    /// A parsed object, expects all regex group names to be defined in properties
    /// </summary>
    internal abstract class BaseParsedObject : ISlicedHeader
    {
        //protected readonly RegexPattern pattern;
        internal abstract Regex Pattern { get; }
        internal abstract string EndOfHeaderTag { get; set; }


        internal int ParsedPropertiesCount => GetType().GetPropertiesFilteredByAttribute<ParsedAttribute>().Count();

        internal bool Initialized { get; private set; }
        public abstract IEnumerable<INamedInfo> Info { get; }
        internal BaseParsedObject()
        {
            //pattern = new RegexPattern(BuildRegexPattern());
            ValidateProperties();
        }
        internal bool InitWithParseableString(string content)
        {
            if (Pattern.IsMatch(content))
            {
                var matches = Pattern.Matches(content);

                int initCount = 0;
                var names = Pattern.GetGroupNames();
                foreach (Match match in matches)
                {
                    foreach (var name in names)
                    {
                        if (match.Groups[name].Success && !string.IsNullOrEmpty(match.Groups[name].Value))
                            SetPropValue(name, match.Groups[name].Value, ref initCount);
                    }
                }
                Initialized = initCount >= ParsedPropertiesCount;
                return Initialized;
            }
            else
            {
                WriteLine($"No matches found for {nameof(content)}");

                return false;

            }

        }
        protected abstract void SetPropValue(string propName, string propValue, ref int initCount);

        protected static string BuildRegexPattern<ParseType>() where ParseType : BaseParsedObject
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("(");
            var props = typeof(ParseType).GetPropertiesFilteredByAttribute<ParsedAttribute>().ToArray();
            for (int i = 0; i < props.Length; i++)
            {
                var parseAttrib = (ParsedAttribute)props[i].GetCustomAttributes(typeof(ParsedAttribute), true).FirstOrDefault();
                builder.AppendFormat("({0}){1}", parseAttrib.ParsePattern, i + 1 == props.Length ? ")" : "|");
            }
            return builder.ToString();
        }
        private void ValidateProperties()
        {
            Dictionary<string, bool> found = BuildGroupNameLocator();

            foreach (var prop in GetType().GetPropertiesFilteredByAttribute<ParsedAttribute>())
            {
                var parsedAttr = (ParsedAttribute)prop.GetCustomAttributes(typeof(ParsedAttribute), true).FirstOrDefault();

                if (found.ContainsKey(prop.Name))
                    found[prop.Name] = true;
                else if (!string.IsNullOrEmpty(parsedAttr.GroupName))
                    if (found.ContainsKey(parsedAttr.GroupName))
                        found[parsedAttr.GroupName] = true;
                    else
                        WriteLine($"{parsedAttr.GroupName} is not defined in the regex pattern");
            }
            // Fails if all of the group names are not defined in object properties
            foreach (var key in found.Keys)
            {
                Assert(found[key]);
            }

        }
        private Dictionary<string, bool> BuildGroupNameLocator()
        {
            var locator = new Dictionary<string, bool>();
            foreach (var name in Pattern.GetGroupNames())
            {
                locator.Add(name, false);
            }
            return locator;
        }
    }
}
