using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Filament_Data
{
    /// <summary>
    /// determine the density of filament using empirical measurement
    /// </summary>
    public class MeasuredDensity:ObservableObject,IDensity
    {
        private float length;
        /// <summary>
        /// Gets or sets the length.  In millimeters.
        /// </summary>
        /// <value>
        /// The length.
        /// </value>
        public float Length
        {
            get => length;
            set => Set<float>(ref length, value);
        }

        private float diameter;

        /// <summary>
        /// Gets or sets the diameter.  In millimeters.
        /// </summary>
        /// <value>
        /// The diameter.
        /// </value>
        public float Diameter
        {
            get => diameter;
            set => Set<float>(ref diameter, value);
        }

        private float weight;
        /// <summary>
        /// Gets or sets the weight.  Weight of a premeasured amount of filament
        /// </summary>
        /// <value>
        /// The weight.
        /// </value>
        public float Weight
        {
            get => weight;
            set => Set<float>(ref weight, value);
        }

        /// <summary>
        /// Gets the density in gm per cc.
        /// </summary>
        /// <value>
        /// The density in gm per cc.
        /// </value>
        [JsonIgnore]
        public float DensityInGmPerCC { get => (Length > 0 && Diameter > 0 && Weight > 0) ? CalcDensity() : float.NaN; }
        private bool dependenciesIntialized = false;
        [JsonIgnore]
        protected override bool DependenciesInitialized { get => dependenciesIntialized; set => dependenciesIntialized=value; }
        [JsonIgnore]
        public override bool HasDependencies => false;

        //protected override bool DocInitialized => throw new NotImplementedException();
        [JsonIgnore]
        public float Density => DensityInGmPerCC;

        /// <summary>
        /// Initializes a new instance of the <see cref="MeasuredDensity"/> class.
        /// </summary>
        /// <param name="weight">The weight.</param>
        /// <param name="diameter">The diameter.</param>
        /// <param name="length">The length.</param>
        public MeasuredDensity(float weight, float diameter, float length)
        {
            Weight = weight;
            Diameter = diameter;
            Length = length;
        }
        public MeasuredDensity()
        {

        }
        /// <summary>
        /// Calculates the density.
        /// </summary>
        /// <returns></returns>
        protected float CalcDensity()
        {
            //var volume = Math.PI * Math.Pow((double)Diameter / 2, 2) * (double)Length*Constants.ConvertFromCubicMillimetersToCubicCentimeters;
            return Weight / (float)LibraryMath.FilamentVolumeInCubicCentimeters(Diameter / 2, Length);
        }

        protected override void InitDependents()
        {
            List<string> dependentNames = new List<string>() { nameof(DensityInGmPerCC) };
            Dictionary<string, List<string>> localDependencies = new Dictionary<string, List<string>>()
            {
                {nameof(Length), dependentNames},
                {nameof(Diameter),dependentNames },
                {nameof(Weight),dependentNames }
            };
            Dependents.Add(GetType().FullName, localDependencies);
            DependenciesInitialized = true;
            //throw new NotImplementedException();
        }
    }
}
