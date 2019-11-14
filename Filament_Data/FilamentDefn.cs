using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Filament_Data
{
    public enum MaterialType
    {
        /// <summary>
        /// Polylactic acid
        /// </summary>
        PLA,
        /// <summary>
        /// acrylonitrile-butadiene-styrene
        /// </summary>
        ABS,
        /// <summary>
        /// Polyethylene Terephthalate Glycol
        /// </summary>
        PETG,
        Nylon,
        /// <summary>
        /// Thermoplastic Elastomer
        /// </summary>
        TPE,
        /// <summary>
        /// Polycarbonate
        /// </summary>
        PC,
        Wood,
        Metal,
        Bio,
        Conductive,
        /// <summary>
        /// Glow In Dark
        /// </summary>
        GID,
        Magnetic,
        ColorChanging,
        ClayCeramic
    }

    /// <summary>
    /// Default FilamentDefn is Generic PLA
    /// </summary>
    public class FilamentDefn : ObservableObject, ILinkedItem
    {
        /// <summary>
        /// Standard filament diameter in mm
        /// </summary>
        public const float StandardFilamentDiameter = 1.75f;
        /// <summary>
        /// Large filament diameter in mm
        /// </summary>
        public const float LargeFilamentDiameter = 3.0f;
        /// <summary>
        /// Default shelf-life in days
        /// </summary>
        public const int DefaultShelfLife = 180;  // in days

        private int filamentID;
        /// <summary>
        /// Gets or sets the filament identifier.
        /// </summary>
        /// <value>
        /// The filament identifier.
        /// </value>
        public int FilamentID { get => filamentID; set => Set<int>(ref filamentID, value, nameof(FilamentID)); }
        private float diameter = StandardFilamentDiameter;
        /// <summary>
        /// Gets or sets the diameter.
        /// </summary>
        /// <value>
        /// The diameter.
        /// </value>
        public float Diameter { get => diameter; set => Set<float>(ref diameter, value, nameof(Diameter)); }

        //private float density = DefinedDensity.BasicPLADensity;
        ///// <summary>
        ///// Gets or sets the filament density.  Expected in gm/cc, default is 1.24 for generic PLA.
        ///// </summary>
        ///// <value>
        ///// The filament density.
        ///// </value>
        //public float MeasuredDensity { get => DensityCalc?.DensityInGmPerCC ?? density; set => Set(ref density, value, nameof(Filament_Data.MeasuredDensity)); }
        private MaterialType materialType = MaterialType.PLA;
        /// <summary>
        /// Gets or sets the type of the material.
        /// </summary>
        /// <value>
        /// The type of the material.
        /// </value>
        public MaterialType MaterialType { get => materialType; set => Set<MaterialType>(ref materialType, value, nameof(MaterialType)); }
        /// <summary>
        /// Density in grams per cc
        /// </summary>
        [JsonIgnore]
        public float Density => ((IDensity)DensityUnion).Density;
        //private MeasuredDensity densityCalc;
        ///// <summary>
        ///// Will be set if an actual density calculation is performed
        ///// </summary>
        //public MeasuredDensity DensityCalc { get => densityCalc; set => Set<MeasuredDensity>(ref densityCalc, value, nameof(DensityCalc)); }
        private object densityUnion;
        /// <summary>
        /// DensityUnion, constrains to objects the implement the IDensity interface
        /// </summary>
        public object DensityUnion
        {
            get => densityUnion;
            set
            {
                if (value is IDensity)
                    Set<object>(ref densityUnion, value, nameof(DensityUnion));
                else
                    throw new ArgumentException($"{nameof(DensityUnion)} expects an object supporting the IDensity interface");
            }
        }

        private int shelfLife = DefaultShelfLife;
        /// <summary>
        /// Gets or sets the shelf life in days.
        /// </summary>
        /// <value>
        /// The shelf life in days.
        /// </value>
        public int ShelfLifeInDays { get => shelfLife; set => Set<int>(ref shelfLife, value, nameof(ShelfLifeInDays)); }
        [JsonIgnore]
        public IDocument Document { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="FilamentDefn"/> class.
        /// </summary>
        /// <param name="diameter">The diameter.</param>
        /// <param name="density">The density.</param>
        /// <param name="materialType">Type of the material.</param>
        public FilamentDefn(float diameter, float density, MaterialType materialType)
        {
            Diameter = diameter;
            //MeasuredDensity = density;
            DensityUnion = new DefinedDensity(density);
            MaterialType = materialType;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="FilamentDefn"/> class.  Basically defining as a standard PLA filament
        /// </summary>
        public FilamentDefn()
        {
            Diameter = StandardFilamentDiameter;
            DensityUnion = new DefinedDensity(DefinedDensity.BasicPLADensity);
            MaterialType = MaterialType.PLA;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="FilamentDefn"/> class.
        /// </summary>
        /// <param name="materialType"></param>
        public FilamentDefn(MaterialType materialType)
        {
            Diameter = StandardFilamentDiameter;
            MaterialType = materialType;
            if (materialType == MaterialType.PLA)
                DensityUnion = DefinedDensity.BasicPLA;
            else if (materialType == MaterialType.ABS)
                DensityUnion = DefinedDensity.BasicABS;
            else
                DensityUnion = new MeasuredDensity(0f, 0f, 0f);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="FilamentDefn"/> class.
        /// </summary>
        /// <param name="diameter">The diameter.</param>
        /// <param name="densityMeasurement">The density measurement.</param>
        /// <param name="materialType">Type of the material.</param>
        public FilamentDefn(float diameter, MeasuredDensity densityMeasurement, MaterialType materialType)
        {
            Diameter = diameter;
            //DensityCalc = densityMeasurement;
            DensityUnion = densityMeasurement;
            MaterialType = materialType;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="FilamentDefn"/> class.
        /// </summary>
        /// <param name="diameter">The diameter.</param>
        /// <param name="definedDensity">The defined density.</param>
        /// <param name="material">The material.</param>
        public FilamentDefn(float diameter, DefinedDensity definedDensity,MaterialType material)
        {
            Diameter = diameter;
            DensityUnion = definedDensity;
            MaterialType = material;
        }
        /// <summary>
        /// Gets the grams per millimeter.
        /// </summary>
        /// <value>
        /// The grams per millimeter.
        /// </value>
        [JsonIgnore]
        public float GramsPerMillimeter => LibraryMath.FilamentVolumeInCubicCentimeters(Diameter / 2, 1) * Density;
        //[JsonIgnore]
        //protected override bool DocInitialized => Document != null;
        [JsonIgnore]
        public override bool HasDependencies => true;

        private static bool dependenciesInitialized = false;
        [JsonIgnore]
        protected override bool DependenciesInitialized { get => dependenciesInitialized; set => dependenciesInitialized = value; }

        protected float OldCalc()
        {
            return (float)(Math.Pow((double)Diameter / 2, 2) * (double)Density * LibraryMath.ConvertFromCubicMillimetersToCubicCentimeters);
        }

        public void EstablishLink(IDocument document)
        {
            if (document != null)
            {
                Document = document;
                if (FilamentID == 0)
                    FilamentID = document.Counters.NextID(GetType());
            }
            else
                throw new ArgumentNullException($"{nameof(document)} is null, a valid reference to the IDocument interface is expected.");
        }

        public static FilamentDefn CreateAndAddFilamentDefn(float diameter, float density, MaterialType material, IDocument document)
        {

            var result = new FilamentDefn(diameter, density, material);
            result.EstablishLink(document);
            System.Diagnostics.Debug.Assert(result.FilamentID > 0);
            //document.Filaments.Add(result.filamentID, result);
            return result;
        }
        public static FilamentDefn CreateAndAddFilamentDefn(float diameter, DefinedDensity definedDensity,MaterialType material,IDocument document)
        {
            var result = new FilamentDefn(diameter, definedDensity, material);
            result.EstablishLink(document);
            System.Diagnostics.Debug.Assert(result.FilamentID > 0);
            return result;
        }
        protected override void InitDependents()
        {
            List<string> updateNames = new List<string>() { nameof(GramsPerMillimeter) };
            Dictionary<string, List<string>> localDependencies = new Dictionary<string, List<string>>()
            {
                {nameof(MeasuredDensity),updateNames },
                {nameof(MaterialType),updateNames },
                {nameof(Diameter),updateNames },
                {nameof(DensityUnion),new List<string>(){nameof(Density) } }
            };
            Dependents.Add(GetType().FullName, localDependencies);
            DependenciesInitialized = true;
        }
    }
}
