using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Filament_Data.Models;

namespace Filament_Data.DataContext
{
    public class FilamentContext:DbContext
    {
        public DbSet<DensityAlias> DensityAliases { get; set; }
        public DbSet<MeasuredDensity> MeasuredDensity { get; set; }
        public DbSet<FilamentDefn> FilamentDefn { get; set; }

        public string DbNameAndLocation { get; set; }

        public FilamentContext()
        {
            var folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            DbNameAndLocation = System.IO.Path.Combine(folder, "FilamentData.db");
        }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={DbNameAndLocation}");
    }
}
