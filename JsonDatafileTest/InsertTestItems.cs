using DataDefinitions.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static System.Diagnostics.Debug;
namespace JsonDatafileTest
{
    internal class InsertTestItems
    {
        internal static void InsertTestObjects(DataDefinitions.JsonSupport.JsonFilamentDocument document, int vendorCount)
        {
            Assert(document.Vendors.Count > 0 && document.VendorDefns.Count == vendorCount);
            // Insert a 'Test' vendor into the database 

            var vendor = new VendorDefn()
            {
                Name = "Test",
                FoundOnAmazon = false
            };
            //vendor.EstablishLink(document);
            var plaFilament = document.Filaments.First(f => f.MaterialType == DataDefinitions.MaterialType.PLA);
            //document.Add(vendor);
            var defn = document.PrintSettingDefns.FirstOrDefault(s => s.Definition.Contains("Extruder Temperature"));
            Assert(defn != null);
            var pDefn = new ConfigItem()
            {
                PrintSettingDefn = defn,
                PrintSettingDefnId = defn.PrintSettingDefnId,
                DateEntered = DateTime.Today
            };
            //pDefn.EstablishLink(document);
            pDefn.Value = 210;
            var vendorSettingConfig = new VendorPrintSettingsConfig()
            {
                FilamentDefn = plaFilament,
                FilamentDefnId = plaFilament.FilamentDefnId
            };
            //vendorSettingConfig.EstablishLink(document);
            vendorSettingConfig.ConfigItems.Add(pDefn);
            vendor.VendorSettings.Add(vendorSettingConfig);
            var spoolDefn = new SpoolDefn()
            {
                Description = "Black plastic",
                DrumDiameter = 68,
                SpoolWidth = 58,
                SpoolDiameter = 200,
                Vendor = vendor,
                Weight = 1
            };
            //spoolDefn.EstablishLink(document);
            vendor.SpoolDefns.Add(spoolDefn);
            var colors = new string[] { "AliceBlue", "Aquamarine", "Azure", "Bisque" ,"CadetBlue","Chartreuse","Chocolate","Coral"};
            foreach(var color in colors )
            {
                var inventory = new InventorySpool()
                {
                    FilamentDefn = plaFilament,
                    FilamentDefnId = plaFilament.FilamentDefnId,
                    ColorName = color,
                    DateOpened = DateTime.Today

                };
                spoolDefn.Inventory.Add(inventory);
            }
            vendor.UpdateItem();
            var testFilename = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Filament", "TestFilamentData.Json");
            document.SaveFile(testFilename);

            var retrieveResult = DataDefinitions.JsonSupport.JsonFilamentDocument.LoadFile(testFilename);
            Assert(retrieveResult != null && retrieveResult.Vendors.Count == vendorCount + 1);
            Console.WriteLine("Insert test vendor appears to have succeeded");
        }
    }
}
