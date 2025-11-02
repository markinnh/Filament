
using DataDefinitions.Interfaces;
using DataDefinitions.LiteDBSupport;
using LiteDB;
using MyLibraryStandard.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using json = System.Text.Json;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace DataDefinitions.Models;

public enum MaterialModifier
{
    None = 0,
    Matte = 1,
    Silk = 2,
    Tough = 4,
    HighFlow = 8,
    CarbonFiber = 16
}
// this is called from the static part of a class, so there will be no sender provided,
// all it currently does is say one of the objects is undergoing a database operation of 
// some sort.
public delegate void InDataOpsChangedHandler(EventArgs args);
/// <summary>
/// Allows for definition of 3d print filament. Default FilamentDefn is Generic PLA
/// </summary>
public class FilamentDefn : DataDefinitions.DatabaseObject, ITrackUsable, ISupportRemoval, IEventListener, ISharingSupport
#if SUPPORTDELETE
    ,IDeleteSupport
#endif//,IEditableObject
{
    internal const string FilamentDefnIdentifier = "A1B2C3D4-E5F6-4789-ABCD-EF0123456789";

    /// <summary>
    /// Standard filament diameter in mm
    /// </summary>
    public const double StandardFilamentDiameter = 1.75;
    /// <summary>
    /// Large filament diameter in mm
    /// </summary>
    public const double LargeFilamentDiameter = 3.0;
    /// <summary>
    /// Default shelf-life in days
    /// </summary>
    public const int DefaultShelfLife = 180;  // in days

    #region Sharing Support
    internal static Guid identifier = new(FilamentDefnIdentifier);
    [JsonIgnore, BsonIgnore]
    public Guid SharingID => identifier;
    private bool isSelected;
    [JsonIgnore, BsonIgnore]
    public bool IsSelectedForSharing
    {
        get => isSelected;
        set => Set(ref isSelected, value);
    }
    [JsonIgnore, BsonIgnore]
    private bool alreadyInDatabase;
    [JsonIgnore, BsonIgnore]
    public bool NotAlreadyInDatabase
    {
        get => alreadyInDatabase;
        set => Set(ref alreadyInDatabase, value);
    }

    public string SerializeForSharing()
    {
        json.JsonSerializerOptions options = new()
        {
            WriteIndented = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            Converters = { new JsonStringEnumConverter() }
        };
        return json.JsonSerializer.Serialize(this, typeof(FilamentDefn), options);
    }
    public string SuggestedFileNameForSharing()
    {
        var safeName = string.Join('-', Material?.ShortName, Vendor?.Name ?? Enhancement).Split(System.IO.Path.GetInvalidFileNameChars());
        return $"{safeName}_{DateTime.Now:yyyyMMddHHmmss}.json";
    }
    #endregion

    #region Data Operations State Management
    public static event InDataOpsChangedHandler InDataOpsChanged;

    private static bool inDataOps;
    public static bool InDataOps
    {
        get => inDataOps;
        set
        {
            inDataOps = value;

            // Notify all the FilamentDefn objects of change to InDataOps state, allowing them to update the UI.
            InDataOpsChanged?.Invoke(EventArgs.Empty);
        }
    }
    [JsonIgnore, BsonIgnore]
    public override bool InDataOperations => InDataOps;
    #endregion

    #region Editing and Validation
    [JsonIgnore, BsonIgnore]
    public override bool CanEdit => base.CanEdit /* && !isIntrinsic */;
    [JsonIgnore, BsonIgnore]
    public override bool InDatabase => FilamentDefnId != default;
    [JsonIgnore, BsonIgnore]
    public override bool SupportsDelete => !IsIntrinsic;  // TODO: Modify this to check for dependencies too.  Actually, it checks for Dependencies in the Remove method in the Browser ViewModel.  FilamentDefn does not have direct access to the database.
    [XmlIgnore, JsonIgnore, BsonIgnore]
    public override bool IsModified
    {
        get => base.IsModified; set => base.IsModified = value;
    }
    [JsonIgnore, BsonIgnore]
    public override bool IsValid => !double.IsNaN(Diameter) && MaterialId != default;
    #endregion

    #region Actual Data
    /// <summary>
    /// Database identifier for FilamentDefn
    /// </summary>
    [JsonIgnore, BsonId]
    public int FilamentDefnId
    {
        get; set;
    }

    private double diameter = StandardFilamentDiameter;
    /// <summary>
    /// Gets or sets the diameter.
    /// </summary>
    /// <value>
    /// The diameter.
    /// </value>
    [Affected(Names = new string[] { nameof(MgPerMM), nameof(IsValid) }), XmlAttribute("diameter")]
    public double Diameter
    {
        get => diameter;
        set => Set<double>(ref diameter, value);
    }

    private bool? stopUsing;
    /// <summary>
    /// Gets or sets a value indicating whether to stop using.
    /// </summary>
    /// <value>
    ///   <c>true</c> if [stop using]; otherwise, <c>false</c>.
    /// </value>
    [XmlAttribute("notUsed"), JsonIgnore]
    public bool? StopUsing
    {
        get => stopUsing ?? false;
        set => Set(ref stopUsing, value);
    }

    //public bool EventsMapped => (InDataOpsChanged?.GetInvocationList().Cast<InDataOpsChangedHandler>().Contains(FilamentDefn_InDataOpsChanged) ?? false) && (DensityAlias?.IsLinkedToNotifyContainer ?? false);
    //private double density = DefinedDensity.BasicPLADensity;
    ///// <summary>
    ///// Gets or sets the filament density.  Expected in gm/cc, default is 1.24 for generic PLA.
    ///// </summary>
    ///// <value>
    ///// The filament density.
    ///// </value>
    //public double MeasuredDensity { get => DensityCalc?.DensityInGmPerCC ?? density; set => Set(ref density, value, nameof(Filament_Data.MeasuredDensity)); }
    private MaterialType materialType = MaterialType.PLA;
    /// <summary>
    /// Gets or sets the type of the material.
    /// </summary>
    /// <value>
    /// The type of the material.
    /// </value>
    [Affected(Names = new string[] { nameof(MgPerMM) }), JsonIgnore]
    public MaterialType MaterialType
    {
        get => materialType; set => Set<MaterialType>(ref materialType, value);
    }

    private string enhancement;

    public string Enhancement
    {
        get => enhancement;
        set => Set(ref enhancement, value);
    }

    private MaterialModifier modifier;
    /// <summary>
    /// Modifiers, as 3d printing has evolved so has the types of filaments available
    /// </summary>
    [JsonIgnore, BsonIgnore]
    public MaterialModifier Modifier
    {
        get => modifier;
        set => Set(ref modifier, value);
    }

    private int? vendorID;
    /// <summary>
    /// Allows for a vendor specific density for various filaments, Bambu PLA as an example
    /// </summary>
    [JsonIgnore]
    public int? VendorID
    {
        get => vendorID;
        set
        {
            var needsUpdate = value != null && value != default && !EqualityComparer<int?>.Default.Equals(value, vendorID);
            Set(ref vendorID, value);
            if (needsUpdate && Singleton<LiteDBDal>.Instance.Vendors != null)
                Vendor = Singleton<LiteDBDal>.Instance.Vendors.Where(v => v.VendorDefnId == vendorID).FirstOrDefault();
        }
    }
#nullable enable
    private string? vendorName;
    [BsonIgnore]
    public string? VendorName
    {
        get => vendorName;
        set => vendorName = value;
    }
#nullable disable
    private VendorDefn vendor;
    [BsonIgnore, JsonIgnore]
    public VendorDefn Vendor
    {
        get => vendor;
        set
        {
            if (Set(ref vendor, value))
                VendorName = Vendor?.Name;
        }
    }

    private double density;
    /// <summary>
    /// Reference to contained DensityAlias
    /// </summary>
    [Affected(Names = new[] { nameof(MgPerMM) }), JsonPropertyName("Density")]
    public double Density
    {
        get => density;
        set => Set(ref density, value);
    }
    private bool isIntrinsic;

    /// <summary>
    /// Determines whether object can be 'edited'
    /// </summary>
    /// <remarks>Currently only set in the database</remarks>
    [XmlAttribute("intrinsic"), JsonPropertyName("Intrinsic")]
    public bool IsIntrinsic
    {
        get => isIntrinsic;
        set => Set<bool>(ref isIntrinsic, value);
    }
    [JsonIgnore]
    public int MaterialId
    {
        get;
        set;
    }

    private Material material;
    [BsonIgnore]
    public Material Material
    {
        get => material;
        set
        {
            if (Set(ref material, value) && MaterialId == default && Material != null)
                MaterialId = Material.MaterialId;
        }
    }

    #endregion

    #region UI Assist Items, not mapped
    /// <summary>
    /// flag used by the UI to <b>show</b> or <b>hide</b> features
    /// </summary>
    //[JsonIgnore, BsonIgnore]
    //public bool MeasuredDensityVisible => DensityAlias?.DensityType == DensityType.Measured;
    //[JsonIgnore, BsonIgnore]
    //public bool DefinedDensityVisible => DensityAlias?.DensityType == DensityType.Defined;
    #endregion
    //private void DensityAlias_NotifyContainer(object sender, MyLibraryStandard.NotifyContainerEventArgs e)
    //{
    //    foreach (var item in e.Names)
    //    {
    //        OnPropertyChanged(item);
    //    }
    //    //throw new NotImplementedException();
    //}
    public void Init()
    {
        if (!InDataOpsChanged?.GetInvocationList().Cast<InDataOpsChangedHandler>().Contains(FilamentDefn_InDataOpsChanged) ?? true)
            InDataOpsChanged += FilamentDefn_InDataOpsChanged;

    }

    private void FilamentDefn_InDataOpsChanged(EventArgs args)
    {
        OnPropertyChanged(nameof(CanEdit));
        //throw new NotImplementedException();
    }
    //internal override void WatchContained()
    //{
    //    //DensityAlias?.Subscribe(WatchContainedHandler);
    //    //DensityAlias?.WatchContained();
    //}
    //internal override void UnWatchContained()
    //{
    //    //DensityAlias?.Unsubscribe(WatchContainedHandler);
    //    //DensityAlias?.UnWatchContained();
    //}
    //protected override void WatchContainedHandler(object sender, PropertyChangedEventArgs e)
    //{
    //    base.WatchContainedHandler(sender, e);
    //    switch (e.PropertyName)
    //    {
    //        //case nameof(Models.DensityAlias.DensityType):
    //        //    var names = new string[] { nameof(MgPerMM)};
    //        //    foreach (var name in names)
    //        //        OnPropertyChanged(name);
    //        //    break;
    //        case nameof(Models.DensityAlias.Density):
    //            OnPropertyChanged(nameof(MgPerMM));
    //            break;
    //    }
    //}
    //public void InitNotificationHandler()
    //{
    //    //if (densityAlias != null)
    //    //    densityAlias.Subscribe(DensityAlias_NotifyContainer);
    //    WatchContained();
    //}
    //public void ReleaseNotificationHandler()
    //{
    //    //if (densityAlias != null)
    //    //    densityAlias.UnSubscribe(DensityAlias_NotifyContainer);
    //    UnWatchContained();
    //}
    public void ReleaseEventHandlers()
    {
        //ReleaseNotificationHandler();

        InDataOpsChanged -= FilamentDefn_InDataOpsChanged;
    }
#if SUPPORTDELETE
    public void Delete(ILiteDBDAL dBDal)
    {
        if (dBDal == null)
            throw new ArgumentNullException(nameof(dBDal), $"the {nameof(dBDal)} argument was not passed to the {nameof(Delete)} method for {GetType().Name}");
        if (FilamentDefnId != default)
        {
            dBDal.Filaments.Remove(this);
            dBDal.Remove(this);
            FilamentDefnId = default;
            InDataOps = false;
            SetContainedModifiedState(false);
            ReleaseEventHandlers();
        }
    }
#endif
    ~FilamentDefn()
    {
        //if (densityAlias != null)
        //    densityAlias.UnSubscribe(DensityAlias_NotifyContainer);

        //InDataOpsChanged -= FilamentDefn_InDataOpsChanged;
    }

    // TODO: Fix before serialization works.  This cannot be an object, it must be a defined type or json serialization will fail.  Its probably a bad programming practice too.  
    //private MeasuredDensity densityCalc;
    ///// <summary>
    ///// Will be set if an actual density calculation is performed
    ///// </summary>
    //public MeasuredDensity DensityCalc { get => densityCalc; set => Set<MeasuredDensity>(ref densityCalc, value, nameof(DensityCalc)); }

    private int shelfLife = DefaultShelfLife;
    /// <summary>
    /// Gets or sets the shelf life in days.
    /// </summary>
    /// <value>
    /// The shelf life in days.
    /// </value>
    [JsonIgnore, XmlAttribute("shelfLife")]
    public int ShelfLifeInDays
    {
        get => shelfLife; set => Set<int>(ref shelfLife, value);
    }
    /// <summary>
    /// Initializes a new instance of the <see cref="FilamentDefn"/> class.
    /// </summary>
    /// <param name="diameter">The diameter.</param>
    /// <param name="density">The density.</param>
    /// <param name="materialType">Type of the material.</param>
    //public FilamentDefn(double diameter, double density, MaterialType materialType)
    //{
    //    Init();
    //    Diameter = diameter;
    //    //MeasuredDensity = density;
    //    DensityUnion = new DefinedDensity(density);
    //    MaterialType = materialType;
    //}
    /// <summary>
    /// Initializes a new instance of the <see cref="FilamentDefn"/> class.  Basically defining as a standard PLA filament
    /// </summary>
    public FilamentDefn()
    {
        Init();
        Diameter = StandardFilamentDiameter;
        //DensityUnion = new DefinedDensity(DefinedDensity.BasicPLADensity);
        Density = Constants.BasicPLADensity;
        MaterialType = MaterialType.PLA;
    }
    /// <summary>
    /// Initializes a new instance of the <see cref="FilamentDefn"/> class.
    /// </summary>
    /// <param name="materialType"></param>
    public FilamentDefn(MaterialType materialType)
    {
        Init();
        Diameter = StandardFilamentDiameter;
        MaterialType = materialType;
        //if (materialType == MaterialType.PLA)
        //    DensityUnion = (DefinedDensity)DefinedDensity.BasicPLADensity;
        //else if (materialType == MaterialType.ABS)
        //    DensityUnion = (DefinedDensity)DefinedDensity.BasicABSDensity;
        //else
        //    DensityUnion = new AverageMeasuredDensity(new MeasuredDensity[] { new MeasuredDensity(0f, 0f, 0f) });
    }
    /// <summary>
    /// Initializes a new instance of the <see cref="FilamentDefn"/> class.
    /// </summary>
    /// <param name="diameter">The diameter.</param>
    /// <param name="densityMeasurement">The density measurement.</param>
    /// <param name="materialType">Type of the material.</param>
    public FilamentDefn(double diameter, double definedDensity, MaterialType materialType = MaterialType.PLA)
    {
        Init();
        Diameter = diameter;
        Density = definedDensity;
        //DensityCalc = densityMeasurement;
        //DensityUnion = new AverageMeasuredDensity(new MeasuredDensity[] { densityMeasurement });
        MaterialType = materialType;
    }
    /// <summary>
    /// Gets the milligrams per millimeter.
    /// </summary>
    /// <value>
    /// The milligrams per millimeter (mg/mm)
    /// </value>
    [Description("Milligrams per millimeter"), JsonIgnore, BsonIgnore]
    public double MgPerMM => !double.IsNaN(Density) ? FilamentMath.FilamentVolumeInCubicCentimeters(Diameter / 2, 1.0) * Density * 1000 : double.NaN;

    public override bool HasDependencies(ILiteDBDAL lite)
    {
        if (lite == null)
            throw new ArgumentNullException(nameof(lite), $"A null argument for {nameof(ILiteDBDAL)} was passed to the HasDependencies method for {GetType().Name}");

        foreach (var vend in lite.Vendors)
        {
            if(vend.SpoolDefns?.Count == 0)
                continue;
            foreach (var sp in vend.SpoolDefns)
            {
                if (sp.Inventory?.Count == 0)
                    continue;
                foreach (var inv in sp.Inventory)
                {
                    if (inv.FilamentDefnId == FilamentDefnId)
                        return true;
                }
            }

        }
        return false;
        // no dependencies found
    }
    //public override void UpdateItem()
    //{
    // TODO: Rewrite to support JsonDocument
    //using (TContext context = new TContext())
    //{
    //    InDataOps = true;

    //    if (InDatabase)
    //    {
    //        context.Entry(this).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
    //        context.Entry(this.DensityAlias).State = DensityAlias.InDatabase ? Microsoft.EntityFrameworkCore.EntityState.Modified : Microsoft.EntityFrameworkCore.EntityState.Added;
    //        UpdateContainedItemEntryState(context);
    //        context.Update(this);
    //    }
    //    else
    //    {
    //        context.Entry(this).State = Microsoft.EntityFrameworkCore.EntityState.Added;
    //        context.Entry(this.DensityAlias).State = DensityAlias.InDatabase ? Microsoft.EntityFrameworkCore.EntityState.Modified : Microsoft.EntityFrameworkCore.EntityState.Added;
    //        UpdateContainedItemEntryState(context);
    //        context.Add(this);
    //    }

    //    //context.Update(this);
    //    context.SaveChanges();
    //    InDataOps = false;
    //    SetContainedModifiedState(false);
    //}
    //}
    /*
    internal override void UpdateContainedItemEntryState<TContext>(TContext context)
    {
        context.SetDataItemsState<MeasuredDensity>(DensityAlias?.MeasuredDensity?.Where(md => Added(md)), Microsoft.EntityFrameworkCore.EntityState.Added);
        context.SetDataItemsState<MeasuredDensity>(DensityAlias?.MeasuredDensity?.Where(md => Modified(md)), Microsoft.EntityFrameworkCore.EntityState.Modified);
    }*/
    //public override void EstablishLink(IJsonFilamentDocument document)
    //{
    //    if (document != null)
    //    {
    //        Document = document;
    //        //DensityAlias.EstablishLink(document);
    //    }
    //    else
    //        throw new ArgumentNullException($"{nameof(document)} is null, a valid reference is required.");
    //}
    public override void LinkChildren<ParentType>(ParentType parent)
    {
        //DensityAlias.LinkChildren(this);
    }
    //protected override void SaveToJsonDatabase()
    //{
    //    //if (Document != null)
    //    //    Document.Filaments.Add(this);
    //}
    internal override int KeyID => FilamentDefnId;
    //internal override void UpdateContainedItemsForJson()
    //{
    //    DensityAlias.UpdateContainedItemsForJson();
    //}
    //protected override void AssignKey(int myID)
    //{
    //    if (Document != null)
    //    {
    //        if (FilamentDefnId == default)
    //        {
    //            FilamentDefnId = myID;
    //        }

    //    }
    //}
    public static void SetDataOperationsState(bool state)
    {
        InDataOps = state;
    }
    public override void SetContainedModifiedState(bool state)
    {
        IsModified = state;
        //if (DensityAlias != null)
        //{
        //    DensityAlias.IsModified = state;
        //    foreach (var md in DensityAlias.MeasuredDensity)
        //        md.IsModified = state;
        //}
    }

    public (RemovalResult, object) Remove(LiteDBDal dBDal)
    {
        // Get the reference usage list for the FilamentDefn, if the list is empty the deletion can proceed.
        //IEnumerable<DataObject> references;
        if (dBDal is IReferenceUsage<FilamentDefn> referenceUsage)
        {
            //references = referenceUsage.GetReferences(this);

            if (referenceUsage.GetReferences(this) is IEnumerable<DataObject> references)
                return (RemovalResult.HasReferences, references);
            else
                //TODO : code to remove the FilamentDefn from the database
                System.Diagnostics.Trace.WriteLine("Unfinished code in Remove method in FilamentDefn.", "Information");
        }
        throw new NotImplementedException();
    }
    //struct BackupData
    //{
    //    public MaterialType MaterialType { get; set; }
    //    public double Diameter { get; set; }
    //    public DensityType DensityType { get; set; }
    //    public double DefinedDensity { get; set; }
    //    public bool StopUsing { get; set; }
    //    public bool DensityAliasIsNull { get; set; }
    //    internal BackupData(FilamentDefn filamentDefn)
    //    {
    //        MaterialType = filamentDefn.MaterialType;
    //        Diameter = filamentDefn.Diameter;
    //        DensityAliasIsNull = filamentDefn.DensityAlias == null;
    //        DensityType = filamentDefn.densityAlias?.DensityType ?? DensityType.Defined;
    //        DefinedDensity = filamentDefn.densityAlias?.DefinedDensity ?? double.NaN;
    //        StopUsing = filamentDefn.StopUsing;
    //    }
    //    internal BackupData(MaterialType materialType = MaterialType.PLA, double diameter = double.NaN, bool stopUsing = true, bool densityAliasNull = true, DensityType densityType = DensityType.Defined, double definedDensity = double.NaN)
    //    {
    //        MaterialType = materialType;
    //        Diameter = diameter;
    //        StopUsing = stopUsing;
    //        DensityAliasIsNull = densityAliasNull;
    //        DensityType = densityType;
    //        DefinedDensity = definedDensity;
    //    }
    //}
    //private BackupData backupData;
    //void IEditableObject.BeginEdit()
    //{
    //    if (!InEdit)
    //    {
    //        backupData = new BackupData(this);
    //        InEdit = true;
    //    }
    //    //throw new NotImplementedException();
    //}

    //void IEditableObject.CancelEdit()
    //{
    //    if (InEdit)
    //    {

    //        StopUsing = backupData.StopUsing;
    //        MaterialType = backupData.MaterialType;
    //        if (!backupData.DensityAliasIsNull)
    //        {
    //            DensityAlias.DefinedDensity = backupData.DefinedDensity;
    //            DensityAlias.DensityType = backupData.DensityType;
    //        }
    //        backupData = default;
    //        InEdit = false;
    //        SetContainedModifiedState(false);
    //    }
    //    //throw new NotImplementedException();
    //}

    //void IEditableObject.EndEdit()
    //{
    //    if (InEdit)
    //    {
    //        backupData = default;
    //        InEdit=false;
    //    }
    //    //throw new NotImplementedException();
    //}
}
