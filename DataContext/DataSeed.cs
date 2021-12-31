using System;
using System.Linq;
using DataDefinitions;
using DataDefinitions.Models;

using Microsoft.EntityFrameworkCore;

namespace DataContext
{
    public class DataSeed
    {
        const double InitialSeed = 0.1;
        const double AddVendorDefn = 0.15;
        public static void Seed<TContext>() where TContext : DbContext, new()
        {
            using (TContext context = new TContext())
            {
                //context.Database.Migrate();
                if (context is FilamentContext filamentContext) 
                {
                    filamentContext.Database.Migrate();
                    var setting = filamentContext.Settings.FirstOrDefault(s => s.Name == "SeedData");
                    if (setting is null)
                    {
                        InitialSeeding();
                        if (filamentContext.Settings.FirstOrDefault(s => s.Name == "SeedData") is Setting setting1)
                        {
                            SeedVendorData<TContext>(setting1,filamentContext);
                        }
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine(setting);
                        if (setting < AddVendorDefn)
                            SeedVendorData<TContext>(setting,filamentContext);
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
        public static void VerifySeed()
        {
            if (FilamentContext.GetSetting(s => s.Name == "SeedData") is Setting setting)
            {
                if (setting == InitialSeed)
                {
                    if (DataContext.FilamentContext.GetAllFilaments()?.FirstOrDefault() is FilamentDefn definition)
                    {
                        System.Diagnostics.Debug.WriteLine($"Filament Type : {definition.MaterialType}, Density : {definition.DensityAlias?.Density}, Density Type : {definition.DensityAlias.DensityType}");
                    }
                }
            }
        }
        private static void InitialSeeding()
        {
            System.Diagnostics.Debug.WriteLine("Performing initial seeding of FilamentDefns");
            var filamentDefns = DataDefinitions.Seed.InitialFilamentDefinitions();
            foreach (var item in filamentDefns)
            {
                item.UpdateItem<FilamentContext>();
            }
            FilamentContext.AddAll(new Setting("SeedData", InitialSeed));
            //DataContext.FilamentContext.AddAll(filamentDefns, );
            //FilamentDefn[] startingFilaments = InitialFilamentDefinitions();
            //ctx.AddRange(startingFilaments);
            //ctx.Add(new Setting("SeedData", InitialSeed));
            //ctx.SaveChanges();

        }
        private static void SeedVendorData<TContext>(Setting setting,FilamentContext filamentContext) where TContext : DbContext,new()
        {
            System.Diagnostics.Debug.WriteLine("Seeding VendorDefns");
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
