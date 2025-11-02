using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

using System.Collections.ObjectModel;

using MyLibraryStandard.Attributes;
using System.ComponentModel;
using System.Text.Json.Serialization;
using System.Reflection;
using System.Xml.Serialization;
using DataDefinitions.Interfaces;
using LiteDB;
using DataDefinitions.LiteDBSupport;
using static System.Diagnostics.Debug;

namespace DataDefinitions.Models;

[UIHints("Inventory")]
public class InventorySpool : ParentLinkedDataObject<SpoolDefn>, ITag, ITrackUsable, IDeleteSupport, IDataErrorInfo, IEventListener, IFormattable
{
    // TODO: Establish an cumulative counter for the InventorySpool it must be stored in the database 
    // TODO: Make InventorySpool a standalone DatabaseObject, to be stored separately from the VendorDefn - abandoned, too much work for not enough benefit
    // TODO: V4, make the field names smaller where ever possible.  ex: FoundOnAmazon can be shorted to OnAmzn, DepthMeasurements can be shortened to Measurement, or Measures.
    // TODO: also, consider making measures datetime, a timespan from the time the spool was opened, although that can changed by drying the spool.
    //until this data is in a separate table
    public static event InDataOpsChangedHandler InDataOpsChanged;

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
#if DEBUG
    const int MaxMeasurements = 5;
#else
    const int MaxMeasurements = 10;
#endif
    [JsonIgnore, BsonIgnore]
    public override bool InDataOperations => inDataOps;
    [JsonIgnore, BsonIgnore]
    public override bool IsModified
    {
        get => base.IsModified || DepthMeasurements.Cast<ITrackModified>().Any(dm => dm.IsModified)||WeightMeasurements.Cast<ITrackModified>().Any(wm=>wm.IsModified); set => base.IsModified = value;
    }
    [JsonIgnore, BsonIgnore]
    public override bool IsValid => FilamentDefn != null && FilamentColor != null && DepthMeasurements?.Where(dm => !dm.IsValid).Count() == 0;
    //[JsonIgnore, BsonIgnore]
    //public override bool SupportsDelete => true;
    //private int inventorySpoolId;
    ////public override bool InDatabase => InventorySpoolId != default;
    //[XmlAttribute("ID"), JsonPropertyName("ID"), BsonField("ID")]
    //public int InventorySpoolId
    //{
    //    get => inventorySpoolId;
    //    set
    //    {
    //        if (inventorySpoolId != value)
    //        {
    //            inventorySpoolId = value;
    //            if (inventorySpoolId != default)
    //                foreach (var dm in DepthMeasurements)
    //                    dm.InventorySpoolId = inventorySpoolId;
    //        }
    //    }
    //}
    private int inventorySpoolId;
    [BsonField("itemCount")]
    public int InventoryCount
    {
        get => inventorySpoolId;
        set
        {
            if (Set<int>(ref inventorySpoolId, value))
            {
            }
        }
    }
    [XmlAttribute("filamentID"), BsonField("filamentID")]
    public int FilamentDefnId
    {
        get; set;
    }

    private FilamentDefn filamentDefn;
    [Affected(Names = new string[] { nameof(IsValid) }), JsonIgnore, XmlIgnore, BsonIgnore]
    public FilamentDefn FilamentDefn
    {
        get => filamentDefn;
        set
        {
            if (Set(ref filamentDefn, value) && filamentDefn != null)
                FilamentDefnId = filamentDefn.FilamentDefnId;
        }
    }
    //[XmlAttribute("spoolID"), BsonIgnore]
    //public int SpoolDefnId { get; set; }
    //private SpoolDefn spoolDefn;
    //[JsonIgnore, XmlIgnore, BsonIgnore]
    //public SpoolDefn SpoolDefn
    //{
    //    get => spoolDefn;
    //    set
    //    {
    //        if (Set<SpoolDefn>(ref spoolDefn, value) && spoolDefn != null)
    //            SpoolDefnId = spoolDefn.SpoolDefnId;
    //    }
    //}

    private string colorName;
    [XmlAttribute("color")]
    public string ColorName
    {
#pragma warning disable CS8603 // Possible null reference return.
        get => colorName;
#pragma warning restore CS8603 // Possible null reference return.
#pragma warning disable CS8601 // Possible null reference assignment.
        set => Set<string>(ref colorName, value);
#pragma warning restore CS8601 // Possible null reference assignment.
    }

    private FilamentColor filamentColor;
    [BsonRef]
    public FilamentColor FilamentColor
    {
        get => filamentColor;
        set
        {
            if (Set(ref filamentColor, value))
                FilamentColorId = FilamentColor.FilamentColorId;
        }
    }

    private int filamentColorId;

    public int FilamentColorId
    {
        get => filamentColorId;
        set => Set(ref filamentColorId, value);
    }

    private DateTime dateOpened = DateTime.Today;
    [XmlAttribute("opened"), JsonPropertyName("opened"), BsonField("opened")]
    public DateTime DateOpened
    {
        get => dateOpened;
        set => Set(ref dateOpened, value);
    }
    private bool? stopUsing;
    [XmlAttribute("notUsed"), JsonPropertyName("Used")]
    public bool? StopUsing
    {
        get => stopUsing;
        set => Set(ref stopUsing, value);
    }
    private float? openingWeight;
    [BsonField("OpenWgt")]
    public float? OpeningWeight
    {
        get => openingWeight;
        set => Set(ref openingWeight, value);
    }

    private float? emptySpoolWeight;
    [BsonField("MTSplWgt")]
    public float? EmptySpoolWeight
    {
        get => emptySpoolWeight;
        set => Set(ref emptySpoolWeight, value);
    }


    [JsonIgnore, BsonIgnore]
    public int AgeInDays => (DateTime.Today - DateOpened).Days;
    [JsonIgnore, BsonIgnore]
    public string Name => $"{ColorName}";
    [JsonIgnore, BsonIgnore]
    public string VendorName => Parent?.Parent?.Name ?? "Undefined";
    [JsonIgnore, BsonIgnore]
    public string SpoolDescription => Parent?.Description ?? "Undefined";
    [JsonIgnore, BsonIgnore]
    public bool CanWeight => OpeningWeight != null;
    public double CalcInitialDepth()
    {
        const double utilizationFactor = 0.95;
        const double layerOverlap = .95;
        var initialLayer = true;
        if (Parent != null && FilamentDefn != null)
        {
            var initialLength = InitialLength;
            if (!double.IsNaN(initialLength))
            {
                var depth = (Parent.SpoolDiameter - Parent.DrumDiameter) / 2 + (FilamentDefn.Diameter / 2);
                double length = 0;
                do
                {
                    var turns = Parent.SpoolWidth / FilamentDefn.Diameter * utilizationFactor;
                    var windLength = (Parent.SpoolDiameter - depth * 2) * Math.PI / 1000;
                    var amountAdded = turns * windLength;
                    length += amountAdded;
                    depth -= FilamentDefn.Diameter * (initialLayer ? 1.0 : layerOverlap);
                    initialLayer = false;
                } while (depth > 0 && length < initialLength);
                return Math.Round(depth, 3);
            }
            return double.NaN;
        }
        else
            return double.NaN;
    }
    [JsonIgnore, BsonIgnore]
    public double InitialLength
    {
        get
        {
            if (Parent != null && FilamentDefn != null)
            {
                return FilamentMath.LengthFromWeightBasedOnDensity(FilamentDefn.Density, Parent.Weight * 1000, FilamentDefn.Diameter) / 1000;

            }
            else
                return double.NaN;
        }
    }
    [BsonIgnore]
    public double GramsRemaining
    {
        get
        {
            if (DepthMeasurements.OrderBy(dm => dm.MeasureDateTime).FirstOrDefault() is DepthMeasurement depthMeasurement)
            {
                return depthMeasurement.FilamentRemainingInGrams;
            }
            else
                return Parent.Weight * 1000;
            //var lastMeasurement = DepthMeasurements.OrderBy(dm => dm.MeasureDateTime).FirstOrDefault();
            //if (lastMeasurement == null)
            //    return 100;
            //else
            //{
            //    return Math.Round(lastMeasurement.FilamentRemainingInGrams / (SpoolDefn.Weight * 1000) * 100, 3);
            //}
        }
    }
    [BsonIgnore]
    public double MaxDepth => Parent != null ? Parent.MaxDepth : double.NaN;
    //private string? ignoreThis="Ignore This";
    //[NotMapped]
    //public string IgnoreThis
    //{
    //    get => ignoreThis="Ignore This";
    //    set => Set<string>(ref ignoreThis, value);
    //}
    [BsonIgnore]
    public int MaxAllowed => MaxMeasurements;
    [BsonIgnore]
    public bool CanAdd => DepthMeasurements?.Count < MaxMeasurements;
    public virtual ObservableCollection<DepthMeasurement> DepthMeasurements
    {
        get; set;
    }
    public virtual ObservableCollection<WeightMeasurement> WeightMeasurements
    {
        get; set;
    }
    public string Error => null;

    public string this[string columnName]
    {
        get
        {
            throw new NotImplementedException();
        }
    }
    internal override void DeleteMe(DataObject dataObject, ILiteDBDAL lite)
    {
        //Assert(this != dataObject, "Can't delete yourself");
        if (lite !=null)
        {
            var parentRef = this.Parent;
            //lite.Remove(this);
            parentRef.DeleteMe(this, lite);
        }
        //dataObject = null;

    }
    // if this is used in the wpf app it will need to exist in some form, probably for the UWP app,
    // BindableInventorySpool will have to be inherited from InventorySpool
    //[NotMapped]
    //public IEnumerable<FilamentDefn> Filaments => GetFilaments();

    //protected IEnumerable<FilamentDefn> GetFilaments()
    //{
    //    if (Singleton<DataLayer>.Instance.GetFilteredSettings(se => se.Name == "SelectShowFlag").SingleOrDefault() is Setting setting)
    //    {
    //        if (setting.Value == "ShowAll")
    //            return Singleton<DataLayer>.Instance.FilamentList;
    //        else
    //            return Singleton<DataLayer>.Instance.GetFilteredFilaments(fi => !fi.StopUsing);
    //    }
    //    else
    //        return Singleton<DataLayer>.Instance.FilamentList;
    //}
    public InventorySpool()
    {
        DepthMeasurements = new ObservableCollection<DepthMeasurement>();

        WeightMeasurements = new ObservableCollection<WeightMeasurement>();

        InitEventHandlers();
        InDataOpsChanged += InventorySpool_InDataOpsChanged;
    }
    public void InitEventHandlers()
    {
        InitEventHandlers(DepthMeasurements);
        InitEventHandlers(WeightMeasurements);
    }
    private void InitEventHandlers<T>(ObservableCollection<T> Measurement)
    {
        Measurement.CollectionChanged += Measurement_CollectionChanged;
    }
    public override void PostDataRetrieveActions()
    {
        InitEventHandlers(DepthMeasurements);
    }
    //public override void EstablishLink(IJsonFilamentDocument document)
    //{
    //    base.EstablishLink(document);
    //    InitEventHandlers(DepthMeasurements);
    //}

    private void HandleMeasurementPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        //throw new NotImplementedException();
    }
    public override void UpdateItem(LiteDBDal dal, bool holdContainerUpdate = false)
    {

        Parent.UpdateItem(dal, holdContainerUpdate);
    }
    internal override void PrepareToSave(LiteDBDal dBDal)
    {
        if (InventoryCount == default)
            AssignInventoryCount(dBDal);
    }
    protected void AssignInventoryCount(LiteDBDal dal)
    {
        var setting = dal.Settings.FirstOrDefault(d => d.Name.Contains("InventorySpoolCounter"));
        if (setting != null)
        {
            setting.SetValue(setting + 1);
            InventoryCount = setting;
            dal.Update(setting);
        }
        else
        {
            InventoryCount = 1;
            setting = new Setting("InventorySpoolCounter", 1);
            dal.SmartAdd(setting);
        }
    }
    internal override void WatchContained()
    {
        foreach (var dm in DepthMeasurements)
            dm.Subscribe(WatchContainedHandler);
    }
    internal override void UnWatchContained()
    {
        foreach (var dm in DepthMeasurements)
            dm.Unsubscribe(WatchContainedHandler);
    }
    protected override void WatchContainedHandler(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(IsModified))
        {
            OnPropertyChanged(e.PropertyName);
            OnPropertyChanged(nameof(IsValid));
        }
        else
            base.WatchContainedHandler(sender, e);
    }
    //public override string UIHintAddType() => typeof(DepthMeasurement).GetCustomAttribute<UIHintsAttribute>()?.AddType ?? string.Empty;
    private void Measurement_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add && e.NewItems != null)
        {
            foreach (var item in e.NewItems)
            {
                if (item is DepthMeasurement measurement)
                {
                    measurement.Parent = this;
                    if (!DepthMeasurement.InDataOps)
                    {
                        //measurement.InventorySpoolId = InventorySpoolId;
                        ///<remarks>
                        /// prevents changing depth if not the initial values
                        ///</remarks>
                        if (measurement.Depth1 == DepthMeasurement.FilamentStartingDepth)
                            measurement.Depth1 = CalcInitialDepth();
                        if (measurement.Depth2 == DepthMeasurement.FilamentStartingDepth)
                            measurement.Depth2 = CalcInitialDepth();
                    }
                    measurement.Subscribe(WatchContainedHandler);
                }
                else if (item is WeightMeasurement weightMeasurement)
                {
                    weightMeasurement.Parent = this;
                    weightMeasurement.Subscribe(WatchContainedHandler);
                }
                OnPropertyChanged(nameof(IsModified));
            }
            IsModified = true;
        }
        else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            IsModified = true;
        OnPropertyChanged(nameof(CanAdd));
        //throw new NotImplementedException();
    }

    public void ReleaseEventHandlers()
    {
        InDataOpsChanged -= InventorySpool_InDataOpsChanged;

        if (DepthMeasurements is ObservableCollection<DepthMeasurement> measurements)
            measurements.CollectionChanged -= Measurement_CollectionChanged;
        if(WeightMeasurements is ObservableCollection<WeightMeasurement> weightMeasurements)
            weightMeasurements.CollectionChanged -= Measurement_CollectionChanged; 
        UnWatchContained();
    }
    ~InventorySpool()
    {

    }
    private void InventorySpool_InDataOpsChanged(EventArgs args)
    {
        OnPropertyChanged(nameof(CanEdit));
        //throw new NotImplementedException();
    }
    //public override void UpdateItem<TContext>()
    //{
    //    if (IsValid)
    //    {
    //        using (TContext context = new TContext())
    //        {
    //            int expectedUpdateCount = 0;

    //            expectedUpdateCount += context.SetDataItemsState(DepthMeasurements.Where(dm => Added(dm)), Microsoft.EntityFrameworkCore.EntityState.Added);
    //            expectedUpdateCount += context.SetDataItemsState(DepthMeasurements.Where((dm) => Modified(dm)), Microsoft.EntityFrameworkCore.EntityState.Modified);

    //            if (InDatabase)
    //                context.Update(this);
    //            else
    //                context.Add(this);

    //            var changeCount = context.SaveChanges();

    //        }
    //        SetContainedModifiedState(false);
    //    }
    //}
    //internal override void UpdateContainedItemEntryState<TContext>(TContext context)
    //{
    //    context.SetDataItemsState(DepthMeasurements.Where(dm => Added(dm)), Microsoft.EntityFrameworkCore.EntityState.Added);
    //    context.SetDataItemsState(DepthMeasurements.Where(dm => Modified(dm)), Microsoft.EntityFrameworkCore.EntityState.Modified);
    //}
    //public override void LinkChildren<ParentType>(ParentType parent)
    //{
    //    if (parent is Models.SpoolDefn spDefn)
    //    {
    //        SpoolDefn = spDefn;
    //        foreach (var dm in DepthMeasurements)
    //            dm.LinkChildren(this);
    //    }
    //    //base.LinkChildren(parent);
    //}
    public override void SetContainedModifiedState(bool state)
    {

        foreach (var dm in DepthMeasurements)
            dm.IsModified = state;
        IsModified = state;
    }

    public void Link(IEnumerable<FilamentDefn> filaments)
    {
        if (FilamentDefnId != default)
        {
            FilamentDefn = filaments.Single(fil => fil.FilamentDefnId == FilamentDefnId);
        }
    }
    internal override void LinkToParent(SpoolDefn parent)
    {
        base.LinkToParent(parent);
        foreach (var dm in DepthMeasurements)
            dm.LinkToParent(this);
        foreach (var wm in WeightMeasurements)
            wm.LinkToParent(this);
        InitEventHandlers();
    }
    public void Delete(ILiteDBDAL lite)
    {
        ReleaseEventHandlers();
        DeleteMe(this, lite);
    }
    private string _tags;
    public string Tags
    {
        get => _tags; set => Set(ref _tags, value);
    }

    public IEnumerable<string> GetTags()
    {
        if (!string.IsNullOrEmpty(Tags))
            return Tags.Split("#", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        else
            return null;
    }

    public string ToString(string format, IFormatProvider formatProvider)
    {
        var specifier = string.IsNullOrEmpty(format) ? "N" : format;
        switch (specifier)
        {
            case "N":
                return $"{DateOpened:d} {FilamentColor?.VendorColorName} {this.FilamentDefn?.MaterialType}";
            case "S":
                return $"{FilamentColor.VendorColorName} {FilamentDefn.MaterialType}";
            case "CG":
                StringBuilder builder = new StringBuilder();
                builder.Append("new InventorySpool() {");
                builder.Append($"DateOpened = new DateTime({DateOpened.Year},{DateOpened.Month},{DateOpened.Day})");

                return $"Format specifier '{specifier}' not implemented for {GetType().Name}";
            default:
                return $"Format specifier '{specifier}' not implemented for {GetType().Name}";
        }
    }

    //internal override int KeyID
    //{
    //    get => InventorySpoolId;
    //    set
    //    {
    //        bool assignChildren = !InDatabase && InventorySpoolId != value;
    //        if (assignChildren)
    //            InventorySpoolId = value;

    //        foreach (var item in DepthMeasurements)
    //        {
    //            if (assignChildren)
    //                item.InventorySpoolId = value;
    //            //if (!item.InDatabase)
    //            //    item.AssignKey(Document.Counters.NextID(item.GetType()));
    //        }
    //    }

    //}
    //internal override void UpdateContainedItemsForJson()
    //{
    //    if (!InDatabase)
    //        AssignKey(Document.Counters.NextID(this));
    //    //foreach (var item in DepthMeasurements)
    //    //{
    //    //    if ( item.IsValid)
    //    //        item.AssignKey(Document.Counters.NextID(item));
    //    //}
    //}
    #region IEditableObject Implementation
    /*        struct BackupData
            {
                public string ColorName { get; set; }
                public DateTime DateOpened { get; set; }
                public int FilamentDefnId { get; set; }
                public FilamentDefn FilamentDefn { get; set; }
                internal BackupData(InventorySpool inventorySpool)
                {
                    ColorName = inventorySpool.ColorName;
                    DateOpened = inventorySpool.DateOpened;
                    FilamentDefnId = inventorySpool.FilamentDefnId;
                    FilamentDefn = inventorySpool.FilamentDefn;
                }
            }
            BackupData backupData;
            void IEditableObject.BeginEdit()
            {
                if (!InEdit)
                {
                    backupData = new BackupData(this);
                    InEdit = true;
                }
                //throw new NotImplementedException();
            }

            void IEditableObject.CancelEdit()
            {
                if (InEdit)
                {
                    ColorName = backupData.ColorName;
                    DateOpened = backupData.DateOpened;
                    FilamentDefnId = backupData.FilamentDefnId;
                    FilamentDefn = backupData.FilamentDefn;
                    SetContainedModifiedState(false);
                    backupData = default(BackupData);
                    InEdit = false;
                }
                //throw new NotImplementedException();
            }

            void IEditableObject.EndEdit()
            {
                if (InEdit)
                {
                    backupData = default(BackupData);
                    InEdit = false;
                }
                //throw new NotImplementedException();
            }
    */
    #endregion
}
