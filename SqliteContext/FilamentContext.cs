using Microsoft.EntityFrameworkCore;
using DataDefinitions.Models;
using System.Collections;

namespace SqliteContext
{
    public class FilamentContext : DataDefinitions.BaseFilamentContext
    {
        //public DbSet<FilamentDefn>? FilamentDefns { get; set; }
        //public DbSet<SpoolDefn>? SpoolDefns { get; set; }
        //public DbSet<VendorDefn>? VendorDefns { get; set; }
        //public DbSet<InventorySpool>? InventorySpools { get; set; }
        //public DbSet<DensityAlias>? DensityAliases { get; set; }
        //public DbSet<DepthMeasurement>? DepthMeasurements { get; set; }
        //public DbSet<MeasuredDensity>? MeasuredDensity { get; set; }
        //public DbSet<Setting>? Settings { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Filament");
            optionsBuilder.UseSqlite($"Data Source={System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Filament", "FilamentData.db")};");
        }
        public override void PerformMigrations(ref bool NeedMigration)
        {
            //if (!DbExists)
            //{
                try
                {
                    using (var context = new FilamentContext())
                    {
                        context.Database.Migrate();
                    }
                    //NeedMigration = DbExists;
                }
                catch (Exception )
                {
                    throw ;
                }
                finally
                {
                    NeedMigration = DbExists;
                }
            //}
        }
        private bool DbExists => File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Filament", "FilamentData.db"));
        //    internal static void AddAll(params object[] items)
        //    {
        //        using (FilamentContext context = new FilamentContext())
        //        {
        //            foreach (object item in items)
        //            {
        //                if (item is IEnumerable enumerable)
        //                    foreach (var ele in enumerable)
        //                    {
        //                        context.Entry(ele).State = EntityState.Added;
        //                        context.Add(ele);
        //                    }
        //                else
        //                {
        //                    context.Entry(item).State = EntityState.Added;
        //                    context.Add(item);
        //                }
        //            }
        //            context.SaveChanges();
        //        }
        //    }
        //    public static List<FilamentDefn>? GetAllFilaments()
        //    {
        //        using (FilamentContext ctx = new FilamentContext())
        //        {
        //            if (ctx != null)
        //                return ctx.FilamentDefns?
        //                    .Include("DensityAlias")
        //                    .Include("DensityAlias.MeasuredDensity")
        //                    .AsNoTracking()
        //                    .ToList();
        //            else
        //                return null;
        //        }
        //    }
        //    public static List<FilamentDefn>? GetFilaments(Func<FilamentDefn, bool> predicate)
        //    {
        //        using (FilamentContext ctx = new FilamentContext())
        //        {
        //            if (ctx != null)
        //            {
        //                return ctx.FilamentDefns?
        //                    .AsNoTracking()
        //                    .Include("DensityAlias")
        //                    .Include("DensityAlias.MeasuredDensity")
        //                    .Where<FilamentDefn>(predicate)
        //                    .ToList();
        //            }
        //            else
        //                return null;
        //        }
        //    }
        //    public static FilamentDefn? GetFilament(Func<FilamentDefn, bool> func)
        //    {
        //        return GetFilaments(func)?.FirstOrDefault();
        //    }
        //    public static List<Setting>? GetAllSettings()
        //    {
        //        using (FilamentContext ctx = new FilamentContext())
        //        {
        //            if (ctx != null)
        //            {
        //                return ctx.Settings?
        //                    .AsNoTracking()
        //                    .ToList();
        //            }
        //            else
        //                return null;
        //        }
        //    }
        //    public static Setting? GetSetting(Func<Setting, bool> predicate)
        //    {
        //        using (FilamentContext context = new FilamentContext())
        //        {
        //            if (context != null)
        //            {
        //                return context.Settings?
        //                    .AsNoTracking()
        //                    .Where<Setting>(predicate)
        //                    .SingleOrDefault();
        //            }
        //            else
        //                return null;
        //        }
        //    }
        //    public static List<Setting>? GetSettings(Func<Setting, bool> predicate)
        //    {
        //        using (FilamentContext context = new FilamentContext())
        //        {
        //            if (context != null)
        //            {
        //                return context.Settings?
        //                    .AsNoTracking()
        //                    .Where(predicate)
        //                    .ToList();
        //            }
        //            else
        //                return null;
        //        }
        //    }
        //    public static List<VendorDefn>? GetAllVendors()
        //    {
        //        using (FilamentContext ctx = new FilamentContext())
        //        {
        //            if (ctx != null)
        //            {
        //                return ctx.VendorDefns?
        //                    .AsNoTracking()
        //                    .Include("SpoolDefns")
        //                    .Include("SpoolDefns.Inventory")
        //                    .Include("SpoolDefns.Inventory.DepthMeasurements")
        //                    .ToList();
        //            }
        //            else
        //                return null;
        //        }
        //    }
        //    public static List<VendorDefn>? GetSomeVendors(Func<VendorDefn, bool> predicate)
        //    {
        //        using (FilamentContext ctx = new FilamentContext())
        //        {
        //            if (ctx != null)
        //            {
        //                return ctx.VendorDefns?
        //                    .AsNoTracking()
        //                    .Include("SpoolDefns")
        //                    .Include("SpoolDefns.Inventory")
        //                    .Include("SpoolDefns.Inventory.DepthMeasurements")
        //                    .Where(predicate)
        //                    .ToList();
        //            }
        //            else
        //                return null;
        //        }
        //    }
        //    public static VendorDefn? GetVendor(Func<VendorDefn, bool> predicate)
        //    {
        //        return GetSomeVendors(predicate)?.FirstOrDefault();
        //    }
    }

}