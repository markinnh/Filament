using System;
using System.Collections.Generic;
using System.ComponentModel;

using System.Text;
using System.Text.Json.Serialization;
using MyLibraryStandard.Attributes;

namespace DataDefinitions.Models
{
    /// <summary>
    /// determine the density of filament using empirical measurement
    /// </summary>
    public class MeasuredDensity : DataDefinitions.DatabaseObject, IDensity, IEditableObject
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
        public override bool InDataOperations => InDataOps;
        [JsonIgnore]
        public static dynamic Initializer { get; set; }
        private int measuredDensityId;

        public int MeasuredDensityId
        {
            get => measuredDensityId;
            set => Set<int>(ref measuredDensityId, value);
        }
        public int DensityAliasId { get; set; }

        public DensityAlias DensityAlias { get; set; }

        private double length;
        /// <summary>
        /// Gets or sets the length.  In millimeters.
        /// </summary>
        /// <value>
        /// The length.
        /// </value>
        [Affected(Names = new[] { nameof(Density), nameof(DensityInGmPerCC) })]
        public double Length
        {
            get => length;
            set => Set<double>(ref length, value);
        }
        // TODO: Consider removing diameter from MeasuredDensity and use the diameter in FilamentDefn
        private double diameter;

        /// <summary>
        /// Gets or sets the diameter.  In millimeters.
        /// </summary>
        /// <value>
        /// The diameter.
        /// </value>
        [Affected(Names = new[] { nameof(Density), nameof(DensityInGmPerCC) })]
        public double Diameter
        {
            get => diameter;
            set => Set<double>(ref diameter, value);
        }

        private double weight;
        /// <summary>
        /// Gets or sets the weight.  Weight of a premeasured amount of filament
        /// </summary>
        /// <value>
        /// The weight.
        /// </value>
        [Affected(Names = new string[] { nameof(DensityInGmPerCC), nameof(Density) })]
        public double Weight
        {
            get => weight;
            set => Set<double>(ref weight, value);
        }
        internal override int KeyID { get => measuredDensityId; set => Set(ref measuredDensityId, value); }
        /// <summary>
        /// Gets the density in gm per cc.
        /// </summary>
        /// <value>
        /// The density in gm per cc.
        /// </value>

        public double DensityInGmPerCC { get => (Length > 0 && Diameter > 0 && Weight > 0) ? CalcDensity() : double.NaN; }
        //static bool dependenciesIntialized = false;
        //[JsonIgnore]
        //protected override bool DependenciesInitialized { get => dependenciesIntialized; set => dependenciesIntialized = value; }
        //[JsonIgnore]
        //public override bool HasDependencies => true;

        //protected override bool DocInitialized => throw new NotImplementedException();

        [Affected(Names = new[] { nameof(Density), nameof(DensityInGmPerCC) })]
        public double Density => DensityInGmPerCC;

        /// <summary>
        /// Initializes a new instance of the <see cref="MeasuredDensity"/> class.
        /// </summary>
        /// <param name="weight">The weight.</param>
        /// <param name="diameter">The diameter.</param>
        /// <param name="length">The length.</param>
        public MeasuredDensity(double weight, double length, double diameter = FilamentDefn.StandardFilamentDiameter)
        {
            Weight = weight;
            Length = length;
            Diameter = diameter;
        }
        public MeasuredDensity()
        {
            // this allows initialization using the larger diameter size if using the UI

            Diameter = FilamentDefn.StandardFilamentDiameter;
            //Init();
        }
        /// <summary>
        /// Calculates the density.
        /// </summary>
        /// <returns>density in grams per cubic centimeter</returns>
        protected double CalcDensity() => Weight / FilamentMath.FilamentVolumeInCubicCentimeters(Diameter / 2, Length);



        //protected override void InitDependents()
        //{
        //    List<string> dependentNames = new List<string>() { nameof(DensityInGmPerCC), nameof(Density) };
        //    Dictionary<string, List<string>> localDependencies = new Dictionary<string, List<string>>()
        //    {
        //        {nameof(Length), dependentNames},
        //        {nameof(Diameter),dependentNames },
        //        {nameof(Weight),dependentNames }
        //    };
        //    Dependents.Add(GetType().FullName, localDependencies);
        //    DependenciesInitialized = true;
        //    //throw new NotImplementedException();
        //}
        public static implicit operator double(MeasuredDensity measuredDensity) => measuredDensity.Density;

        public static bool operator ==(MeasuredDensity left, MeasuredDensity right)
        {
            return (ReferenceEquals(left, null) || ReferenceEquals(right, null)) ? false : left.diameter == right.diameter && left.length == right.length && left.weight == right.weight;
        }
        public static bool operator !=(MeasuredDensity left, MeasuredDensity right)
        {
            return !(left == right);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (ReferenceEquals(obj, null))
            {
                return false;
            }
            if (obj is MeasuredDensity measuredDensity)
                return this == measuredDensity;
            else
                return false;
            //throw new NotImplementedException();
        }
        //protected override void AssignKey(int myId)
        //{
        //    if (MeasuredDensityId == default)
        //        MeasuredDensityId = myId;
        //    else
        //        ReportKeyAlreadyInitialized();

        //}
        public override int GetHashCode()
        {
            return length.GetHashCode() ^ weight.GetHashCode() ^ diameter.GetHashCode();
        }
        #region IEditableObject Implementation
        struct BackupData
        {
            public double Length { get; set; }
            public double Weight { get; set; }
            public double Diameter { get; set; }

            internal BackupData(double length, double weight, double diameter)
            {
                Length = length;
                Weight = weight;
                Diameter = diameter;
            }
            internal BackupData(MeasuredDensity measuredDensity)
            {
                Length = measuredDensity.Length;
                Weight = measuredDensity.Weight;
                Diameter = measuredDensity.Diameter;
            }
        }
        BackupData backupData;
        void IEditableObject.BeginEdit()
        {
            if (!InEdit)
            {
                backupData = new BackupData(this);
                InEdit = true;
            }
            //throw new NotImplementedException();
        }

        void IEditableObject.CancelEdit()
        {
            if (InEdit)
            {
                Length = backupData.Length;
                Weight = backupData.Weight;
                Diameter = backupData.Diameter;
                backupData = default(BackupData);
                InEdit = false;
            }
            //throw new NotImplementedException();
        }

        void IEditableObject.EndEdit()
        {
            if (InEdit)
            {
                backupData = default(BackupData);
                InEdit = false;
            }
            //throw new NotImplementedException();
        }
        #endregion
    }
}
