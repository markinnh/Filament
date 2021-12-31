using System;
using System.Collections.Generic;
using System.Text;
using DataDefinitions.Models;
namespace DataDefinitions
{
    /// <summary>
    /// This contains the 'seed' data for all data cases
    /// </summary>
    public class Seed
    {
        #region Filament Definition Seed Data
        public static FilamentDefn[] InitialFilamentDefinitions()
        {
            return new FilamentDefn[]
            {
                            new FilamentDefn()
                            {
                                IsIntrinsic = true,
                                DensityAlias= new DensityAlias()
                                {
                                    DefinedDensity=Constants.BasicPLADensity,
                                    DensityType= DensityType.Defined
                                }
                            },
                            new FilamentDefn()
                            {
                                IsIntrinsic = true,
                                MaterialType= MaterialType.ABS,
                                DensityAlias= new DensityAlias()
                                {
                                    DefinedDensity = Constants.BasicABSDensity,
                                    DensityType= DensityType.Defined
                                }
                            },
                            new FilamentDefn(MaterialType.PETG)
                            {
                                IsIntrinsic = true,
                                DensityAlias = new DensityAlias()
                                {
                                    DefinedDensity= Constants.BasicPETGDensity,
                                    DensityType = DensityType.Defined
                                }
                            },
                            new FilamentDefn(MaterialType.Nylon)
                            {
                                IsIntrinsic = true,
                                DensityAlias = new DensityAlias()
                                {
                                    DensityType= DensityType.Defined,
                                    DefinedDensity= Constants.BasicNylonDensity
                                }
                            },
                            new FilamentDefn(MaterialType.PC)
                            {
                                IsIntrinsic = true,
                                DensityAlias = new DensityAlias()
                                {
                                    DensityType=DensityType.Defined,
                                    DefinedDensity= Constants.BasicPolycarbonateDensity
                                }
                            },
                            new FilamentDefn(MaterialType.Wood)
                            {
                                IsIntrinsic = true,
                                DensityAlias = new DensityAlias()
                                {
                                    DensityType = DensityType.Defined,
                                    DefinedDensity = Constants.BasicWoodPLA
                                }
                            }
            };
        }
        #endregion

        #region Vendor Definition Seed Data
        public static VendorDefn[] InitialVendorDefinitions()
        {
            var vendor1 = new VendorDefn()
            {
                Name = "3d Solutech",
                WebUrl = "https://www.3dsolutech.com/",
                FoundOnAmazon = true
            };
            vendor1.SpoolDefns.Add(new SpoolDefn()
            {
                Description = "Black plastic",
                DrumDiameter = 80,
                SpoolDiameter = 200,
                SpoolWidth = 55,
                Vendor = vendor1,
                Weight = 1
            });
            var vendor2 = new VendorDefn()
            {
                Name = "HatchBox",
                WebUrl = "https://www.hatchbox3d.com/",
                FoundOnAmazon = true
            };
            vendor2.SpoolDefns.Add(new SpoolDefn()
            {
                Description = "Black plastic",
                DrumDiameter = 77,
                SpoolDiameter = 200,
                SpoolWidth = 63,
                Weight = 1,
                Vendor = vendor2
            });
            var vendor3 = new VendorDefn()
            {
                Name = "Sunlu",
                WebUrl = "http://www.sunlugw.com/"
                    ,
                FoundOnAmazon = true
            };
            vendor3.SpoolDefns.Add(new SpoolDefn()
            {
                Description = "Clear plastic",
                DrumDiameter = 68,
                SpoolWidth = 55,
                SpoolDiameter = 195,
                Vendor = vendor3,
                Weight = 1
            });
            var vendor4 = new VendorDefn()
            {
                Name = "Flashforge",
                WebUrl = "https://flashforge-usa.com/",
                FoundOnAmazon = true
            };
            vendor4.SpoolDefns.Add(new SpoolDefn()
            {
                Description = "Black plastic",
                DrumDiameter = 69,
                SpoolWidth = 58,
                SpoolDiameter = 200,
                Vendor = vendor4,
                Weight = 1
            });
            return new VendorDefn[]
            {
                vendor1,
                vendor2,
                vendor3,
                vendor4,
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
