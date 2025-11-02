using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using json = System.Text.Json.Serialization;
using json1 = System.Text.Json;
using System.Threading.Tasks;
using DataDefinitions.Interfaces;
using LiteDB;
using DataDefinitions.LiteDBSupport;
using MyLibraryStandard.Data;

namespace DataDefinitions.Models;

public class FilamentColor : TaggedDatabaseObject, IFormattable, ISharingSupport
#if SUPPORTDELETE
    , IDeleteSupport
#endif
{
    #region Sharing Support
    internal const string FilamentColorIdentifier = "D2B71E3C-1D3A-4F0C-8F1D-2E8F0C8F1D2E";
    internal static Guid identifier = new(FilamentColorIdentifier);
    [json.JsonIgnore, BsonIgnore]
    public Guid SharingID => identifier;

    private bool isSelected;
    [json.JsonIgnore, BsonIgnore]
    public bool IsSelectedForSharing
    {
        get => isSelected;
        set => Set(ref isSelected, value);
    }
    private bool alreadyInDatabase;
    [json.JsonIgnore, BsonIgnore]
    public bool NotAlreadyInDatabase
    {
        get => alreadyInDatabase;
        set => Set(ref alreadyInDatabase, value);
    }

    public string SerializeForSharing()
    {
        return json1.JsonSerializer.Serialize(this);
    }
    public string SuggestedFileNameForSharing()
    {
        var safeName = string.Concat(VendorColorName.Split(System.IO.Path.GetInvalidFileNameChars()));
        return $"{safeName}_{DateTime.Now:yyyyMMddHHmmss}.json";
    }
    #endregion

    public static FilamentColor Default = new() { VendorColorName = "Ice Blue", ColorRGB = "FFA3D8E1" };

    #region DataOps
    private static bool inDataOps;
    public static bool InDataOps
    {
        get => inDataOps;
        set => inDataOps = value;
    }
    [json.JsonIgnore, BsonIgnore]
    public override bool InDataOperations => InDataOps;
    #endregion

    #region Editing and Validation
    [json.JsonIgnore, BsonIgnore]
    public override bool CanEdit => base.CanEdit;
    [json.JsonIgnore, BsonIgnore]
    public override bool InDatabase => FilamentColorId != default;
    [json.JsonIgnore, BsonIgnore]
    public override bool IsValid => !string.IsNullOrEmpty(ColorRGB) && !string.IsNullOrEmpty(VendorColorName);

    #endregion

    #region Actual Data
    [json.JsonIgnore]
    public int FilamentColorId
    {
        get;
        set;
    }
    private string vendorColorName = "Ice Blue";

    public string VendorColorName
    {
        get => vendorColorName;
        set => Set(ref vendorColorName, value);
    }
    private string colorRGB = "#FFA3D8E1";

    public string ColorRGB
    {
        get => colorRGB;
        set => Set(ref colorRGB, value);
    }
    #endregion

    public override bool HasDependencies(ILiteDBDAL lite)
    {
        if (lite == null)
            throw new ArgumentNullException(nameof(lite), $"A null reference for {nameof(ILiteDBDAL)} was passed into HasDependencies in {GetType().Name}.");
        if (lite.Vendors != null && lite.Vendors.Count > 0)
            
            foreach (var vend in lite.Vendors)
            {
                if (vend.SpoolDefns.Count == 0)
                    continue;
                foreach (var sp in vend.SpoolDefns)
                {
                    if (sp.Inventory.Count == 0)
                        continue;
                    foreach (var inv in sp.Inventory)
                        if (inv.FilamentColorId == FilamentColorId)
                            return true;
                }
            }
        return false;
    }
#if SUPPORTDELETE
    public void Delete(ILiteDBDAL lite)
    {
        if (lite == null)
            throw new ArgumentNullException(nameof(lite), $"A null reference was passed for the {nameof(ILiteDBDAL)} in Delete for {GetType().Name}.");
        if (lite is LiteDBDal dal)
        {
            if (HasDependencies(lite))
                throw new InvalidOperationException("This color cannot be deleted as it is assigned to inventory items.");

            dal.FilamentColors.Remove(this);
            dal.Remove(this);
            //lite.FilamentColors.Remove(this);
        }
        else
            throw new ArgumentException("The ILiteDBDAL passed was not of the correct type.", nameof(lite));
    }
#endif
    // Replace the method signature to implement IFormattable instead of trying to override a non-existent base method.
    public string ToString(string format = Constants.EmptyString, IFormatProvider formatProvider = null)
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

    // Optionally, you may want to override the parameterless ToString for convenience:
    public override string ToString()
    {
        return ToString("N", null);
    }
}
