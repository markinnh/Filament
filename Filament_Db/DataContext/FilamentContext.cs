using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Filament_Db.Models;
using System.Collections;
using System.Threading.Tasks;
using System.Linq;

namespace Filament_Db.DataContext
{
    public partial class FilamentContext : DbContext
    {
        public DbSet<DensityAlias>? DensityAliases { get; set; }
        public DbSet<MeasuredDensity>? MeasuredDensity { get; set; }
        public DbSet<FilamentDefn>? FilamentDefn { get; set; }
        public DbSet<Setting>? Settings { get; set; }
        public DbSet<VendorDefn>? VendorDefns { get; set; }
        public DbSet<InventorySpool>? InventorySpools { get; set; }
        public DbSet<DepthMeasurement>? DepthMeasurement { get; set; }
        public static string? DbNameAndLocation { get; set; }

        public FilamentContext()
        {
            var folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            DbNameAndLocation = System.IO.Path.Combine(folder, "FilamentData.db");
        }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={DbNameAndLocation}");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FilamentDefn>()
                .HasOne(fd => fd.DensityAlias)
                .WithOne(da => da.FilamentDefn)
                .HasForeignKey<DensityAlias>(da => da.FilamentDefnId);

            //modelBuilder.Entity<SpoolDefn>()
            //     .HasOne<VendorDefn>()
            //     .WithMany(sd=>sd.SpoolDefns)
            //     .HasForeignKey(sd=>sd.VendorDefnId);
                                
        }

        public static int AddAll(int expectedItems, params object[] items)
        {
            using (FilamentContext context = new FilamentContext())
            {
                if (context != null)
                {

                    foreach (var item in items)
                    {
                        if (item is IEnumerable ts)
                            context.AddRange(ts);
                        else
                            context.Add(item);
                    }
                    return context.SaveChanges();
                }
                else
                    return 0;
            }
        }
        public static int UpdateItems(params object[] items)
        {
            using (FilamentContext ctx = new FilamentContext())
            {
                if (ctx != null)
                {
                    foreach (var item in items)
                    {
                        if (item is IEnumerable ts)
                            foreach (var ele in ts)
                            {
                                ctx.Entry(ele).State = EntityState.Modified;
                                ctx.Update(ele);
                            }

                        else
                        {
                            ctx.Entry(item).State = EntityState.Modified;
                            ctx.Update(item);
                        }
                    }
                    return ctx.SaveChanges();
                }
                else
                    return -1;
            }
        }
        public static void UpdateSpec(VendorDefn vendorDefn)
        {
            using(FilamentContext context=new FilamentContext())
            {
                if(context != null)
                {
                    // flag the 'new' objects for added state
                    SetDataItemsState(context, vendorDefn.SpoolDefns.Where(sd => sd.VendorDefnId == default), EntityState.Added);
                    //foreach (var item in vendorDefn.SpoolDefns.Where(sd=>sd.VendorDefnId==default))
                    //{
                    //    context.Entry<SpoolDefn>(item).State = EntityState.Added;

                    //}
                    // flag the 'modified' objects for modified state
                    SetDataItemsState(context, vendorDefn.SpoolDefns.Where(sd => sd.IsModified && sd.VendorDefnId != default), EntityState.Modified);
                    
                    foreach(var sd in vendorDefn.SpoolDefns)
                    {
                        SetDataItemsState<InventorySpool>(context,sd.Inventory.Where(inv=>inv.SpoolDefnId == default), EntityState.Added);
                        SetDataItemsState<InventorySpool>(context,sd.Inventory.Where(inv=>inv.IsModified && inv.SpoolDefnId != default),EntityState.Modified);
                        foreach(var inv in sd.Inventory)
                        {
                            SetDataItemsState<DepthMeasurement>(context,inv.DepthMeasurements.Where(dm=>dm.InventorySpoolId==default), EntityState.Added);
                            SetDataItemsState<DepthMeasurement>(context,inv.DepthMeasurements.Where(dm=>dm.InventorySpoolId!=default&& dm.IsModified), EntityState.Modified);
                        }
                    }
                    //foreach(var item in vendorDefn.SpoolDefns.Where())
                    //    context.Entry<SpoolDefn>(item).State= EntityState.Modified;

                    context.Update<VendorDefn>(vendorDefn);
                    context.SaveChanges();
                }
            }
        }
        public static void UpdateSpec(FilamentDefn filamentDefn)
        {
            using(FilamentContext ctx=new FilamentContext())
            {
                if(ctx != null && filamentDefn.DensityAlias!=null)
                {
                    // flag the 'new' objects for added state
                    SetDataItemsState(ctx, filamentDefn.DensityAlias.MeasuredDensity.Where(d => d.DensityAliasId == default), EntityState.Added);
                    //ctx.Entry<MeasuredDensity>(item).State = EntityState.Added;
                    // flag the 'modified' objects for modified state
                    SetDataItemsState(ctx, filamentDefn.DensityAlias.MeasuredDensity.Where(d => d.DensityAliasId != default),EntityState.Modified);
                    //foreach (var item in filamentDefn.DensityAlias))
                    //{
                    //    ctx.Entry(item).State= EntityState.Modified;
                    //}
                    ctx.Update<FilamentDefn>(filamentDefn);
                    ctx.SaveChanges();
                }
            }
        }
        public static int DeleteItems(params object[] items)
        {
            using (FilamentContext context = new FilamentContext())
            {
                if (context != null)
                {
                    foreach (var item in items)
                    {
                        if (item is IEnumerable ts)
                        {
                            foreach (var ele in ts)
                                context.Remove(ele);
                        }
                        else
                            context.Remove(item);
                    }
                    return context.SaveChanges();
                }
                else
                    return -1;
            }
        }
        public static FilamentDefn[]? GetAllFilaments()
        {
            using (FilamentContext ctx = new FilamentContext())
            {
                if (ctx != null)
                    return ctx.FilamentDefn?
                        .Include("DensityAlias")
                        .Include("DensityAlias.MeasuredDensity")
                        .AsNoTracking()
                        .ToArray();
                else
                    return null;
            }
        }
        public static List<FilamentDefn>? GetFilaments(Func<FilamentDefn, bool> predicate)
        {
            using (FilamentContext ctx = new FilamentContext())
            {
                if (ctx != null)
                {
                    return ctx.FilamentDefn?
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
        public static FilamentDefn? GetFilament(Func<FilamentDefn, bool> func)
        {
            return GetFilaments(func)?.FirstOrDefault();
        }
        public static List<Setting>? GetAllSettings()
        {
            using (FilamentContext ctx = new())
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
        public static Setting? GetSetting(Func<Setting, bool> predicate)
        {
            using (FilamentContext context = new())
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
        public static List<Setting>? GetSettings(Func<Setting, bool> predicate)
        {
            using (FilamentContext context = new())
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
        public static List<VendorDefn>? GetAllVendors()
        {
            using (FilamentContext ctx = new())
            {
                if (ctx != null)
                {
                    return ctx.VendorDefns?
                        .AsNoTracking()
                        .Include("SpoolDefns")
                        .Include("SpoolDefns.Inventory")
                        .Include("SpoolDefns.Inventory.DepthMeasurements")
                        .ToList();
                }
                else
                    return null;
            }
        }
        public static List<VendorDefn>? GetSomeVendors(Func<VendorDefn, bool> predicate)
        {
            using (FilamentContext ctx = new())
            {
                if (ctx != null)
                {
                    return ctx.VendorDefns?
                        .AsNoTracking()
                        .Include("SpoolDefns")
                        .Include("SpoolDefns.Inventory")
                        .Include("SpoolDefns.Inventory.DepthMeasurements")
                        .Where(predicate)
                        .ToList();
                }
                else
                    return null;
            }
        }
        public static VendorDefn? GetVendor(Func<VendorDefn, bool> predicate)
        {
            return GetSomeVendors(predicate)?.FirstOrDefault();
        }
        public static void SetDataItemsState<TItem>(FilamentContext context,IEnumerable<TItem> items,EntityState state) where TItem : class
        {
            if(items != null)
                foreach(TItem item in items)
                    context.Entry<TItem>(item).State = state;
        }
    }
}
