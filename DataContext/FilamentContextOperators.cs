using System;
using System.Collections.Generic;
using System.Text;
using DataDefinitions.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections;

namespace DataContext
{
    public partial class FilamentContext : DataDefinitions.BaseFilamentContext
    {
        //static string DbNameAndLocation = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "FilamentDataCmb.db");
        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<FilamentDefn>()
        //        .HasOne(fd => fd.DensityAlias)
        //        .WithOne(da => da.FilamentDefn)
        //        .HasForeignKey<DensityAlias>(da => da.FilamentDefnId);

        //    //modelBuilder.Entity<SpoolDefn>()
        //    //     .HasOne<VendorDefn>()
        //    //     .WithMany(sd=>sd.SpoolDefns)
        //    //     .HasForeignKey(sd=>sd.VendorDefnId);

        //}
        //public Setting GetSetting(Func<Setting,bool> func)
        //{
        //    if (func != null)
        //        return Settings.FirstOrDefault(s => func(s));
        //    else
        //        return null;
        //}
        //internal static void AddAll(params object[] items)
        //{
        //    using (FilamentContext context = new FilamentContext())
        //    {
        //        foreach (object item in items)
        //        {
        //            if (item is IEnumerable enumerable)
        //                foreach (var ele in enumerable)
        //                {
        //                    context.Entry(ele).State = EntityState.Added;
        //                    context.Add(ele);
        //                }
        //            else
        //            {
        //                context.Entry(item).State = EntityState.Added;
        //                context.Add(item);
        //            }
        //        }
        //        context.SaveChanges();
        //    }
        //}
        //internal static void UpdateItems(params object[] items)
        //{
        //    using (FilamentContext ctx = new FilamentContext())
        //    {
        //        foreach (var item in items)
        //        {
        //            if (item is IEnumerable enumerable)
        //                foreach (var ele in enumerable)
        //                {
        //                    ctx.Entry(ele).State = EntityState.Modified;
        //                    ctx.Update(ele);
        //                }
        //            else
        //            {
        //                ctx.Entry(item).State = EntityState.Modified;
        //                ctx.Update(item);
        //            }
        //        }
        //        ctx.SaveChanges();
        //    }
        //}
        //public static List<FilamentDefn> GetAllFilaments()
        //{
        //    using (FilamentContext ctx = new FilamentContext())
        //    {
        //        if (ctx != null)
        //            return ctx.FilamentDefn?
        //                .Include("DensityAlias")
        //                .Include("DensityAlias.MeasuredDensity")
        //                .AsNoTracking()
        //                .ToList();
        //        else
        //            return null;
        //    }
        //}
        //public static List<FilamentDefn> GetFilaments(Func<FilamentDefn, bool> predicate)
        //{
        //    using (FilamentContext ctx = new FilamentContext())
        //    {
        //        if (ctx != null)
        //        {
        //            return ctx.FilamentDefn?
        //                .AsNoTracking()
        //                .Include("DensityAlias")
        //                .Include("DensityAlias.MeasuredDensity")
        //                .Where<FilamentDefn>(predicate)
        //                .ToList();
        //        }
        //        else
        //            return null;
        //    }
        //}
        //public static FilamentDefn GetFilament(Func<FilamentDefn, bool> func)
        //{
        //    return GetFilaments(func)?.FirstOrDefault();
        //}
        //public static List<Setting> GetAllSettings()
        //{
        //    using (FilamentContext ctx = new FilamentContext())
        //    {
        //        if (ctx != null)
        //        {
        //            return ctx.Settings?
        //                .AsNoTracking()
        //                .ToList();
        //        }
        //        else
        //            return null;
        //    }
        //}
        //public static Setting GetSetting(Func<Setting, bool> predicate)
        //{
        //    using (FilamentContext context = new FilamentContext())
        //    {
        //        if (context != null)
        //        {
        //            return context.Settings?
        //                .AsNoTracking()
        //                .Where<Setting>(predicate)
        //                .SingleOrDefault();
        //        }
        //        else
        //            return null;
        //    }
        //}
        //public static List<Setting> GetSettings(Func<Setting, bool> predicate)
        //{
        //    using (FilamentContext context = new FilamentContext())
        //    {
        //        if (context != null)
        //        {
        //            return context.Settings?
        //                .AsNoTracking()
        //                .Where(predicate)
        //                .ToList();
        //        }
        //        else
        //            return null;
        //    }
        //}
        //public static List<VendorDefn> GetAllVendors()
        //{
        //    using (FilamentContext ctx = new FilamentContext())
        //    {
        //        if (ctx != null)
        //        {
        //            return ctx.VendorDefns?
        //                .AsNoTracking()
        //                .Include("SpoolDefns")
        //                .Include("SpoolDefns.Inventory")
        //                .Include("SpoolDefns.Inventory.DepthMeasurements")
        //                .ToList();
        //        }
        //        else
        //            return null;
        //    }
        //}
        //public static List<VendorDefn> GetSomeVendors(Func<VendorDefn, bool> predicate)
        //{
        //    using (FilamentContext ctx = new FilamentContext())
        //    {
        //        if (ctx != null)
        //        {
        //            return ctx.VendorDefns?
        //                .AsNoTracking()
        //                .Include("SpoolDefns")
        //                .Include("SpoolDefns.Inventory")
        //                .Include("SpoolDefns.Inventory.DepthMeasurements")
        //                .Where(predicate)
        //                .ToList();
        //        }
        //        else
        //            return null;
        //    }
        //}
        //public static VendorDefn GetVendor(Func<VendorDefn, bool> predicate)
        //{
        //    return GetSomeVendors(predicate)?.FirstOrDefault();
        //}

    }
}
