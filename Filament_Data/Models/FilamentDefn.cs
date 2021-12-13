using MyLibraryStandard.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Filament_Data.Models
{
    /// <summary>
    /// Default FilamentDefn is Generic PLA
    /// </summary>
    public class FilamentDefn :Observable
    {
        /// <summary>
        /// Standard filament diameter in mm
        /// </summary>
        public const double StandardFilamentDiameter = 1.75;
        /// <summary>
        /// Large filament diameter in mm
        /// </summary>
        public const double LargeFilamentDiameter = 3.0;
        /// <summary>
        /// Default shelf-life in days
        /// </summary>
        public const int DefaultShelfLife = 180;  // in days

        private double diameter = StandardFilamentDiameter;
        /// <summary>
        /// Gets or sets the diameter.
        /// </summary>
        /// <value>
        /// The diameter.
        /// </value>
        [Affected(Names =new string[] {nameof(MgPerMM)})]
        public double Diameter { get => diameter; set => Set<double>(ref diameter, value); }

        private bool stopUsing;
        /// <summary>
        /// Gets or sets a value indicating whether to stop using.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [stop using]; otherwise, <c>false</c>.
        /// </value>
        public bool StopUsing
        {
            get => stopUsing;
            set => Set<bool>(ref stopUsing, value);
        }

        //private double density = DefinedDensity.BasicPLADensity;
        ///// <summary>
        ///// Gets or sets the filament density.  Expected in gm/cc, default is 1.24 for generic PLA.
        ///// </summary>
        ///// <value>
        ///// The filament density.
        ///// </value>
        //public double MeasuredDensity { get => DensityCalc?.DensityInGmPerCC ?? density; set => Set(ref density, value, nameof(Filament_Data.MeasuredDensity)); }
        private MaterialType materialType = MaterialType.PLA;
        /// <summary>
        /// Gets or sets the type of the material.
        /// </summary>
        /// <value>
        /// The type of the material.
        /// </value>
        [Affected(Names = new string[] {nameof(MgPerMM)})]
        public MaterialType MaterialType { get => materialType; set => Set<MaterialType>(ref materialType, value); }
        ///// <summary>
        ///// Density in grams per cc
        ///// </summary>
        //[JsonIgnore]
        //public abstract double Density { get; set; }
        private DensityAlias densityAlias;
        // TODO: This will require a one-to-one mapping in the OnModelCreating method, if not defined.
        public DensityAlias DensityAlias
        {
            get => densityAlias;
            set => Set<DensityAlias>(ref densityAlias, value);
        }

        // TODO: Fix before serialization works.  This cannot be an object, it must be a defined type or json serialization will fail.  Its probably a bad programming practice too.  
        //private MeasuredDensity densityCalc;
        ///// <summary>
        ///// Will be set if an actual density calculation is performed
        ///// </summary>
        //public MeasuredDensity DensityCalc { get => densityCalc; set => Set<MeasuredDensity>(ref densityCalc, value, nameof(DensityCalc)); }

        private int shelfLife = DefaultShelfLife;
        /// <summary>
        /// Gets or sets the shelf life in days.
        /// </summary>
        /// <value>
        /// The shelf life in days.
        /// </value>
        public int ShelfLifeInDays { get => shelfLife; set => Set<int>(ref shelfLife, value); }
        /// <summary>
        /// Initializes a new instance of the <see cref="FilamentDefn"/> class.
        /// </summary>
        /// <param name="diameter">The diameter.</param>
        /// <param name="density">The density.</param>
        /// <param name="materialType">Type of the material.</param>
        //public FilamentDefn(double diameter, double density, MaterialType materialType)
        //{
        //    Init();
        //    Diameter = diameter;
        //    //MeasuredDensity = density;
        //    DensityUnion = new DefinedDensity(density);
        //    MaterialType = materialType;
        //}
        /// <summary>
        /// Initializes a new instance of the <see cref="FilamentDefn"/> class.  Basically defining as a standard PLA filament
        /// </summary>
        public FilamentDefn()
        {
            //Init();
            Diameter = StandardFilamentDiameter;
            //DensityUnion = new DefinedDensity(DefinedDensity.BasicPLADensity);
            MaterialType = MaterialType.PLA;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="FilamentDefn"/> class.
        /// </summary>
        /// <param name="materialType"></param>
        public FilamentDefn(MaterialType materialType)
        {
            //Init();
            Diameter = StandardFilamentDiameter;
            MaterialType = materialType;
            //if (materialType == MaterialType.PLA)
            //    DensityUnion = (DefinedDensity)DefinedDensity.BasicPLADensity;
            //else if (materialType == MaterialType.ABS)
            //    DensityUnion = (DefinedDensity)DefinedDensity.BasicABSDensity;
            //else
            //    DensityUnion = new AverageMeasuredDensity(new MeasuredDensity[] { new MeasuredDensity(0f, 0f, 0f) });
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="FilamentDefn"/> class.
        /// </summary>
        /// <param name="diameter">The diameter.</param>
        /// <param name="densityMeasurement">The density measurement.</param>
        /// <param name="materialType">Type of the material.</param>
        public FilamentDefn(double diameter, MaterialType materialType=MaterialType.PLA)
        {
            //Init();
            Diameter = diameter;
            //DensityCalc = densityMeasurement;
            //DensityUnion = new AverageMeasuredDensity(new MeasuredDensity[] { densityMeasurement });
            MaterialType = materialType;
        }
         /// <summary>
        /// Gets the milligrams per millimeter.
        /// </summary>
        /// <value>
        /// The milligrams per millimeter (mg/mm)
        /// </value>
        [JsonIgnore,Description("Milligrams per millimeter")]
        public double MgPerMM => FilamentMath.FilamentVolumeInCubicCentimeters(Diameter / 2, 1.0) * DensityAlias.Density*1000;
        //[JsonIgnore]
        //protected override bool DocInitialized => Document != null;
        //[JsonIgnore]
        //public override bool HasDependencies => true;

        //private static bool dependenciesInitialized = false;
        //[JsonIgnore]
        //protected override bool DependenciesInitialized { get => dependenciesInitialized; set => dependenciesInitialized = value; }

        //protected double OldCalc()
        //{
        //    return (Math.Pow((double)Diameter / 2, 2) * (double)Density * FilamentMath.ConvertFromCubicMillimetersToCubicCentimeters);
        //}

        /*
        public static FilamentDefn CreateAndAddFilamentDefn(double diameter, double density, MaterialType material, IDocument document)
        {

            var result = new FilamentDefn(diameter, density, material);
            result.EstablishLink(document);
            System.Diagnostics.Debug.Assert(result.FilamentID > 0);
            //document.Filaments.Add(result.filamentID, result);
            return result;
        }
        public static FilamentDefn CreateAndAddFilamentDefn(IDocument document,double density,MaterialType material=MaterialType.PLA,double diameter = StandardFilamentDiameter)
        {
            var result = new FilamentDefn(diameter, density, material);
            result.EstablishLink(document);
            System.Diagnostics.Debug.Assert(result.FilamentID > 0);
            return result;
        }
        public static FilamentDefn CreateAndAddFilamentDefn(double diameter, DefinedDensity definedDensity, MaterialType material, IDocument document)
        {
            var result = new FilamentDefn(diameter, definedDensity, material);
            result.EstablishLink(document);
            System.Diagnostics.Debug.Assert(result.FilamentID > 0);
            return result;
        }
        */
        //protected override void InitDependents()
        //{
        //    List<string> updateNames = new List<string>() { nameof(GramsPerMillimeter) };
        //    Dictionary<string, List<string>> localDependencies = new Dictionary<string, List<string>>()
        //    {
        //        {nameof(MeasuredDensity),updateNames },
        //        {nameof(MaterialType),updateNames },
        //        {nameof(Diameter),updateNames },
        //        {nameof(DensityUnion),new List<string>(){nameof(Density) } }
        //    };
        //    Dependents.Add(GetType().FullName, localDependencies);
        //    DependenciesInitialized = true;
        //}
    }
}
