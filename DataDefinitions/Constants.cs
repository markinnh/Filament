using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace DataDefinitions
{
    public enum SupportedWeight
    {
        [Description("(g)")]
        Gram,
        [Description("(kg)")]
        Kilogram,
        [Description("(oz)")]
        Ounce,
        [Description("(lb)")]
        Pound
    }
    public enum ConvertToWeight
    {
        [Description("grams")]
        Grams,
        [Description ("milligrams")]
        Millgrams,
        [Description("kilograms")]
        Kilograms,
    }
    public enum SupportedLength
    {
        [Description("millimeter (mm)")]
        Millimeter,
        [Description("centimeter (cm)")]
        Centimeter,
        [Description("meters (m)")]
        Meter,
        [Description("inches (in)")]
        Inches
    }
    public enum ConvertToLength
    {
        [Description("millimeter")]
        Millimeter,
        [Description("centimeter")]
        Centimeter,
        [Description("meter")]
        Meter
    }
    public enum SupportedVolume
    {
        [Description("cubic centimeter")]
        CubicCentimeter,
        [Description("cubic millimeter")]
        CubicMillimeter,
        [Description("cubic inches")]
        CubicInches
    }
    public enum ConvertToVolume
    {
        [Description("Cubic Centimeter")]
        CubicCentimeter,
        [Description("Cubic Millimeter")]
        CubicMillimeter
    }
    public class Constants
    {
        public const double BasicPLADensity = 1.24;
        public const double BasicABSDensity = 1.04;
        public const double BasicPETGDensity = 1.27;
        public const double BasicNylonDensity = 1.04;
        public const double BasicPolycarbonateDensity = 1.19;
        public const double BasicWoodPLA = 1.4;

        // spool size definitions, for spools I have personally used.
        public const double StandardSpoolLoad = 1.0;
        public const double DefaultDrumDiameter = 78;
        public const double DefaultSpoolDiameter = 200;
        public const short HatchBox1KgSpoolOuterDiameter = 199;
        public const short HatchBox1KgSpoolDrumDiameter = 77;
        public const short HatchBox1KgSpoolWidth = 63;
        public const short Solutech1KgSpoolOuterDiameter = 200;
        public const short Solutech1KgSpoolDrumDiameter = 80;
        public const short Solutech1KgSpoolWidth = 55;

        // Vendor names, the list is incomplete        
        public const string _3DSolutechName = "3D Solutech";
        public const string HatchBoxName = "HatchBox";
        public const string SunluName = "Sunlu";
        public const string DefaultVendorName = "Generic";

        // Conversion factors
        public const double CubicInchesToCubicMillimeters = 16387.064;
        public const double CubicInchesToCubicCentimeters = 16.387064;
        public const double CubicMillimetersToCubicCentimeter = .001;
        public const double CubicCentimetersToCubicInches = 0.0610237441;

        
        public const double InchToMillimeters = 25.4;
        public const double FootToMillimeter = 304.8;
        public const double MillimetersToMeters = 1e-3;
        public const double MillimetersToCentimeters = .1;
        public const double MetersToMillimeter = 1e3;
        public const double CentimetersToMillimeters = 10;
        public const double PoundToGram = 453.592;
        public const double PoundToKg = .453592;
        public const double PoundToMilligram = 453592;
        public const double OunceToGram = 28.3495;
        public const double OunceToMilligram = 28349.5;
        public const double GramToOunce = 0.0352739907;
        

        public const double KilogramToMillgram = 1000000;
        public const double GramToMilligram = 1000;
        public const string regexFindFraction = @"(?<whole>\d+)?_?(?<numerator>\d+)/(?<denominator>\d+) *(?<units>[a-zA-Z\x22]+)?";
        public const string regexFindNumberAndUnit = @"(?<number>\d*(\.\d+)?) *(?<units>[a-zA-Z\x22]*)";
        public const string regexARGB = "#(?<alpha>[0-9A-F]{2})(?<red>[0-9A-F]{2})(?<green>[0-9A-F]{2})(?<blue>[0-9A-F]{2})";

        // Settings Key, mainly for database type settings
        public const string dataSeedingKey = "SeedData";
        public const string prepopulateDateTime = "PrepopulateDateTime";
    }
}
