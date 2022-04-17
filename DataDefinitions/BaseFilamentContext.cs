using Microsoft.EntityFrameworkCore;
using DataDefinitions.Models;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

namespace DataDefinitions
{
    public class BaseFilamentContext : DbContext
    {
        public DbSet<FilamentDefn> FilamentDefns { get; set; }
        public DbSet<SpoolDefn> SpoolDefns { get; set; }
        public DbSet<VendorDefn> VendorDefns { get; set; }
        public DbSet<InventorySpool> InventorySpools { get; set; }
        public DbSet<DensityAlias> DensityAliases { get; set; }
        public DbSet<DepthMeasurement> DepthMeasurements { get; set; }
        public DbSet<MeasuredDensity> MeasuredDensity { get; set; }
        public DbSet<Setting> Settings { get; set; }
    
        public DbSet<PrintSettingDefn> PrintSettingDefns { get; set; }
        public DbSet<VendorSettingsConfig> VendorSettingsConfigs { get; set; }
        public DbSet<ConfigItem> ConfigItems { get; set; }
        public virtual void PerformMigrations()
        {
            throw new NotImplementedException($"PerformMigrations not implemented in {GetType().Name}.");
        }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    //System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Filament");
        //    optionsBuilder.UseSqlite($"Data Source={System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),"Filament","FilamentData.db")};");
        //}
        public static void AddAll<TContext>(params object[] items)where TContext : BaseFilamentContext,new()
        {
            using (BaseFilamentContext context = new TContext())
            {
                foreach (object item in items)
                {
                    if (item is IEnumerable enumerable)
                        foreach (var ele in enumerable)
                        {
                            context.Entry(ele).State = EntityState.Added;
                            context.Add(ele);
                        }
                    else
                    {
                        context.Entry(item).State = EntityState.Added;
                        context.Add(item);
                    }
                }
                context.SaveChanges();
            }
        }
        public static List<FilamentDefn> GetAllFilaments<TContext>() where TContext : BaseFilamentContext,new()
        {
            using (BaseFilamentContext ctx = new TContext())
            {
                if (ctx != null)
                    return ctx.FilamentDefns?
                        .Include("DensityAlias")
                        .Include("DensityAlias.MeasuredDensity")
                        .AsNoTracking()
                        .ToList();
                else
                    return null;
            }
        }
        public static List<FilamentDefn> GetFilaments<TContext>(Func<FilamentDefn, bool> predicate) where TContext : BaseFilamentContext, new()
        {
            using (BaseFilamentContext ctx = new TContext())
            {
                if (ctx != null)
                {
                    return ctx.FilamentDefns?
                        .AsNoTracking()
                        .Include("DensityAlias")
                        .Include("DensityAlias.MeasuredDensity")
                        .Where<FilamentDefn>(predicate)
                        .ToList();
                }
                else
                    return null;
            }
        }
        public static FilamentDefn GetFilament<TContext>(Func<FilamentDefn, bool> func) where TContext : BaseFilamentContext,new()
        {
            return GetFilaments<TContext>(func)?.FirstOrDefault();
        }
        public static List<Setting> GetAllSettings<TContext>() where TContext : BaseFilamentContext,new()
        {
            using (BaseFilamentContext ctx = new TContext())
            {
                if (ctx != null)
                {
                    return ctx.Settings?
                        .AsNoTracking()
                        .ToList();
                }
                else
                    return null;
            }
        }
        public static Setting GetSetting<TContext>(Func<Setting, bool> predicate) where TContext: BaseFilamentContext,new()
        {
            using (BaseFilamentContext context = new TContext())
            {
                if (context != null)
                {
                    return context.Settings?
                        .AsNoTracking()
                        .Where<Setting>(predicate)
                        .SingleOrDefault();
                }
                else
                    return null;
            }
        }
        public static List<Setting> GetSettings<TContext>(Func<Setting, bool> predicate) where TContext:BaseFilamentContext, new()
        {
            using (BaseFilamentContext context = new TContext())
            {
                if (context != null)
                {
                    return context.Settings?
                        .AsNoTracking()
                        .Where(predicate)
                        .ToList();
                }
                else
                    return null;
            }
        }
        public static List<VendorDefn> GetAllVendors<TContext>() where TContext : BaseFilamentContext,new()
        {
            using (BaseFilamentContext ctx = new TContext())
            {
                if (ctx != null)
                {
                    return ctx.VendorDefns?
                        .AsNoTracking()
                        .Include("SpoolDefns")
                        .Include("SpoolDefns.Inventory")
                        .Include("SpoolDefns.Inventory.DepthMeasurements")
                        .Include("VendorSettings")
                        .Include("VendorSettings.ConfigItems")
                        .ToList();
                }
                else
                    return null;
            }
        }
        public static List<VendorDefn> GetSomeVendors<TContext>(Func<VendorDefn, bool> predicate) where TContext : BaseFilamentContext, new()
        {
            using (BaseFilamentContext ctx = new TContext())
            {
                if (ctx != null)
                {
                    return ctx.VendorDefns?
                        .AsNoTracking()
                        .Include("SpoolDefns")
                        .Include("SpoolDefns.Inventory")
                        .Include("SpoolDefns.Inventory.DepthMeasurements")
                        .Include("VendorSettings")
                        .Include("VendorSettings.ConfigItems")
                        .Where(predicate)
                        .ToList();
                }
                else
                    return null;
            }
        }
        public static VendorDefn GetVendor<TContext>(Func<VendorDefn, bool> predicate) where TContext : BaseFilamentContext,new()
        {
            return GetSomeVendors<TContext>(predicate)?.FirstOrDefault();
        }
        public static IEnumerable<PrintSettingDefn> GetAllPrintSettingDefns<TContext>() where TContext : BaseFilamentContext, new()
        {
            using(BaseFilamentContext ctx = new TContext())
            {
                if (ctx != null)
                {
                    return ctx.PrintSettingDefns
                        .AsNoTracking()
                        .ToList();
                }
                else
                    return null;
            }
        }
    }

}