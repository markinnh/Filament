using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using MyLibraryStandard.Attributes;

namespace Filament_Data.Models
{
    /// <summary>
    /// determine the density of filament using empirical measurement
    /// </summary>
    public class MeasuredDensity : Observable, IDensity
    {
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

        /// <summary>
        /// Gets the density in gm per cc.
        /// </summary>
        /// <value>
        /// The density in gm per cc.
        /// </value>
        [JsonIgnore]
        public double DensityInGmPerCC { get => (Length > 0 && Diameter > 0 && Weight > 0) ? CalcDensity() : double.NaN; }
        //static bool dependenciesIntialized = false;
        //[JsonIgnore]
        //protected override bool DependenciesInitialized { get => dependenciesIntialized; set => dependenciesIntialized = value; }
        //[JsonIgnore]
        //public override bool HasDependencies => true;

        //protected override bool DocInitialized => throw new NotImplementedException();
        [JsonIgnore]
        [Affected(Names = new[] { nameof(Density), nameof(DensityInGmPerCC) })]
        public double Density => DensityInGmPerCC;

        /// <summary>
        /// Initializes a new instance of the <see cref="MeasuredDensity"/> class.
        /// </summary>
        /// <param name="weight">The weight.</param>
        /// <param name="diameter">The diameter.</param>
        /// <param name="length">The length.</param>
        public MeasuredDensity(double weight, double length, double diameter=FilamentDefn.StandardFilamentDiameter)
        {
            Weight = weight;
            Length = length;
            Diameter = diameter;
        }
        public MeasuredDensity()
        {
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

        public override int GetHashCode()
        {
            return length.GetHashCode() ^ weight.GetHashCode() ^ diameter.GetHashCode();
        }
    }
}
