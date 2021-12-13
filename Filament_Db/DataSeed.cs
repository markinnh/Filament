using System;
using System.Linq;
using Filament_Db.Models;
using Microsoft.EntityFrameworkCore;

namespace Filament_Db
{
    public class DataSeed
    {
        const double InitialSeed = 0.1;
        const double AddVendorDefn = 0.15;
        public static void Seed()
        {
            var setting = DataContext.FilamentContext.GetSetting(s => s.Name == "SeedData");
            if (setting is null)
                InitialSeeding();
            else
            {
                System.Diagnostics.Debug.WriteLine(setting);
                if (setting < AddVendorDefn)
                    SeedVendorData(setting);
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
            if (DataContext.FilamentContext.GetSetting(s => s.Name == "SeedData") is Setting setting)
            {
                if (setting == InitialSeed)
                {
                    if (DataContext.FilamentContext.GetAllFilaments()?.FirstOrDefault() is FilamentDefn definition)
                    {
                        System.Diagnostics.Debug.WriteLine($"Filament Type : {definition.MaterialType}, Density : {definition.DensityAlias?.Density}, Density Type : {definition.DensityAlias.DensityType}");
                    }
                }
            }
            //using(DataContext.FilamentContext ctx = new())
            //{
            //    if (ctx != null)
            //    {
            //        if(ctx.Settings?.FirstOrDefault(s => s.Name == "SeedData") is Setting setting)
            //        {
            //            if (setting == InitialSeed)
            //            {
            //                if(ctx.FilamentDefn?.Include("DensityAlias").Include("DensityAlias.MeasuredDensity").FirstOrDefault() is FilamentDefn definition)
            //                {
            //                    System.Diagnostics.Debug.WriteLine($"Filament Type : {definition.MaterialType}, Density : {definition.DensityAlias.Density}, Density Type : {definition.DensityAlias.DensityType}");
            //                }
            //            }
            //        }
            //    }
            //}
        }
        private static void InitialSeeding()
        {
            var filamentDefns = InitialFilamentDefinitions();
            Filament_Db.DataContext.FilamentContext.AddAll(filamentDefns.Length + 1, filamentDefns, new Setting("SeedData", InitialSeed));
            //FilamentDefn[] startingFilaments = InitialFilamentDefinitions();
            //ctx.AddRange(startingFilaments);
            //ctx.Add(new Setting("SeedData", InitialSeed));
            //ctx.SaveChanges();

        }
        private static void SeedVendorData(Setting setting)
        {
            var seedVendors = InitialVendorDefinitions();

            var insertCount = DataContext.FilamentContext.AddAll(seedVendors.Length, seedVendors);
            System.Diagnostics.Debug.Assert(insertCount == seedVendors.Length, "Not all vendor definitions were inserted into the database.");
            setting.SetValue(AddVendorDefn);
            if (insertCount == seedVendors.Length)
                DataContext.FilamentContext.UpdateItems(setting);
        }
        #region Filament Definition Seed Data
        private static FilamentDefn[] InitialFilamentDefinitions()
        {
            return new FilamentDefn[]
            {
                            new FilamentDefn()
                            {
                                DensityAlias= new DensityAlias()
                                {
                                    DefinedDensity=Constants.BasicPLADensity,
                                    DensityType= DensityType.Defined
                                }
                            },
                            new FilamentDefn()
                            {
                                MaterialType= MaterialType.ABS,
                                DensityAlias= new DensityAlias()
                                {
                                    DefinedDensity = Constants.BasicABSDensity,
                                    DensityType= DensityType.Defined
                                }
                            },
                            new FilamentDefn(MaterialType.PETG)
                            {
                                DensityAlias = new DensityAlias()
                                {
                                    DefinedDensity= Constants.BasicPETGDensity,
                                    DensityType = DensityType.Defined
                                }
                            },
                            new FilamentDefn(MaterialType.Nylon)
                            {
                                DensityAlias = new()
                                {
                                    DensityType= DensityType.Defined,
                                    DefinedDensity= Constants.BasicNylonDensity
                                }
                            },
                            new FilamentDefn(MaterialType.PC)
                            {
                                DensityAlias = new()
                                {
                                    DensityType=DensityType.Defined,
                                    DefinedDensity= Constants.BasicPolycarbonateDensity
                                }
                            },
                            new FilamentDefn(MaterialType.Wood)
                            {
                                DensityAlias = new()
                                {
                                    DensityType = DensityType.Defined,
                                    DefinedDensity = Constants.BasicWoodPLA
                                }
                            }
            };
        }
        #endregion

        #region Vendor Definition Seed Data
        private static VendorDefn[] InitialVendorDefinitions()
        {
            return new VendorDefn[]
            {
                new VendorDefn()
                {
                    Name="3d Solutech",
                    WebUrl="https://www.3dsolutech.com/",
                    FoundOnAmazon=true
                },
                new VendorDefn()
                {
                    Name="HatchBox",
                    WebUrl="https://www.hatchbox3d.com/",
                    FoundOnAmazon=true
                },
                new VendorDefn()
                {
                    Name ="Sunlu",
                    WebUrl="http://www.sunlugw.com/"
                    ,FoundOnAmazon=true
                },
                new VendorDefn()
                {
                    Name="Flashforge",
                    WebUrl ="https://flashforge-usa.com/",
                    FoundOnAmazon=true
                },
                new VendorDefn()
                {
                    Name="Overture",
                    WebUrl="https://overture3d.com/",
                    FoundOnAmazon=true
                },
                new VendorDefn()
                {
                    Name="Amolen",
                    WebUrl="https://amolen.com/",
                    FoundOnAmazon=true
                },
                new VendorDefn()
                {
                    Name="Geetech",
                    WebUrl="https://www.geeetech.com/",
                    FoundOnAmazon=true
                },
                new VendorDefn()
                {
                    Name="eSun",
                    WebUrl="https://esun3dstore.com/collections/3d-printing-filament",
                    FoundOnAmazon=true
                    },
                new VendorDefn()
                {
                    Name="iSanMate",
                    WebUrl="http://www.isanmate.net/index.php?lang=en",
                    FoundOnAmazon=true
                },
                new VendorDefn()
                {
                    Name="Ziro",
                    WebUrl="https://ziro3d.com/",
                    FoundOnAmazon=true
                },
                new VendorDefn(){
                    Name="Gudteks",
                    WebUrl="http://gudteks.com/",
                    FoundOnAmazon=true
                    },
                new VendorDefn()
                    {
                        Name="Eryone",
                        WebUrl="https://eryone3d.com/",
                        FoundOnAmazon=true
                    },
                new VendorDefn()
                {
                    Name = "Reprapper",
                    WebUrl="https://www.reprapwarehouse.com/",
                    FoundOnAmazon=true
                },
                new VendorDefn()
                {
                    Name="Polymaker",
                    WebUrl ="https://polymaker.com/",
                    FoundOnAmazon=true
                },
                new VendorDefn()
                {
                    Name="Aystkniet",
                    FoundOnAmazon=true
                },
                new VendorDefn()
                {
                    Name="Comgrow",
                    WebUrl="https://www.comgrow.com/",
                    FoundOnAmazon=true
                },
                new VendorDefn()
                {
                    Name="Shengtian",
                    FoundOnAmazon=true
                },
                new VendorDefn()
                {
                    Name="Gizmo Dorks",
                    WebUrl="https://gizmodorks.com/pla-3d-printer-filament/",
                    FoundOnAmazon=true
                },
                new VendorDefn()
                {
                    Name="CCTree3d",
                    WebUrl="http://www.cctree3dstore.com/collections/pla-filament-371",
                    FoundOnAmazon=true
                },
                new VendorDefn()
                {
                    Name="Monoprice",
                    WebUrl="https://www.monoprice.com/pages/3d_printers",
                    FoundOnAmazon=true
                },
                new VendorDefn()
                {
                    Name="Makerbot",
                    WebUrl="https://www.makerbot.com/",
                    FoundOnAmazon=true
                },
                new VendorDefn()
                {
                    Name="Ataraxia",
                    WebUrl="https://ataraxiaart.com/",
                    FoundOnAmazon=true
                },
                new VendorDefn()
                {
                    Name="Prusament",
                    WebUrl="https://prusament.com/",
                    FoundOnAmazon=true
                }
            };
        }
        #endregion
    }
}
