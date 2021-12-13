using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.Linq;
using System.Collections.ObjectModel;
using static System.Diagnostics.Debug;
using Filament_Data.ProjectAttributes;
using System.ComponentModel;
using MyLibraryStandard.Extensions;
using MyLibraryStandard.Attributes;

namespace Filament_Data.JsonModel
{
    // TODO: Develop a UI for Measurement; add, update, delete
    // TODO: Convert Measurement to a SpoolDepthMeasurement class.  
    public class Measurement : ObservableObject, IParentRef<InventorySpool>, IDataErrorInfo
    {
        //const double FilamentDensityInCC = 1.24;
        //const double ConvertFromCCToCMM = 0.001;
        const double FilamentStartingDepth = 12;
        const double PercentOffset = 94;

        public const double ConvertFromMMToMeters = 0.001f;

        private DateTime measureDateTime;
        /// <summary>
        /// Gets or sets the measure date time.
        /// </summary>
        /// <value>
        /// The measure date time.
        /// </value>
        public DateTime MeasureDateTime
        {
            get => measureDateTime;
            set => Set<DateTime>(ref measureDateTime, value);
        }

        private double depth1 = FilamentStartingDepth;
        /// <summary>
        /// Gets or sets the depth1.
        /// </summary>
        /// <value>
        /// The depth1.
        /// </value>
        [Affected(Names =new[] { nameof(FilamentRemainingInGrams), nameof(FilamentRemainingInMeters), nameof(FilamentRemainingInMillimeters) })]
        public double Depth1
        {
            get => depth1;
            set => Set<double>(ref depth1, value);
        }
        private double depth2 = FilamentStartingDepth;
        /// <summary>
        /// Gets or sets the depth2.
        /// </summary>
        /// <value>
        /// The depth2.
        /// </value>
        public double Depth2
        {
            get => depth2;
            set => Set<double>(ref depth2, value);
        }
        //private double percentOffset = 94;
        //public double PercentOffset
        //{
        //    get => percentOffset; set
        //    {
        //        if (percentOffset != value)
        //        {
        //            percentOffset = value;
        //            OnPropertyChanged(nameof(PercentOffset));
        //            UpdateCalcs();
        //        }
        //    }
        //}

        private string comment;

        public string Comment
        {
            get => comment;
            set => Set<string>(ref comment, value, nameof(Comment));
        }

        /// <summary>
        /// Gets the filament remaining in millimeters.
        /// </summary>
        /// <value>
        /// The filament remaining in millimeters.
        /// </value>
        [JsonIgnore]
        public double FilamentRemainingInMillimeters => CalcRemaining();
        /// <summary>
        /// Gets the filament remaining in meters.
        /// </summary>
        /// <value>
        /// The filament remaining in meters.
        /// </value>
        [JsonIgnore]
        public double FilamentRemainingInMeters => (double)Math.Round(CalcRemaining() * ConvertFromMMToMeters, 3);
        /// <summary>
        /// Gets the filament remaining in grams.
        /// </summary>
        /// <value>
        /// The filament remaining in grams.
        /// </value>
        [JsonIgnore]
        public double FilamentRemainingInGrams => CalcGramsRemaining();
        /// <summary>
        /// Gets the depth average.
        /// </summary>
        /// <value>
        /// The depth average.
        /// </value>
        [JsonIgnore]
        public double DepthAverage { get => (Depth1 + Depth2) / 2; }
        //private string spoolName;
        ///// <summary>
        ///// Gets or sets the name of the spool.
        ///// </summary>
        ///// <value>
        ///// The name of the spool.
        ///// </value>
        //public string SpoolName
        //{
        //    get => spoolName;
        //    set => Set<string>(ref spoolName, value, nameof(SpoolName));
        //}
        /// <summary>
        /// Gets the spool definition.
        /// </summary>
        /// <value>
        /// The spool definition.
        /// </value>
        [JsonIgnore]
        public SpoolDefinition SpoolDefinition => ((IParentRef<InventorySpool>)this)?.Parent.SpoolDefn;
        //private SpoolDefinition spoolDefinition;
        //[JsonIgnore]
        //public SpoolDefinition SpoolDefinition
        //{
        //    get => spoolDefinition;
        //    set => Set<SpoolDefinition>(ref spoolDefinition, value, nameof(SpoolDefinition));
        //}
        //[JsonIgnore]
        //protected override bool DocInitialized => Document != null;
        //protected static bool dependenciesInitialized = false;
        //[JsonIgnore]
        //protected override bool DependenciesInitialized { get => dependenciesInitialized; set => dependenciesInitialized = value; }
        //[JsonIgnore]
        //public override bool HasDependencies => true;
        [JsonIgnore]
        InventorySpool IParentRef<InventorySpool>.Parent { get; set; }

        public string Error => null;

        public string this[string columnName]
        {
            get
            {
                var minDepth = 0;
                
                var maxDepth = (SpoolDefinition.SpoolDiameter - SpoolDefinition.DrumDiameter) / 2;

                if (columnName == nameof(Depth1) && !Depth1.Between(minDepth,maxDepth))
                    return $"Depth reading must be between {minDepth} and {maxDepth}";
                else if (columnName == nameof(Depth2) && !Depth2.Between(minDepth,maxDepth))
                    return $"Depth reading must be between {minDepth} and {maxDepth}";
                else
                    return null;
            }
        }

        internal bool IsValidNext(Measurement check)
        {
            //var localAverage = leftDepth + rightDepth / 2;
            return check.measureDateTime > measureDateTime && check.DepthAverage <= DepthAverage ? true : false;
        }
        public Measurement()
        {
            Init();
        }
        protected double CalcRemaining()
        {
            //var lostSpool = 10;
            var percentUtilization = .95f;
            double length = 0.0f;
            double windAmount = (SpoolDefinition.SpoolWidth / SpoolDefinition.Filament.Diameter) * percentUtilization;
            var maxDiameter = SpoolDefinition.SpoolDiameter - (2 * DepthAverage);
            var curDiameter = SpoolDefinition.DrumDiameter + SpoolDefinition.Filament.Diameter / 2;
            var loop = 0;
            while (curDiameter < maxDiameter)
            {
                length += windAmount * (double)Math.PI * curDiameter;
                curDiameter += SpoolDefinition.Filament.Diameter * 2 * PercentOffset / 100;
                // adjust for gain from stepping up while wind
                if (loop > 0)
                    length += SpoolDefinition.Filament.Diameter * 2;
                loop++;
            }
            WriteLine($"Loop count : {loop}");
            return length;
            //throw new NotImplementedException();
        }
        protected double CalcGramsRemaining()
        {
            //var gramsPerMillimeter = Math.Pow((double)SpoolDefinition.Filament.Diameter / 2, 2) * Math.PI * (double)SpoolDefinition.Filament.Density * ConvertFromCCToCMM;
            return (double)Math.Round(SpoolDefinition.Filament.MgPerMM/1000 * FilamentRemainingInMillimeters, 3);
            //throw new NotImplementedException();
        }

        //protected override void InitDependents()
        //{

        //    var stdList = new List<string>() { nameof(FilamentRemainingInGrams), nameof(FilamentRemainingInMeters), nameof(FilamentRemainingInMillimeters) };
        //    var dictionary = new Dictionary<string, List<string>>()
        //    {
        //        {nameof(Depth1),stdList },
        //        {nameof(Depth2),stdList }
        //    };
        //    Dependents.Add(GetType().FullName, dictionary);
        //    DependenciesInitialized = true;
        //    throw new NotImplementedException();
        //}

    }
}
