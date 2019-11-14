using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.Linq;
using System.Collections.ObjectModel;

namespace Filament_Data
{
    public class Measurement : ObservableObject, ILinkedItem,IParentItem<InventorySpool>
    {
        //const double FilamentDensityInCC = 1.24;
        //const double ConvertFromCCToCMM = 0.001;
        const float FilamentStartingDepth = 12;
        const float PercentOffset = 94;
        const float StepAmount = 4;
        public const float ConvertFromMMToMeters = 0.001f;

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
            set => Set<DateTime>(ref measureDateTime, value, nameof(MeasureDateTime));
        }

        private float depth1 = FilamentStartingDepth;
        /// <summary>
        /// Gets or sets the depth1.
        /// </summary>
        /// <value>
        /// The depth1.
        /// </value>
        public float Depth1
        {
            get => depth1;
            set => Set<float>(ref depth1, value, nameof(Depth1));
        }
        private float depth2 = FilamentStartingDepth;
        /// <summary>
        /// Gets or sets the depth2.
        /// </summary>
        /// <value>
        /// The depth2.
        /// </value>
        public float Depth2
        {
            get => depth2;
            set => Set<float>(ref depth2, value, nameof(Depth2));
        }
        //private float percentOffset = 94;
        //public float PercentOffset
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
        public float FilamentRemainingInMillimeters => CalcRemaining();
        /// <summary>
        /// Gets the filament remaining in meters.
        /// </summary>
        /// <value>
        /// The filament remaining in meters.
        /// </value>
        [JsonIgnore]
        public float FilamentRemainingInMeters => (float)Math.Round(CalcRemaining() * ConvertFromMMToMeters, 3);
        /// <summary>
        /// Gets the filament remaining in grams.
        /// </summary>
        /// <value>
        /// The filament remaining in grams.
        /// </value>
        [JsonIgnore]
        public float FilamentRemainingInGrams => CalcGramsRemaining();
        /// <summary>
        /// Gets the depth average.
        /// </summary>
        /// <value>
        /// The depth average.
        /// </value>
        [JsonIgnore]
        public float DepthAverage { get => (Depth1 + Depth2) / 2; }
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
        public SpoolDefinition SpoolDefinition =>Parent?.SpoolDefn;
        //private SpoolDefinition spoolDefinition;
        //[JsonIgnore]
        //public SpoolDefinition SpoolDefinition
        //{
        //    get => spoolDefinition;
        //    set => Set<SpoolDefinition>(ref spoolDefinition, value, nameof(SpoolDefinition));
        //}
        //[JsonIgnore]
        //protected override bool DocInitialized => Document != null;
        protected static bool dependenciesInitialized = false;
        [JsonIgnore]
        protected override bool DependenciesInitialized { get => dependenciesInitialized; set => dependenciesInitialized = value; }
        [JsonIgnore]
        public override bool HasDependencies => true;
        [JsonIgnore]
        public IDocument Document { get ;protected set ; }
        public InventorySpool Parent { get ; set ; }

        protected float CalcRemaining()
        {
            //var lostSpool = 10;
            var percentUtilization = .95f;
            float length = 0.0f;
            float windAmount = (SpoolDefinition.SpoolWidth / SpoolDefinition.Filament.Diameter) * percentUtilization;
            var maxDiameter = SpoolDefinition.SpoolDiameter - (2 * DepthAverage);
            var curDiameter = SpoolDefinition.MinimumDiameter + SpoolDefinition.Filament.Diameter / 2;
            var loop = 0;
            while (curDiameter < maxDiameter)
            {
                length += windAmount * (float)Math.PI * curDiameter;
                curDiameter += SpoolDefinition.Filament.Diameter * 2 * PercentOffset / 100;
                // adjust for gain from stepping up  in wind
                if (loop > 0)
                    length += StepAmount;
                loop++;
            }
            Console.WriteLine($"Loop count : {loop}");
            return length;
            //throw new NotImplementedException();
        }
        protected float CalcGramsRemaining()
        {
            //var gramsPerMillimeter = Math.Pow((double)SpoolDefinition.Filament.Diameter / 2, 2) * Math.PI * (double)SpoolDefinition.Filament.Density * ConvertFromCCToCMM;
            return (float)Math.Round(SpoolDefinition.Filament.GramsPerMillimeter * FilamentRemainingInMillimeters, 3);
            //throw new NotImplementedException();
        }

        protected override void InitDependents()
        {

            var stdList = new List<string>() { nameof(FilamentRemainingInGrams), nameof(FilamentRemainingInMeters), nameof(FilamentRemainingInMillimeters) };
            var dictionary = new Dictionary<string, List<string>>()
            {
                {nameof(Depth1),stdList },
                {nameof(Depth2),stdList }
            };
            Dependents.Add(GetType().FullName, dictionary);
            DependenciesInitialized = true;
            //throw new NotImplementedException();
        }

        public void EstablishLink(IDocument document)
        {
            //System.Diagnostics.Debug.Assert(!string.IsNullOrEmpty(spoolName));
            Document = document;
            //SpoolDefinition = document.Spools.Where(sp => sp.SpoolName == spoolName).FirstOrDefault();
            System.Diagnostics.Debug.Assert(SpoolDefinition != null);
            //throw new NotImplementedException();
        }
    }
}
