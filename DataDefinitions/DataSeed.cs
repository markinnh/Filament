using System;
using System.Linq;
using DataDefinitions;
using DataDefinitions.Models;
using static System.Diagnostics.Debug;

using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace DataDefinitions
{
    public class DataSeed
    {
        const double InitialSeed = 0.1;
        const double AddVendorDefn = 0.15;
        const double AddPrinterSettingDefn = 0.151;
        public static void Seed<TContext>() where TContext : BaseFilamentContext, new()
        {
            using (BaseFilamentContext context = new TContext())
            {
                context.PerformMigrations();
                //context.Database.Migrate();
                if (context is BaseFilamentContext filamentContext)
                {

                    var setting = context.Settings.FirstOrDefault(s => s.Name == "SeedData");
                    if (setting is null)
                    {
                        InitialSeeding<TContext>();
                        if (filamentContext.Settings.FirstOrDefault(s => s.Name == "SeedData") is Setting setting1)
                        {
                            SeedVendorData<TContext>(setting1, context);
                        }
                    }
                    else
                    {
                        WriteLine(setting);
                        if (setting < AddVendorDefn)
                            SeedVendorData<TContext>(setting, filamentContext);
                        if (setting < AddPrinterSettingDefn)
                            SeedData<TContext>(setting,AddPrinterSettingDefn, filamentContext,DataDefinitions.Seed.InitialPrintSettingDefinitions());
                    }
                }
            }

            //using(DataContext.FilamentContext ctx = new ())
            //{
            //    if(ctx != null)
            //    {
            //        if(ctx.Settings?.FirstOrDefault(s=>s.Name=="SeedData") is null)
            //        {
            //            InitialSeeding();
            //        }
            //        // if further seeding is required, just check the SeedData value 
            //    }
            //}
        }
        protected static void SeedData<TContext>(Setting setting,double UpdatedSeedValue,BaseFilamentContext filamentContext,IEnumerable<DatabaseObject> databaseObjects) where TContext:BaseFilamentContext,new()
        {
            foreach (DepthMeasurement databaseObject in databaseObjects)
                databaseObject.UpdateItem<TContext>();
            
            setting.SetValue(UpdatedSeedValue);
            setting.UpdateItem<TContext>();

            setting.SetValue(UpdatedSeedValue);
        }
        public static void VerifySeed<TContext>() where TContext : BaseFilamentContext, new()
        {

            if (BaseFilamentContext.GetSetting<TContext>(s => s.Name == "SeedData") is Setting setting)
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
            BaseFilamentContext.AddAll<TContext>(new Setting("SeedData", InitialSeed));
            //DataContext.FilamentContext.AddAll(filamentDefns, );
            //FilamentDefn[] startingFilaments = InitialFilamentDefinitions();
            //ctx.AddRange(startingFilaments);
            //ctx.Add(new Setting("SeedData", InitialSeed));
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
    }
}
