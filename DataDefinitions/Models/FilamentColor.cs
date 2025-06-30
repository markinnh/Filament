using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using LiteDB;

namespace DataDefinitions.Models;
public class FilamentColor:DatabaseObject,IFormattable
{
    public static FilamentColor Default = new () { VendorColorName = "Ice Blue", ColorRGB = "FFA3D8E1" };
    private static bool inDataOps;
    public static bool InDataOps
    {
        get => inDataOps;
        set => inDataOps = value;
    }
    [JsonIgnore, BsonIgnore]
    public override bool InDataOperations => InDataOps;
    [JsonIgnore, BsonIgnore]
    public override bool CanEdit => base.CanEdit;
    [JsonIgnore, BsonIgnore]
    public override bool InDatabase => FilamentColorId != default;
    [JsonIgnore, BsonIgnore]
    public override bool IsValid => !string.IsNullOrEmpty(ColorRGB) && !string.IsNullOrEmpty(VendorColorName);
    public int FilamentColorId
    {
        get;
        set;
    }
    private string vendorColorName;

    public string VendorColorName
    {
        get => vendorColorName;
        set => Set(ref vendorColorName, value);
    }
    private string colorRGB= "#FFA3D8E1";

    public string ColorRGB
    {
        get => colorRGB;
        set => Set(ref colorRGB, value);
    }

    public string ToString(string format, IFormatProvider formatProvider)
    {
        var specifier = string.IsNullOrEmpty(format) ? "N" : format;
        switch (specifier)
        {
            case "N":
                return $"{VendorColorName}";
            case "S":
                return $"{ColorRGB}";
            case "F":
                return $"{VendorColorName} - (0x{ColorRGB})";
            case "CG":
                return $"new FilamentColor() {{ VendorColorName={VendorColorName.DoQuote()}, ColorRGB={ColorRGB.DoQuote()} }}";
            default:
                return $"{specifier} is not implemented";
        }
        
    }
}
