using System;
using System.Collections.Generic;
using System.Text;
//using Filament_Data.Alias;
using Filament_Data.DataFilters;
using System.Linq;
using System.Text.RegularExpressions;
using static System.Diagnostics.Debug;

namespace Filament_Data.ParsedObjects
{
    internal class CuraHeader : BaseParsedObject
    {
        private static readonly Regex pattern = new Regex(BuildRegexPattern<CuraHeader>());

        internal override Regex Pattern => pattern;

        internal override string EndOfHeaderTag { get; set; } = ";Generated with Cura_SteamEngine";
        //const string curaPattern = @"(;Filament used: (?<filament>[0-9]*\.[0-9]*))|(;TIME:(?<Time>[0-9]*))|(;MINX:(?<MinX>[0-9]*\.[0-9]*))|(;MINY:(?<MinY>[0-9]*\.[0-9]*))|(;MINZ:(?<MinZ>[0-9]*\.[0-9]*))|(;MAXX:(?<MaxX>[0-9]*\.[0-9]*))|(;MAXY:(?<MaxY>[0-9]*\.[0-9]*))|(;MAXZ:(?<MaxZ>[0-9]*\.[0-9]*))";
        [Parsed(@";TIME:(?<Time>[0 - 9] *)")]
        internal int Time { get; set; }
        [Parsed(@";Filament used:(?<Filament>[0-9]*\.[0-9]*)", GroupName = "Filament")]
        internal float FilamentUsed { get; set; }
        [Parsed(@";MINX:(?<MinX>[0-9]*\.[0-9]*)", GroupName = "MinX")]
        internal float MinX { get; set; }
        [Parsed(@";MINY:(?<MinY>[0-9]*\.[0-9]*)", GroupName = "MinY")]
        internal float MinY { get; set; }
        [Parsed(@";MINZ:(?<MinZ>[0-9]*\.[0-9]*)", GroupName = "MinZ")]
        internal float MinZ { get; set; }
        [Parsed(@";MAXX:(?<MaxX>[0-9]*\.[0-9]*)", GroupName = "MaxX")]
        internal float MaxX { get; set; }
        [Parsed(@";MAXY:(?<MaxY>[0-9]*\.[0-9]*)", GroupName = "MaxY")]
        internal float MaxY { get; set; }
        [Parsed(@";MAXZ:(?<MaxZ>[0-9]*\.[0-9]*)", GroupName = "MaxZ")]
        internal float MaxZ { get; set; }

        public override IEnumerable<INamedInfo> Info => GetInfo();
        public CuraHeader()
        {

        }
        private IEnumerable<INamedInfo> GetInfo()
        {
//            var formatSpecifier = @"Time :{0} 
//Filament Used :{1:0.00}m
//(W,L,H) :({2:0.00},{3:0.00},{4:0.00})";
            var timeAsHMS = new TimeSpan(0, 0, Time);
            var info = new NamedInfo[]
            {
                new NamedInfo("Time",GetFormattedTime(ref timeAsHMS)),
                new NamedInfo("Filament Used",$"{FilamentUsed:0.00} m"),
                new NamedInfo("(Width, Length, Height)",$"({MaxX-MinX:0.00},{MaxY-MinY:0.00},{MaxZ-MinZ:0.00})")
            };
            return info.Cast<INamedInfo>().AsEnumerable<INamedInfo>();
        }

        private static string GetFormattedTime(ref TimeSpan timeAsHMS)
        {
            //string formattedTime;
            if (timeAsHMS.TotalDays > 1)
                return string.Format("{0} days, {1} hours, {2} minutes", timeAsHMS.Days, timeAsHMS.Hours, timeAsHMS.Minutes);
            else if (timeAsHMS.TotalHours > 1)
                return string.Format("{0} hours, {1} minutes", timeAsHMS.Hours, timeAsHMS.Minutes);
            else
                return string.Format("{0} minutes", timeAsHMS.Minutes);
            //return formattedTime;
        }

        protected override void SetPropValue(string propName, string propValue, ref int initCount)
        {
           
            var aliaser = (MyLibrary.Parseable)propValue;
            if (aliaser.TrySetValue(this, propName))
                initCount++;
            else
                WriteLine($"Unable to set {propName} to {propValue}");
            //switch (propName)
            //{
            //    case "Time":
            //        Time = aliaser;
            //        initCount++;
            //        break;
            //    case "Filament":
            //        FilamentUsed = aliaser;
            //        initCount++;
            //        break;
            //    case "MinX":
            //        MinX = aliaser;
            //        initCount++;
            //        break;
            //    case "MinY":
            //        MinY = aliaser;
            //        initCount++;
            //        break;
            //    case "MinZ":
            //        MinZ = aliaser;
            //        initCount++;
            //        break;
            //    case "MaxX":
            //        MaxX = aliaser;
            //        initCount++;
            //        break;
            //    case "MaxY":
            //        MaxY = aliaser;
            //        initCount++;
            //        break;
            //    case "MaxZ":
            //        MaxZ = aliaser;
            //        initCount++;
            //        break;
            //    default:
            //        System.Diagnostics.Debug.WriteLine($"unable to assign {propName} to a property.");
            //        break;
            //}
        }
    }
}
