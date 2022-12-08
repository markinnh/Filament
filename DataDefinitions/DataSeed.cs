using System;
using System.Linq;
using DataDefinitions;
using DataDefinitions.Models;
using static System.Diagnostics.Debug;


using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Json;
//using LiteDB;
using DataDefinitions.Interfaces;

namespace DataDefinitions
{
    public static class Extensions
    {
        public static string JsonSerializedSize(this object obj) => $"{obj.GetType().Name} serialized size {JsonSerializer.Serialize(obj).Length}";

    }
    public class DataSeed
    {
        internal const double InitialSeed = 0.1;
        internal const double AddVendorDefn = 0.15;
        internal const double AddPrinterSettingDefn = 0.171;
        internal const double AddInventorySpools = .16;
        // TODO: write seeding code to support JSON document format and current version of software
        public static void Seed(IJsonFilamentDocument document)
        {
            var setting = document.Settings.FirstOrDefault(st => st.Name == Constants.dataSeedingKey);
            if (setting == null || setting < InitialSeed)
            {
                // needs initial seeding
                InitialSeeding(document, ref setting);
                // needs VendorDefn seeding
                //SeedVendors(document, ref setting);
                // needs PrintSettingDefn seeding
                //SeedPrintDefns(document, ref setting);
                //SeedInventory(document, ref setting);
            }
            Assert(setting != null);
            ///<remarks>
            /// indicates the database will be seeded
            ///</remarks>
            var seeded = setting < AddInventorySpools;

            if (setting < AddVendorDefn)
                SeedVendors(document, ref setting);



            if (setting < AddInventorySpools)
                SeedInventory(document, ref setting);
            if (setting < AddPrinterSettingDefn)
                SeedPrintDefns(document, ref setting);
            
            if (seeded)
            {
                var prepopulate = document.Settings.FirstOrDefault(st => st.Name == Constants.prepopulateDateTime);

                if (prepopulate == null)
                {
                    prepopulate = new Setting() { Name = Constants.prepopulateDateTime };
                    //prepopulate.EstablishLink(document);
                    prepopulate.UpdateItem();
                }

                prepopulate.SetValue(DateTimeOffset.Now);
            }
        }

        protected static void InitialSeeding(IJsonFilamentDocument document, ref Setting setting)
        {
            foreach (var filamentDefn in DataDefinitions.Seed.InitialFilamentDefinitions())
            {
                filamentDefn.UpdateItem();
            }
            if (setting != null)
                setting.SetValue(InitialSeed);
            else
                setting = CreateSeedSetting(document, InitialSeed);
        }
        protected static void SeedVendors(IJsonFilamentDocument document, ref Setting setting)
        {
            foreach (var vendor in DataDefinitions.Seed.InitialVendorDefinitions())
            {
                vendor.UpdateItem();
            }

            if (setting != null)

                setting.SetValue(AddVendorDefn);

            else
                setting = CreateSeedSetting(document, AddVendorDefn);
        }
        protected static void SeedInventory(IJsonFilamentDocument document, ref Setting setting)
        {
            DepthMeasurement.InDataOps = true;
            var vendor = document.Vendors.FirstOrDefault(v => v.Name == "Flashforge");
            var plaFilament = document.Filaments.FirstOrDefault(f => f.MaterialType == MaterialType.PLA);
            var vendorSpool = vendor.SpoolDefns.First();
            Assert(plaFilament != null, "PLA filament is not initialized.");
            Assert(vendor != null, "Vendor is not initialized");
            // Add an orange inventory and a measurement
            if (vendor != null && plaFilament != null)
            {
                InventorySpool inventory = new InventorySpool()
                {
                    ColorName = "Orange",
                    DateOpened = DateTime.Today - TimeSpan.FromDays(5),
                    FilamentDefn = plaFilament,
                    FilamentDefnId = plaFilament.FilamentDefnId,
                    Parent = vendorSpool
                    //SpoolDefnId = vendorSpool.SpoolDefnId
                };
                //Console.WriteLine(inventory.SpoolDefn.Vendor.JsonSerializedSize());
                //Console.WriteLine(inventory.JsonSerializedSize());
                var dm = new DepthMeasurement() { Parent = inventory, Depth1 = 32.2, Depth2 = 31.1, MeasureDateTime = DateTime.Now };
                //Console.WriteLine(dm.JsonSerializedSize());
                //dm.InventorySpool = inventory;
                //Console.WriteLine($"{dm.Depth1},{dm.Depth2}");
                inventory.DepthMeasurements.Add(dm);
                //inventory.EstablishLink(document);
                vendorSpool.Inventory.Add(inventory);
                vendorSpool.Vendor.UpdateItem();
                if (setting != null)
                    setting.SetValue(AddInventorySpools);
                else
                    setting = CreateSeedSetting(document, AddInventorySpools);

            }
            DepthMeasurement.InDataOps = false;
        }
        protected static void SeedPrintDefns(IJsonFilamentDocument document, ref Setting setting)
        {
            foreach (var printSettingDefn in DataDefinitions.Seed.InitialPrintSettingDefinitions())
            {
                //printSettingDefn.EstablishLink(document);
                printSettingDefn.UpdateItem();
            }

            if (setting != null)
                setting.SetValue(AddPrinterSettingDefn);
            else
                setting = CreateSeedSetting(document, AddPrinterSettingDefn);
        }
        protected static Setting CreateSeedSetting(IJsonFilamentDocument document, double settingValue)
        {
            var setting = new Setting() { Name = Constants.dataSeedingKey, Value = settingValue.ToString() };
            //setting.EstablishLink(document);
            setting.UpdateItem();
            return setting;
        }
        //public static void Seed() 
        //{
        //    using (BaseFilamentContext context = new TContext())
        //    {
        //        context.PerformMigrations();
        //        //context.Database.Migrate();
        //        if (context is BaseFilamentContext filamentContext)
        //        {

        //            var setting = context.Settings.FirstOrDefault(s => s.Name == Constants.dataSeedingKey);
        //            if (setting is null)
        //            {
        //                InitialSeeding<TContext>();
        //                if (filamentContext.Settings.FirstOrDefault(s => s.Name == Constants.dataSeedingKey) is Setting setting1)
        //                {
        //                    SeedVendorData<TContext>(setting1, context);
        //                }
        //            }
        //            else
        //            {
        //                WriteLine(setting);
        //                if (setting < AddVendorDefn)
        //                    SeedVendorData<TContext>(setting, filamentContext);
        //                if (setting < AddPrinterSettingDefn)
        //                    SeedData<TContext>(setting,AddPrinterSettingDefn, filamentContext,DataDefinitions.Seed.InitialPrintSettingDefinitions());
        //            }
        //        }
        //    }

        //    //using(DataContext.FilamentContext ctx = new ())
        //    //{
        //    //    if(ctx != null)
        //    //    {
        //    //        if(ctx.Settings?.FirstOrDefault(s=>s.Name==Constants.dataSeedingKey) is null)
        //    //        {
        //    //            InitialSeeding();
        //    //        }
        //    //        // if further seeding is required, just check the SeedData value 
        //    //    }
        //    //}
        //}
        /*
        protected static void SeedData<TContext>(Setting setting,double UpdatedSeedValue,BaseFilamentContext filamentContext,IEnumerable<DatabaseObject> databaseObjects) where TContext:BaseFilamentContext,new()
        {
            foreach (DepthMeasurement databaseObject in databaseObjects)
                databaseObject.UpdateItem();
            
            setting.SetValue(UpdatedSeedValue);
            setting.UpdateItem();

            setting.SetValue(UpdatedSeedValue);
        }
        public static void VerifySeed<TContext>() where TContext : BaseFilamentContext, new()
        {

            if (BaseFilamentContext.GetSetting<TContext>(s => s.Name == Constants.dataSeedingKey) is Setting setting)
            {
                if (setting == InitialSeed)
                {
                    if (BaseFilamentContext.GetAllFilaments<TContext>()?.FirstOrDefault() is FilamentDefn definition)
                    {
                        Assert(definition.MaterialType == MaterialType.PLA);
                        WriteLine($"Filament Type : {definition.MaterialType}, Density : {definition.DensityAlias?.Density}, Density Type : {definition.DensityAlias.DensityType}");
                    }
                }
            }
        }
        private static void InitialSeeding<TContext>() where TContext : BaseFilamentContext, new()
        {
            WriteLine("Performing initial seeding of FilamentDefns");
            var filamentDefns = DataDefinitions.Seed.InitialFilamentDefinitions();
            foreach (var item in filamentDefns)
            {
                item.UpdateItem<TContext>();
            }
            BaseFilamentContext.AddAll<TContext>(new Setting(Constants.dataSeedingKey, InitialSeed));
            //DataContext.FilamentContext.AddAll(filamentDefns, );
            //FilamentDefn[] startingFilaments = InitialFilamentDefinitions();
            //ctx.AddRange(startingFilaments);
            //ctx.Add(new Setting(Constants.dataSeedingKey, InitialSeed));
            //ctx.SaveChanges();

        }
        private static void SeedVendorData<TContext>(Setting setting, BaseFilamentContext filamentContext) where TContext : BaseFilamentContext, new()
        {
            WriteLine("Seeding VendorDefns");
            var seedVendors = DataDefinitions.Seed.InitialVendorDefinitions();
            foreach (var item in seedVendors)
                item.UpdateItem<TContext>();
            //System.Diagnostics.Debug.Assert(insertCount == seedVendors.Length, "Not all vendor definitions were inserted into the database.");
            setting.SetValue(AddVendorDefn);

            filamentContext.Entry(setting).State = EntityState.Modified;
            filamentContext.Update(setting);
            filamentContext.SaveChanges();
        }
        */
    }
}
