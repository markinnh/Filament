using System;
using System.ComponentModel;
using LiteDB;
using MyLibraryStandard.Attributes;

namespace DataDefinitions.Models;

public class WeightMeasurement : ParentLinkedDataObject<InventorySpool>, IDataErrorInfo
{
    public static event InDataOpsChangedHandler InDataOpsChanged;
    public override bool InDataOperations => InDataOps;
    private static bool inDataOps;
    public static bool InDataOps
    {
        get => inDataOps;
        set
        {
            inDataOps = value;

            // Notify all the InventorySpool objects of change to InDataOps state, allowing them to update the UI.
            InDataOpsChanged?.Invoke(EventArgs.Empty);
        }
    }
    public string this[string columnName] => throw new NotImplementedException();
    [BsonIgnore]
    public string Error => throw new NotImplementedException();
    [BsonIgnore]
    public override bool IsValid => MeasuredOn != default && Weight != default;

    private DateTime measuredOn = DateTime.Today;

    public DateTime MeasuredOn
    {
        get => measuredOn;
        set => Set(ref measuredOn, value);
    }
    private float weight;
    [Affected("AmountUsed", "Remaining")]
    public float Weight
    {
        get => weight;
        set => Set(ref weight, value);
    }
    [BsonIgnore]
    public float AmountUsed => Parent?.OpeningWeight != null ? (float)Parent.OpeningWeight - SpoolWeight - Weight : float.NaN;
    [BsonIgnore]
    public float SpoolWeight => Parent?.OpeningWeight != null ? Parent.EmptySpoolWeight != null ? (float)Parent.EmptySpoolWeight : (float)Parent.OpeningWeight - (float)Parent.Parent.Weight : float.NaN;
    [BsonIgnore]
    public double Remaining => Parent?.OpeningWeight != null ? Math.Round((Weight - SpoolWeight) * 1000) : float.NaN;
}
