using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using DataDefinitions.Models;
namespace DataDefinitions;

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
#if SUPPORTINTERNALSEED
  // This no longer works as we are defining material separately
            new ()
            {
                MaterialType=MaterialType.PLA,
                IsIntrinsic = true,
                Density=Constants.BasicPLADensity
            },
             new ()
             {
                IsIntrinsic = true,
                MaterialType= MaterialType.ABS,
                Density=Constants.BasicABSDensity
             },
             new ()
             {
                IsIntrinsic = true,
                MaterialType= MaterialType.PETG,
                Density=Constants.BasicPETGDensity 
             },
             new ()
             {
                MaterialType = MaterialType.Nylon,
                IsIntrinsic = true,
                Density= Constants.BasicNylonDensity
             },
             new ()
             {
                 MaterialType=MaterialType.PC,
                IsIntrinsic = true,
                Density= Constants.BasicPolycarbonateDensity
             },
             new ()
             {
                MaterialType=MaterialType.Wood,
                IsIntrinsic = true,
                Density = Constants.BasicWoodPLA
             }
             #endif
        };
    }
    #endregion

    #region Vendor Definition Seed Data
    public static VendorDefn[] InitialVendorDefinitions()
    {
#if SUPPORTINTERNALSEED
        var vendor1 = new VendorDefn()
        {
            Name = "3d Solutech",
            WebUrl = String.Empty,
            FoundOnAmazon = true
        };
        var spool1 = new SpoolDefn()
        {
            Description = "Black plastic",
            DrumDiameter = 80,
            SpoolDiameter = 200,
            SpoolWidth = 55,
            Vendor = vendor1,
            Weight = 1
        };
        vendor1.SpoolDefns.Add(spool1);
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
            WebUrl = "http://www.sunlugw.com/",
            Tags = "#rainbow #pla #silkpla #petg #using",
            FoundOnAmazon = true
        };
        vendor3.SpoolDefns.Add(new SpoolDefn()
        {
            Description = "Clear plastic",
            Vendor = vendor3,
            SpoolWidth = 55,
            SpoolDiameter = 195,
            DrumDiameter = 68,
            Weight = 1,
            Verified = false
        });
        vendor3.SpoolDefns.Add(new SpoolDefn()
        {
            Description = "Black Plastic",
            Vendor = vendor3,
            SpoolWidth = 54,
            SpoolDiameter = 197,
            DrumDiameter = 82,
            Weight = 1,
            Verified = false
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
            DrumDiameter = 68,
            SpoolWidth = 58,
            SpoolDiameter = 200,
            Vendor = vendor4,
            Weight = 1
        });
        var vendor5 = new VendorDefn()
        {
            Name = "Prusament",
            WebUrl = "https://prusament.com/",
            FoundOnAmazon = true
        };
        vendor5.SpoolDefns.Add(new SpoolDefn()
        {
            Description = "Black",
            DrumDiameter = 53,
            SpoolWidth = 53,
            SpoolDiameter = 190,
            Vendor = vendor5
        });
        var vendor6 = new VendorDefn()
        {
            Name = "3D-Fuel",
            WebUrl = "https://www.3dfuel.com/collections/pro-pla",
            FoundOnAmazon = true
        };
        // removed to support the training portion of the documentation
        //vendor6.SpoolDefns.Add(new SpoolDefn()
        //{
        //    Description = "Black plastic",
        //    DrumDiameter = 88.2,
        //    SpoolWidth = 55,
        //    SpoolDiameter = 200,
        //    Vendor = vendor6
        //});
        var vendor7 = new VendorDefn()
        {
            Name = "Overture",
            WebUrl = "https://overture3d.com/",
            Tags = "#petg #pla #using",

            FoundOnAmazon = true
        };
        vendor7.SpoolDefns.Add(new SpoolDefn()
        {
            Description = "Black Plastic",
            Vendor = vendor7,
            SpoolWidth = 60,
            SpoolDiameter = 200,
            DrumDiameter = 98,
            Weight = 1,
            Verified = true
        });

        var vendor26 = new VendorDefn()
        {
            Name = "Bambu Lab",
            WebUrl = "https://www.bambulab.com",
            Tags = "#pla #petg #favorite #using",
            FoundOnAmazon = false
        };
        vendor26.SpoolDefns.Add(new SpoolDefn()
        {
            Description = "reusable",
            Vendor = vendor26,
            SpoolWidth = 61,
            SpoolDiameter = 200,
            DrumDiameter = 88,
            Weight = 1,
            Verified = true
        });

        return new VendorDefn[]
        {
            vendor1,
            vendor2,
            vendor3,
            vendor4,
            vendor5,
            vendor6,
            vendor26,
            vendor7,
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
            }

        };
#else
        return Array.Empty<VendorDefn>();
#endif

    }
#endregion

    #region Print Setting Definitions Seed Data
    public static PrintSettingDefn[] InitialPrintSettingDefinitions()
    {
        return new PrintSettingDefn[]
        {
#if SUPPORTINTERNALSEED
            new PrintSettingDefn()
            {
                Definition="Extruder Temperature",
                SettingValueType= SupportedSettingValueType.Integer
            },
            new PrintSettingDefn()
            {
                Definition="Layer Height",
                SettingValueType= SupportedSettingValueType.Float
            },
            new PrintSettingDefn()
            {
                Definition= "Nozzle Diameter",
                SettingValueType = SupportedSettingValueType.Float
            },
            new PrintSettingDefn()
            {
                Definition="Parts Cooling Fan",
                SettingValueType=SupportedSettingValueType.Boolean
            }
            #endif
        };
    }
    #endregion
}
