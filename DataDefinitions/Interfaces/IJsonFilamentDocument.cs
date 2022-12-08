using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using DataDefinitions.JsonSupport;
using DataDefinitions.Models;

namespace DataDefinitions.Interfaces
{
    public interface IStdDatabaseActions
    {
        void SeedDatabase();
    }
    /// <summary>
    /// StandardCrudActions, doesn't implement delete items, since it only applies to InventorySpools
    /// </summary>
    public interface IStandardCrudActions : IStdDatabaseActions
    {
        void Add(VendorDefn vendor);
        void Add(FilamentDefn filament);
        void Add(Setting setting);
        void Add(PrintSettingDefn printSettingDefn);
        void Update(VendorDefn vendor);
        void Update(FilamentDefn filament);
        void Update(Setting setting);
        void Update(PrintSettingDefn printSettingDefn);
    }
    /// <summary>
    /// Contains objects/collections that support the Filament database in json format
    /// </summary>
    /// <remarks>
    /// Most of the 'objects' are contained within other items.  So the number of 'collections' is minimal.  Settings, Vendors, and Filaments
    /// </remarks>
    public interface IJsonFilamentDocument
    {
        ICollection<VendorDefn> Vendors { get; }
        ICollection<FilamentDefn> Filaments { get; }
        ICollection<PrintSettingDefn> PrintSettingsDefn { get; }
        ICollection<Setting> Settings { get; }
        CounterProvider Counters { get; }
        bool IsModified { get; }
        void Add(DatabaseObject databaseObject);
        void Add(VendorDefn vendor);
        void Add(FilamentDefn filament);
        void Add(Setting setting);
        void Add(PrintSettingDefn printSettingDefn);
        void EstablishDocumentLinks();
        void SeedDocument();
        void SaveFile(string fileName);
    }
}
