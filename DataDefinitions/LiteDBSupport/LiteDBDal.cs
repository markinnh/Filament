using DataDefinitions.Interfaces;
using DataDefinitions.Models;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataDefinitions.LiteDBSupport;

public partial class LiteDBDal : ILiteDBDAL, IReferenceUsage<FilamentDefn>, IEventListener
{

    const string settingTableName = "settings";
    const string printSettingTableName = "printSettings";
    const string vendorTableName = "vendors";
    const string noteTableName = "notes";
    const string filamentTableName = "filaments";
    const string inventoryTableName = "inventory";
    const string filamentColorsTableName = "colors";
    const string materialTableName = "materials";
    //#if DEBUG 

    //        static readonly string testingFilename = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "FilamentDataLiteDb.db");
    //#else
    //        static readonly string testingFilename = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "FilamentDataLiteDb.db");
    //#endif

    public ObservableCollection<Material> Materials
    {
        get; protected set;
    }
    public ObservableCollection<FilamentDefn> Filaments
    {
        get; protected set;
    }

    public CollateTagsCollection<VendorDefn> Vendors
    {
        get; protected set;
    }
    public CollateTagsCollection<NoteDefn> Notes
    {
        get; protected set;
    }

    public List<Setting> Settings
    {
        get; protected set;
    }

    public ObservableCollection<PrintSettingDefn> PrintSettings
    {
        get; protected set;
    }

    public ObservableCollection<FilamentColor> FilamentColors
    {
        get;
        protected set;
    }
    public string Filename
    {
        get; protected set;
    }
    protected string ConnectionString => $"filename={Filename}; Connection=\"shared\"";
    /// <summary>
    /// Initialize the LiteDb Data Layer
    /// </summary>
    /// <param name="filename">filename to be access, currently ignored using TestingFilamentDataLDB.db in the My Documents directory</param>
    /// <param name="platform"></param>
    public void Initialize(string filename, UIPlatform platform = UIPlatform.WPF)
    {
        //Debug.Assert(filename != null);
        if (string.IsNullOrEmpty(filename))
        {
            Filename = FileLocator.DatabasePath(platform);
        }
        else
            Filename = FileLocator.DatabasePath(platform);
        //var cString = $"filename={Filename}; Connection=\"shared\"";
        ConnectionString connection = new ConnectionString(ConnectionString);
        PrepareToDeserialize();
        using LiteDatabase database = new LiteDatabase(connection);
        var setting = database.GetCollection<Setting>(settingTableName).Query().ToEnumerable().FirstOrDefault(s => s.Name.Contains("SeedData"));
        if (setting == null || setting < DataSeed.AddInventorySpools)
            SeedDatabase();

        Settings = new List<Setting>(database.GetCollection<Setting>(settingTableName).Query().ToEnumerable());
        //PrintSettings = new ObservableCollection<PrintSettingDefn>(database.GetCollection<PrintSettingDefn>(printSettingTableName).Query().ToEnumerable());

        Filaments = new ObservableCollection<FilamentDefn>(database.GetCollection<FilamentDefn>(filamentTableName).Query().ToEnumerable());
        Vendors = new CollateTagsCollection<VendorDefn>(database.GetCollection<VendorDefn>(vendorTableName).Query().ToEnumerable());
        FilamentColors = new ObservableCollection<FilamentColor>(database.GetCollection<FilamentColor>(filamentColorsTableName).Query().ToEnumerable());
        Materials = new ObservableCollection<Material>(database.GetCollection<Material>(materialTableName).Query().ToEnumerable());
        // populate the Inventory
        // TODO: relink the Materials with the Filaments, after the MaterialType is changed to Material and a MaterialId member is added to the FilamentDefn
        foreach (var filament in Filaments)
        {
            if (filament.VendorID is int id && id != 0)
            {
                filament.Vendor = Vendors.Where(v => v.VendorDefnId == id).SingleOrDefault();
            }
            if (filament.MaterialId != default)
                filament.Material = Materials.Where(m => m.MaterialId == filament.MaterialId).SingleOrDefault();
        }

        foreach (var vendor in Vendors)
        {
            vendor.LinkInventoryToFilaments(Filaments);
            vendor.LinkInventoryToFilamentColors(FilamentColors);
            vendor.LinkInventoryToMeasurements();
            //vendor.LinkChildren(vendor);
            vendor.PostDataRetrieveActions();
            //vendor.LinkVendorSettingsToPrintDefns(PrintSettings);
        }
        Notes = new CollateTagsCollection<NoteDefn>(database.GetCollection<NoteDefn>(noteTableName).Query().ToEnumerable());
        FinishDeserialization();
    }
    public LiteDBDal()
    {

    }
    public void SetFilename(string filename) => Filename = filename;
    public void SmartAdd(DataObject dbObject)
    {
        switch (dbObject)
        {
            case VendorDefn vendorDefn:
                vendorDefn.PrepareToSave(this);
                if (vendorDefn.InDatabase)
                    Update(vendorDefn);
                else
                {
                    Add(vendorDefn);
                    if (!Vendors.Contains(vendorDefn))
                        Vendors.Add(vendorDefn);
                    // A second update is necessary if the VendorDefnId has to be changed in the SpoolDefn
                    var updated = false;
                    vendorDefn.UpdateContainedItemsForLiteDb(ref updated);
                    if (updated)
                        Update(vendorDefn);
                }
                break;
            case FilamentDefn filamentDefn:
                if (filamentDefn.InDatabase)
                    Update(filamentDefn);
                else
                {
                    Add(filamentDefn);
                    if (!Filaments.Contains(filamentDefn))
                        Filaments.Add(filamentDefn);
                }
                break;
            case Setting setting:
                if (setting.InDatabase)
                    Update(setting);
                else
                {
                    Add(setting);
                    if (!Settings.Contains(setting))
                        Settings.Add(setting);
                }
                break;

            case NoteDefn noteDefn:
                if (noteDefn.InDatabase)
                    Update(noteDefn);
                else
                {
                    Add(noteDefn);
                    if (!Notes.Contains(noteDefn))
                        Notes.Add(noteDefn);
                }
                break;
            case FilamentColor filamentColor:
                if (filamentColor.InDatabase)
                    Update(filamentColor);
                else
                    Add(filamentColor);

                if (!FilamentColors.Contains(filamentColor))
                    FilamentColors.Add(filamentColor);
                break;
            case Material material:
                if (material.InDatabase)
                    Update(material);
                else
                    Add(material);
                if (!Materials.Contains(material))
                    Materials.Add(material);
                break;
            default:
                throw new NotSupportedException($"Support for {dbObject.GetType().Name} is not implemented");
        }
        dbObject.IsModified = false;
        dbObject.SetContainedModifiedState(false);
    }
    public void Upsert(IEnumerable<DatabaseObject> adds)
    {
        switch (adds)
        {

            case IEnumerable<VendorDefn> vendorDefinitions:
                Upsert(vendorDefinitions, vendorTableName);
                break;
            case IEnumerable<FilamentDefn> filamentDefinitions:
                Upsert(filamentDefinitions, filamentTableName);
                break;
            default:
                Trace.WriteLine($"Upsert not implemented for IEnumerable<{GetType().GetGenericArguments().FirstOrDefault()?.Name}>");
                break;
        }


    }
    private void Upsert<TAdd>(IEnumerable<TAdd> adds, string collectionName) where TAdd : DatabaseObject
    {
        using LiteDatabase db = new LiteDatabase(ConnectionString);
        db.GetCollection<TAdd>(collectionName).Upsert(adds);

    }
    private void Add<TAdd>(TAdd add, string collectionName) where TAdd : DatabaseObject
    {
        using LiteDatabase database = new LiteDatabase(ConnectionString);
        database.GetCollection<TAdd>(collectionName).Insert(add);
    }
    public void Add(Material material) => Add(material, materialTableName);
    public void Add(VendorDefn vendor)
    {
        Add(vendor, vendorTableName);
        //using(LiteDatabase database=new LiteDatabase(ConnectionString))
        //{
        //    database.GetCollection<VendorDefn>("vendors").Insert(vendor);
        //}
        //throw new NotImplementedException();
    }

    public void Add(FilamentDefn filament)
    {
        Add(filament, filamentTableName);
        //throw new NotImplementedException();
    }

    public void Add(Setting setting)
    {
        Add(setting, settingTableName);
        //throw new NotImplementedException();
    }

    public void Add(PrintSettingDefn printSettingDefn)
    {
        Add(printSettingDefn, printSettingTableName);
        //throw new NotImplementedException();
    }
    public void Add(NoteDefn noteDefn)
    {
        Add(noteDefn, noteTableName);
    }
    public void Add(FilamentColor filamentColor) => Add(filamentColor, filamentColorsTableName);
    //public void Add(InventorySpool inventorySpool) => Add(inventorySpool, inventoryTableName);
    private void Update<TUpdate>(TUpdate update, string collectionName) where TUpdate : DatabaseObject
    {
        using LiteDatabase database = new LiteDatabase(ConnectionString);
        database.GetCollection<TUpdate>(collectionName).Update(update);
    }
    public void Update(VendorDefn vendor)
    {
        Update(vendor, vendorTableName);
        //throw new NotImplementedException();
    }

    public void Update(FilamentDefn filament)
    {
        Update(filament, filamentTableName);
        //throw new NotImplementedException();
    }

    public void Update(Setting setting)
    {
        Update(setting, settingTableName);
        //throw new NotImplementedException();
    }

    public void Update(PrintSettingDefn printSettingDefn)
    {
        Update(printSettingDefn, printSettingTableName);
        //throw new NotImplementedException();
    }
    public void Update(NoteDefn noteDefn)
    {
        Update(noteDefn, noteTableName);
    }
    public void Update(FilamentColor filamentColor) => Update(filamentColor, filamentColorsTableName);
    public void Update(Material material) => Update(material, materialTableName);
    //public void Update(InventorySpool inventorySpool) => Update(inventorySpool, inventoryTableName);
    
    public void SeedDatabase()
    {
        if (string.IsNullOrEmpty(Filename))
            throw new Exception("Filename is not initialized, unable to seed the database.");
        else
        {
            var cString = $"filename={Filename}; Connection=\"shared\"";
            using LiteDatabase database = new LiteDatabase(cString);
            Seed(database);
        }
        //throw new NotImplementedException();
    }
    internal void Seed(ILiteDatabase liteDatabase)
    {
        var seeding = liteDatabase.GetCollection<Setting>(settingTableName).Query().ToArray().FirstOrDefault<Setting>(s => s.Name.Contains(Constants.dataSeedingKey));
        if (seeding == null || seeding < DataSeed.InitialSeed)
        {
            Debug.WriteLine("Seeding filaments");
            InitialSeeding(liteDatabase, ref seeding);
        }
        if (seeding < DataSeed.AddVendorDefn)
        {
            Debug.WriteLine("Seeding vendors");
            SeedVendorDefn(liteDatabase, seeding);
        }
        if (seeding < DataSeed.AddInventorySpools)
        {
            Debug.WriteLine("Seeding inventory");
            SeedInventory(liteDatabase, seeding);
        }

    }

    private static void InitialSeeding(ILiteDatabase liteDatabase, ref Setting seeding)
    {
        var col = liteDatabase.GetCollection<FilamentDefn>(filamentTableName);
        col.InsertBulk(DataDefinitions.Seed.InitialFilamentDefinitions());
        foreach (var item in col.Query().ToArray())
        {
            item.LinkChildren<FilamentDefn>(item);
            col.Update(item);
        }
        if (seeding != null)
        {
            seeding.SetValue(DataSeed.InitialSeed);
            liteDatabase.GetCollection<Setting>(settingTableName).Update(seeding);
        }
        else
        {
            seeding = new Setting()
            {
                Name = Constants.dataSeedingKey,
                Value = DataSeed.InitialSeed.ToString()
            };

            liteDatabase.GetCollection<Setting>(settingTableName).Insert(seeding);
        }
    }
    private static void SeedVendorDefn(ILiteDatabase liteDatabase, Setting seeding)
    {
        var col = liteDatabase.GetCollection<VendorDefn>(vendorTableName);
        var vendors = DataDefinitions.Seed.InitialVendorDefinitions();
        col.InsertBulk(vendors);
        foreach (var item in vendors)
            col.Update(item);
        seeding.SetValue(DataSeed.AddVendorDefn);
        liteDatabase.GetCollection<Setting>(settingTableName).Update(seeding);
    }
    private void SeedInventory(ILiteDatabase liteDatabase, Setting seeding)
    {
        var col = liteDatabase.GetCollection<VendorDefn>(vendorTableName);
        var flashforge = col.Query().ToEnumerable().First<VendorDefn>(v => v.Name.Contains("Flashforge"));
        var plaFilament = liteDatabase.GetCollection<FilamentDefn>(filamentTableName).Query().ToEnumerable().First(f => f.MaterialType == MaterialType.PLA);
        var sd = flashforge.SpoolDefns.First();
        InventorySpool inventory = new InventorySpool()
        {
            ColorName = "Orange",
            DateOpened = DateTime.Today - TimeSpan.FromDays(5),
            FilamentDefn = plaFilament,
            FilamentDefnId = plaFilament.FilamentDefnId,
            Parent = sd
            //SpoolDefn = sd,
            //SpoolDefnId = sd.SpoolDefnId
        };
        inventory.LinkToParent(sd);
        var dm = new DepthMeasurement() { InventorySpool = inventory, Depth1 = 32.2, Depth2 = 31.1, MeasureDateTime = DateTime.Now };
        inventory.DepthMeasurements.Add(dm);
        sd.Inventory.Add(inventory);

        col.Update(flashforge);

        seeding.SetValue(DataSeed.AddInventorySpools);
        liteDatabase.GetCollection<Setting>(settingTableName).Update(seeding);
    }
    private static void SeedPrintSettings(ILiteDatabase database, Setting setting)
    {
        var col = database.GetCollection<PrintSettingDefn>(printSettingTableName);

        col.InsertBulk(DataDefinitions.Seed.InitialPrintSettingDefinitions());
        setting.SetValue(DataSeed.AddPrinterSettingDefn);
        database.GetCollection<Setting>(settingTableName).Update(setting);
    }
    protected static void PrepareToDeserialize()
    {
        FilamentDefn.InDataOps = true;
        VendorDefn.InDataOps = true;
        SpoolDefn.InDataOps = true;
        DensityAlias.InDataOps = true;
        PrintSettingDefn.InDataOps = true;
        DepthMeasurement.InDataOps = true;
        WeightMeasurement.InDataOps = true;
        InventorySpool.InDataOps = true;
        VendorPrintSettingsConfig.InDataOps = true;
        ConfigItem.InDataOps = true;
        NoteDefn.InDataOps = true;
        FilamentColor.InDataOps = true;
        Material.InDataOps = true;
    }
    protected static void FinishDeserialization()
    {
        FilamentDefn.InDataOps = false;
        VendorDefn.InDataOps = false;
        SpoolDefn.InDataOps = false;
        DensityAlias.InDataOps = false;
        PrintSettingDefn.InDataOps = false;
        DepthMeasurement.InDataOps = false;
        WeightMeasurement.InDataOps = false;
        InventorySpool.InDataOps = false;
        VendorPrintSettingsConfig.InDataOps = false;
        ConfigItem.InDataOps = false;
        NoteDefn.InDataOps = false;
        FilamentColor.InDataOps = false;
        Material.InDataOps = false;
    }

    public IEnumerable<DataObject> GetReferences(FilamentDefn item)
    {
        foreach (var vendor in Vendors)
            foreach (var vendorRef in vendor.GetReferences(item))
                yield return vendorRef;
        //throw new NotImplementedException();
    }
    public void ReleaseEventHandlers()
    {
        foreach (var vendor in Vendors)
            vendor.ReleaseEventHandlers();
        foreach (var filament in Filaments)
            filament.ReleaseEventHandlers();

    }
    public void UpdateInDatabaseFlag(ISharingSupport support)
    {
        switch (support)
        {
            case VendorDefn vdefn:
                vdefn.NotAlreadyInDatabase = Vendors.FirstOrDefault(v => v.Name == vdefn.Name && v.WebUrl == vdefn.WebUrl) == null;
                break;
            case FilamentDefn fdefn:
                fdefn.NotAlreadyInDatabase = Filaments.FirstOrDefault(fd => fd.Material?.ShortName == fdefn?.Material.ShortName) == null;
                break;
            case FilamentColor filaColor:
                filaColor.NotAlreadyInDatabase = FilamentColors.FirstOrDefault(fc => fc.VendorColorName == filaColor.VendorColorName && fc.ColorRGB == filaColor.ColorRGB) == null;
                break;
            default:
                Trace.WriteLine($"No support to update database existence for {support.GetType().Name}");
                break;
        }
    }
#if DEBUG

#endif
}
