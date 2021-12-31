
using MyLibraryStandard.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;

namespace DataDefinitions.Models
{
    // this is called from the static part of a class, so there will be no sender provided,
    // all it currently does is say one of the objects is undergoing a database operation of 
    // some sort.
    public delegate void InDataOpsChangedHandler(EventArgs args);
    /// <summary>
    /// Allows for definition of 3d print filament. Default FilamentDefn is Generic PLA
    /// </summary>
    public class FilamentDefn : DatabaseObject
    {

        public static event InDataOpsChangedHandler InDataOpsChanged;

        private static bool inDataOps;
        public static bool InDataOps
        {
            get => inDataOps;
            set
            {
                inDataOps = value;

                // Notify all the FilamentDefn objects of change to InDataOps state, allowing them to update the UI.
                InDataOpsChanged?.Invoke(EventArgs.Empty);
            }
        }
        [JsonIgnore]
        public override bool InDataOperations => InDataOps;
        [JsonIgnore]
        public override bool CanEdit => base.CanEdit && !isIntrinsic;
        [JsonIgnore]
        public override bool InDatabase => FilamentDefnId != default;
        [JsonIgnore]
        public override bool SupportsDelete => true && !IsIntrinsic;
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FilamentDefnId { get; set; }
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
        [Affected(Names = new string[] { nameof(MgPerMM) })]
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
        [NotMapped]
        //public bool EventsMapped => (InDataOpsChanged?.GetInvocationList().Cast<InDataOpsChangedHandler>().Contains(FilamentDefn_InDataOpsChanged) ?? false) && (DensityAlias?.IsLinkedToNotifyContainer ?? false);
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
        [Affected(Names = new string[] { nameof(MgPerMM) })]
        public MaterialType MaterialType { get => materialType; set => Set<MaterialType>(ref materialType, value); }
        ///// <summary>
        ///// Density in grams per cc
        ///// </summary>
        //[JsonIgnore]
        //public abstract double Density { get; set; }
        private DensityAlias densityAlias;
        [Affected(Names = new[] { nameof(MgPerMM) })]
        public DensityAlias DensityAlias
        {
            get => densityAlias;
            set
            {
                if (densityAlias != null)
                    densityAlias.NotifyContainer -= DensityAlias_NotifyContainer;

                Set<DensityAlias>(ref densityAlias, value);

                if (densityAlias != null)
                    densityAlias.NotifyContainer += DensityAlias_NotifyContainer;
            }
        }
        private bool isIntrinsic;

        public bool IsIntrinsic
        {
            get => isIntrinsic;
            set => Set<bool>(ref isIntrinsic, value);
        }
        #region UI Assist Items, not mapped
        [NotMapped,JsonIgnore]
        public bool MeasuredDensityVisible => DensityAlias?.DensityType == DensityType.Measured;
        [NotMapped,JsonIgnore]
        public bool DefinedDensityVisible => DensityAlias?.DensityType == DensityType.Defined;
        #endregion
        private void DensityAlias_NotifyContainer(object sender, MyLibraryStandard.NotifyContainerEventArgs e)
        {
            foreach (var item in e.Names)
            {
                OnPropertyChanged(item);
            }
            //throw new NotImplementedException();
        }
        public void Init()
        {
            if (!InDataOpsChanged?.GetInvocationList().Cast<InDataOpsChangedHandler>().Contains(FilamentDefn_InDataOpsChanged) ?? true)
                InDataOpsChanged += FilamentDefn_InDataOpsChanged;
        }

        private void FilamentDefn_InDataOpsChanged(EventArgs args)
        {
            OnPropertyChanged(nameof(CanEdit));
            //throw new NotImplementedException();
        }
        internal override void WatchContained()
        {
            DensityAlias?.Subscribe(WatchContainedHandler);
            DensityAlias?.WatchContained();
        }
        internal override void UnWatchContained()
        {
            DensityAlias?.Unsubscribe(WatchContainedHandler);
            DensityAlias?.UnWatchContained();
        }
        protected override void WatchContainedHandler(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(IsModified))
                OnPropertyChanged(nameof(IsModified));
        }
        public void InitNotificationHandler()
        {
            if (densityAlias != null)
                densityAlias.Subscribe(DensityAlias_NotifyContainer);
        }
        public void ReleaseNotificationHandler()
        {
            if (densityAlias != null)
                densityAlias.UnSubscribe(DensityAlias_NotifyContainer);
        }
        ~FilamentDefn()
        {
            if (densityAlias != null)
                densityAlias.UnSubscribe(DensityAlias_NotifyContainer);

            InDataOpsChanged -= FilamentDefn_InDataOpsChanged;
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
        [JsonIgnore]
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
            Init();
            Diameter = StandardFilamentDiameter;
            //DensityUnion = new DefinedDensity(DefinedDensity.BasicPLADensity);
            DensityAlias = new DensityAlias() { DensityType=DensityType.Defined};
            MaterialType = MaterialType.PLA;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="FilamentDefn"/> class.
        /// </summary>
        /// <param name="materialType"></param>
        public FilamentDefn(MaterialType materialType)
        {
            Init();
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
        public FilamentDefn(double diameter,double definedDensity, MaterialType materialType = MaterialType.PLA)
        {
            Init();
            Diameter = diameter;
            DensityAlias = new DensityAlias
            {
                DefinedDensity = definedDensity,
                DensityType = DensityType.Defined
            };
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
        [Description("Milligrams per millimeter")]
        public double MgPerMM => DensityAlias != null ? FilamentMath.FilamentVolumeInCubicCentimeters(Diameter / 2, 1.0) * DensityAlias.Density * 1000 : double.NaN;

        public override void UpdateItem<TContext>()
        {
            using (TContext context = new TContext())
            {
                InDataOps = true;

                if (InDatabase) { 
                    context.Entry(this).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    context.Entry(this.DensityAlias).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    context.Update(this);
                }
                else
                { 
                    context.Entry(this).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                    context.Entry(this.DensityAlias).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                    context.Add(this);
                }

                //context.Update(this);
                context.SaveChanges();

                IsModified = false;
                InDataOps = false;
            }
        }

        public static void SetDataOperationsState(bool state)
        {
            InDataOps = state;
        }
    }
}
