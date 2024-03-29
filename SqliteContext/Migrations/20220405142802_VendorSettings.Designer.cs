﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SqliteContext;

namespace SqliteContext.Migrations
{
    [DbContext(typeof(FilamentContext))]
    [Migration("20220405142802_VendorSettings")]
    partial class VendorSettings
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.22");

            modelBuilder.Entity("DataDefinitions.Models.ConfigItem", b =>
                {
                    b.Property<int>("ConfigItemId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("DateEntered")
                        .HasColumnType("TEXT");

                    b.Property<int>("PrintSettingDefnId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("TextValue")
                        .HasColumnType("TEXT");

                    b.Property<int>("VendorSettingsConfigId")
                        .HasColumnType("INTEGER");

                    b.HasKey("ConfigItemId");

                    b.HasIndex("PrintSettingDefnId");

                    b.HasIndex("VendorSettingsConfigId");

                    b.ToTable("ConfigItems");
                });

            modelBuilder.Entity("DataDefinitions.Models.DensityAlias", b =>
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

            modelBuilder.Entity("DataDefinitions.Models.DepthMeasurement", b =>
                {
                    b.Property<int>("DepthMeasurementId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("AdjustForWind")
                        .HasColumnType("INTEGER");

                    b.Property<double>("Depth1")
                        .HasColumnType("REAL");

                    b.Property<double>("Depth2")
                        .HasColumnType("REAL");

                    b.Property<int>("InventorySpoolId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("MeasureDateTime")
                        .HasColumnType("TEXT");

                    b.Property<double>("PercentOffset")
                        .HasColumnType("REAL");

                    b.HasKey("DepthMeasurementId");

                    b.HasIndex("InventorySpoolId");

                    b.ToTable("DepthMeasurements");
                });

            modelBuilder.Entity("DataDefinitions.Models.FilamentDefn", b =>
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

                    b.ToTable("FilamentDefns");
                });

            modelBuilder.Entity("DataDefinitions.Models.InventorySpool", b =>
                {
                    b.Property<int>("InventorySpoolId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ColorName")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DateOpened")
                        .HasColumnType("TEXT");

                    b.Property<int>("FilamentDefnId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("SpoolDefnId")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("StopUsing")
                        .HasColumnType("INTEGER");

                    b.HasKey("InventorySpoolId");

                    b.HasIndex("FilamentDefnId");

                    b.HasIndex("SpoolDefnId");

                    b.ToTable("InventorySpools");
                });

            modelBuilder.Entity("DataDefinitions.Models.MeasuredDensity", b =>
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

            modelBuilder.Entity("DataDefinitions.Models.PrintSettingDefn", b =>
                {
                    b.Property<int>("PrintSettingDefnId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Definition")
                        .HasColumnType("TEXT")
                        .HasMaxLength(128);

                    b.Property<int>("SettingValueType")
                        .HasColumnType("INTEGER");

                    b.HasKey("PrintSettingDefnId");

                    b.ToTable("PrintSettingDefns");
                });

            modelBuilder.Entity("DataDefinitions.Models.Setting", b =>
                {
                    b.Property<int>("SettingId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("Value")
                        .HasColumnType("TEXT");

                    b.HasKey("SettingId");

                    b.ToTable("Settings");
                });

            modelBuilder.Entity("DataDefinitions.Models.SpoolDefn", b =>
                {
                    b.Property<int>("SpoolDefnId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT")
                        .HasMaxLength(128);

                    b.Property<double>("DrumDiameter")
                        .HasColumnType("REAL");

                    b.Property<double>("SpoolDiameter")
                        .HasColumnType("REAL");

                    b.Property<double>("SpoolWidth")
                        .HasColumnType("REAL");

                    b.Property<bool>("StopUsing")
                        .HasColumnType("INTEGER");

                    b.Property<int>("VendorDefnId")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Verified")
                        .HasColumnType("INTEGER");

                    b.Property<double>("Weight")
                        .HasColumnType("REAL");

                    b.HasKey("SpoolDefnId");

                    b.HasIndex("VendorDefnId");

                    b.ToTable("SpoolDefns");
                });

            modelBuilder.Entity("DataDefinitions.Models.VendorDefn", b =>
                {
                    b.Property<int>("VendorDefnId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("FoundOnAmazon")
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

            modelBuilder.Entity("DataDefinitions.Models.VendorSettingsConfig", b =>
                {
                    b.Property<int>("VendorSettingsConfigId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("FilamentDefnId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("VendorDefnId")
                        .HasColumnType("INTEGER");

                    b.HasKey("VendorSettingsConfigId");

                    b.HasIndex("FilamentDefnId");

                    b.HasIndex("VendorDefnId");

                    b.ToTable("VendorSettingsConfigs");
                });

            modelBuilder.Entity("DataDefinitions.Models.ConfigItem", b =>
                {
                    b.HasOne("DataDefinitions.Models.PrintSettingDefn", "PrintSettingDefn")
                        .WithMany()
                        .HasForeignKey("PrintSettingDefnId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DataDefinitions.Models.VendorSettingsConfig", "VendorSettings")
                        .WithMany("ConfigItems")
                        .HasForeignKey("VendorSettingsConfigId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DataDefinitions.Models.DensityAlias", b =>
                {
                    b.HasOne("DataDefinitions.Models.FilamentDefn", "FilamentDefn")
                        .WithOne("DensityAlias")
                        .HasForeignKey("DataDefinitions.Models.DensityAlias", "FilamentDefnId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DataDefinitions.Models.DepthMeasurement", b =>
                {
                    b.HasOne("DataDefinitions.Models.InventorySpool", "InventorySpool")
                        .WithMany("DepthMeasurements")
                        .HasForeignKey("InventorySpoolId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DataDefinitions.Models.InventorySpool", b =>
                {
                    b.HasOne("DataDefinitions.Models.FilamentDefn", "FilamentDefn")
                        .WithMany()
                        .HasForeignKey("FilamentDefnId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DataDefinitions.Models.SpoolDefn", "SpoolDefn")
                        .WithMany("Inventory")
                        .HasForeignKey("SpoolDefnId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DataDefinitions.Models.MeasuredDensity", b =>
                {
                    b.HasOne("DataDefinitions.Models.DensityAlias", "DensityAlias")
                        .WithMany("MeasuredDensity")
                        .HasForeignKey("DensityAliasId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DataDefinitions.Models.SpoolDefn", b =>
                {
                    b.HasOne("DataDefinitions.Models.VendorDefn", "Vendor")
                        .WithMany("SpoolDefns")
                        .HasForeignKey("VendorDefnId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DataDefinitions.Models.VendorSettingsConfig", b =>
                {
                    b.HasOne("DataDefinitions.Models.FilamentDefn", "FilamentDefn")
                        .WithMany()
                        .HasForeignKey("FilamentDefnId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DataDefinitions.Models.VendorDefn", "VendorDefn")
                        .WithMany("VendorSettings")
                        .HasForeignKey("VendorDefnId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
