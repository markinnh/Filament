using DataDefinitions.Interfaces;
using DataDefinitions.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DataDefinitions.JsonSupport
{
    public class JsonFilamentDocument : IJsonFilamentDocument
    {
        public DocumentLinkedCollection<VendorDefn> VendorDefns { get; set; }
        [JsonIgnore]
        public ICollection<VendorDefn> Vendors => VendorDefns ?? throw new NullReferenceException();

        public DocumentLinkedCollection<FilamentDefn> FilamentDefns { get; set; }
        [JsonIgnore]
        public ICollection<FilamentDefn> Filaments => FilamentDefns ?? throw new NullReferenceException();
        public DocumentLinkedCollection<Setting> SettingDefns { get; set; }
        [JsonIgnore]
        public ICollection<Setting> Settings => SettingDefns ?? throw new NullReferenceException();
        public DocumentLinkedCollection<PrintSettingDefn> PrintSettingDefns { get; set; }
        [JsonIgnore]
        public ICollection<PrintSettingDefn> PrintSettingsDefn => PrintSettingDefns ?? throw new NullReferenceException();
        public CounterProvider IDProvider { get; set; }
        [JsonIgnore]
        public CounterProvider Counters => IDProvider ?? throw new NullReferenceException();

        public bool IsModified { get => VendorDefns.IsModified || FilamentDefns.IsModified || SettingDefns.IsModified || IDProvider.IsModified; }
        public JsonFilamentDocument()
        {
            VendorDefns = new DocumentLinkedCollection<VendorDefn>();
            PrintSettingDefns = new DocumentLinkedCollection<PrintSettingDefn>();
            FilamentDefns = new DocumentLinkedCollection<FilamentDefn>();
            SettingDefns = new DocumentLinkedCollection<Setting>();
            IDProvider = new CounterProvider();
            EstablishDocumentLinks();
        }
        public void Add(DatabaseObject databaseObject)
        {
            if (databaseObject is VendorDefn vendor)
                Add(vendor);
            else if (databaseObject is PrintSettingDefn printSetting)
                Add(printSetting);
            else if (databaseObject is FilamentDefn filament)
                Add(filament);
            else if (databaseObject is Setting setting)
                Add(setting);
            else throw new NotSupportedException($"Unable to add {databaseObject.GetType()} to the datafile");
        }
        public void Add(VendorDefn vendor)
        {
            VendorDefns.Add(vendor);
        }

        public void Add(FilamentDefn filament)
        {
            FilamentDefns.Add(filament);
        }

        public void Add(Setting setting)
        {
            Settings.Add(setting);
        }
        public void Add(PrintSettingDefn printSettingDefn)
        {
            PrintSettingDefns.Add(printSettingDefn);
        }
        public void EstablishDocumentLinks()
        {
            VendorDefns.EstablishLink(this);
            FilamentDefns.EstablishLink(this);
            SettingDefns.EstablishLink(this);
            PrintSettingDefns.EstablishLink(this);

            //throw new NotImplementedException();
        }
        public void SeedDocument()
        {
            DataSeed.Seed(this);
        }
        public string Serialize() => JsonSerializer.Serialize(this);
        public static JsonFilamentDocument LoadFile(string fileName)
        {
            string contents;
            if (File.Exists(fileName))
            {
                contents = File.ReadAllText(fileName);
                return Deserialize(contents);
            }
            else
            {
                return new JsonFilamentDocument();
            }
        }
        public void SaveFile(string fileName)
        {
            //var options = ;

            var file = File.CreateText(fileName);
            file.Write(JsonSerializer.Serialize<JsonFilamentDocument>(this, new JsonSerializerOptions() { WriteIndented = true }));
            file.Flush();
            file.Close();
        }
        protected static void PrepareToDeserialize()
        {
            FilamentDefn.InDataOps = true;
            VendorDefn.InDataOps = true;
            SpoolDefn.InDataOps = true;
            DensityAlias.InDataOps = true;
            PrintSettingDefn.InDataOps = true;
            DepthMeasurement.InDataOps = true;
            InventorySpool.InDataOps = true;
            VendorPrintSettingsConfig.InDataOps = true;
            ConfigItem.InDataOps = true;
        }
        protected static void FinishDeserialization()
        {
            FilamentDefn.InDataOps = false;
            VendorDefn.InDataOps = false;
            SpoolDefn.InDataOps = false;
            DensityAlias.InDataOps = false;
            PrintSettingDefn.InDataOps = false;
            DepthMeasurement.InDataOps = false;
            InventorySpool.InDataOps = false;
            VendorPrintSettingsConfig.InDataOps = false;
            ConfigItem.InDataOps = false;
        }
        public static JsonFilamentDocument Deserialize(string content)
        {
            //JsonFilamentDocument jsonFilamentDocument = ;
            PrepareToDeserialize();
            if (JsonSerializer.Deserialize<JsonFilamentDocument>(content) is JsonFilamentDocument filamentDocument)
            {
                filamentDocument.EstablishDocumentLinks();
                filamentDocument.LinkChildren();
                FinishDeserialization();
                return filamentDocument;
            }
            else
            {
                return new JsonFilamentDocument();
            }
            // TODO: code to establish document links

        }
        protected void LinkChildren()
        {
            foreach (var vendorDefn in VendorDefns)
                vendorDefn.LinkChildren(this);
            //throw new NotImplementedException();
        }
    }
}
