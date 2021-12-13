﻿// <auto-generated />
using Filament_Db.DataContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Filament_Db.Migrations
{
    [DbContext(typeof(FilamentContext))]
    [Migration("20211210211326_SupportIntrinsic")]
    partial class SupportIntrinsic
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.0");

            modelBuilder.Entity("Filament_Db.Models.DensityAlias", b =>
                {
                    b.Property<int>("DensityAliasId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<double>("DefinedDensity")
                        .HasColumnType("REAL");

                    b.Property<int>("DensityType")
                        .HasColumnType("INTEGER");

                    b.Property<int>("FilamentDefnId")
                        .HasColumnType("INTEGER");

                    b.HasKey("DensityAliasId");

                    b.HasIndex("FilamentDefnId")
                        .IsUnique();

                    b.ToTable("DensityAliases");
                });

            modelBuilder.Entity("Filament_Db.Models.FilamentDefn", b =>
                {
                    b.Property<int>("FilamentDefnId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<double>("Diameter")
                        .HasColumnType("REAL");

                    b.Property<bool>("IsIntrinsic")
                        .HasColumnType("INTEGER");

                    b.Property<int>("MaterialType")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ShelfLifeInDays")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("StopUsing")
                        .HasColumnType("INTEGER");

                    b.HasKey("FilamentDefnId");

                    b.ToTable("FilamentDefn");
                });

            modelBuilder.Entity("Filament_Db.Models.MeasuredDensity", b =>
                {
                    b.Property<int>("MeasuredDensityId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("DensityAliasId")
                        .HasColumnType("INTEGER");

                    b.Property<double>("Diameter")
                        .HasColumnType("REAL");

                    b.Property<double>("Length")
                        .HasColumnType("REAL");

                    b.Property<double>("Weight")
                        .HasColumnType("REAL");

                    b.HasKey("MeasuredDensityId");

                    b.HasIndex("DensityAliasId");

                    b.ToTable("MeasuredDensity");
                });

            modelBuilder.Entity("Filament_Db.Models.Setting", b =>
                {
                    b.Property<int>("SettingId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("SettingId");

                    b.ToTable("Settings");
                });

            modelBuilder.Entity("Filament_Db.Models.VendorDefn", b =>
                {
                    b.Property<int>("VendorDefnId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("FoundOnAmazon")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsIntrinsic")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("StopUsing")
                        .HasColumnType("INTEGER");

                    b.Property<string>("WebUrl")
                        .HasColumnType("TEXT");

                    b.HasKey("VendorDefnId");

                    b.ToTable("VendorDefns");
                });

            modelBuilder.Entity("Filament_Db.Models.DensityAlias", b =>
                {
                    b.HasOne("Filament_Db.Models.FilamentDefn", "FilamentDefn")
                        .WithOne("DensityAlias")
                        .HasForeignKey("Filament_Db.Models.DensityAlias", "FilamentDefnId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("FilamentDefn");
                });

            modelBuilder.Entity("Filament_Db.Models.MeasuredDensity", b =>
                {
                    b.HasOne("Filament_Db.Models.DensityAlias", "DensityAlias")
                        .WithMany("MeasuredDensity")
                        .HasForeignKey("DensityAliasId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DensityAlias");
                });

            modelBuilder.Entity("Filament_Db.Models.DensityAlias", b =>
                {
                    b.Navigation("MeasuredDensity");
                });

            modelBuilder.Entity("Filament_Db.Models.FilamentDefn", b =>
                {
                    b.Navigation("DensityAlias");
                });
#pragma warning restore 612, 618
        }
    }
}
