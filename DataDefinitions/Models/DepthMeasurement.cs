
using DataDefinitions.Interfaces;
using LiteDB;
using MyLibraryStandard.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DataDefinitions.Models
{
    /// <summary>
    /// Stores actually depth measurements to determine remaining amount of filament
    /// </summary>
    [UIHints(AddType = "Measurement")]
    public class DepthMeasurement : ParentLinkedDataObject<InventorySpool>//, IEditableObject
    {
        const double FilamentDensityInCC = 1.24;
        const double ConvertFromCMMToCCM = 0.001;
        internal const double FilamentStartingDepth = 12;
        const double stepAdjustment = 4;
        const int digitsOfPrecision = 2;
        //public event EventHandler ValueChanged;
        public static event InDataOpsChangedHandler InDataOpsChanged;

        private static bool inDataOps;
        public static bool InDataOps
        {
            get => inDataOps;
            set
            {
                inDataOps = value;

                // Notify all the InventorySpool objects of change to InDataOps state, allowing them to update the UI.
                InDataOpsChanged?.Invoke(EventArgs.Empty);
            }
        }
        [JsonIgnore, BsonIgnore]
        public override bool InDataOperations => inDataOps;
        [BsonIgnore]
        public override bool IsModified { get => base.IsModified; set => base.IsModified = value; }
        /// <summary>
        /// Whether or not the DepthMeasurement has all required data
        /// </summary>
        [JsonIgnore, BsonIgnore]
        public override bool IsValid => !double.IsNaN(depth1) && !double.IsNaN(depth2) && measureDateTime != default && (Parent != null);

        private double depth1 = FilamentStartingDepth;
        /// <summary>
        /// Depth measurement taken from one side of spool
        /// </summary>
        [Affected(Names = new string[] { nameof(FilamentRemainingInGrams), nameof(FilamentRemainingInMillimeters), nameof(FilamentRemainingInMeters),nameof(IsValid) }),
            XmlAttribute(AttributeName = "depth1")]
        public double Depth1
        {
            get => depth1;
            set
            {
                Set<double>(ref depth1, value);
            }

        }
        private double depth2 = FilamentStartingDepth;
        /// <summary>
        /// Depth measurement taken from the other side of the spool
        /// </summary>
        [Affected(Names = new string[]
        {
        nameof(FilamentRemainingInGrams),
        nameof(FilamentRemainingInMillimeters),
        nameof(FilamentRemainingInMeters),
        nameof(IsValid)
        }),
            XmlAttribute(AttributeName = "depth2")]
        public double Depth2
        {
            get => depth2;
            set
            {

                Set<double>(ref depth2, value);
            }

        }

        private DateTime measureDateTime = DateTime.Today;

        /// <summary>
        /// Date the measurement was taken
        /// </summary>
        [XmlAttribute(AttributeName = "MeasuredOn"),
            JsonPropertyName("Measured"), BsonField("Measured"),Affected(Names =new string[] {nameof(IsValid)})]
        public DateTime MeasureDateTime
        {
            get => measureDateTime;
            set => Set<DateTime>(ref measureDateTime, value);
        }

        /// <summary>
        /// Average of Depth1 and Depth2
        /// </summary>
        [JsonIgnore, BsonIgnore]
        public double AverageDepth => (depth1 + depth2) / 2;

        private double percentOffset = 94;
        /// <summary>
        /// Amount of interlace between layers of filament wound onto a spool
        /// </summary>
        [Affected(Names = new string[] { nameof(FilamentRemainingInGrams), nameof(FilamentRemainingInMillimeters), nameof(FilamentRemainingInMeters) })]
        [JsonIgnore, XmlIgnore, BsonIgnore]
        public double PercentOffset
        {
            get => percentOffset;
            set => Set<double>(ref percentOffset, value);

        }
        private bool adjustForWind;
        /// <summary>
        /// Compesate for wind in the CalcRemaining calculation
        /// </summary>
        /// <remarks>Normally is false</remarks>
        [Affected(Names = new string[] { nameof(FilamentRemainingInGrams), nameof(FilamentRemainingInMeters), nameof(FilamentRemainingInMeters) }),
            JsonIgnore, XmlIgnore, BsonIgnore]
        public bool AdjustForWind
        {
            get { return adjustForWind; }
            set => Set<bool>(ref adjustForWind, value);
        }
        [JsonIgnore, BsonIgnore]
        public double FilamentRemainingInMillimeters => CalcRemaining();
        [JsonIgnore, BsonIgnore]
        public double FilamentRemainingInMeters => CalcRemaining() / 1000;
        [JsonIgnore, BsonIgnore]
        public double FilamentRemainingInGrams => CalcGramsRemaining();

        /// <summary>
        /// Identifier of containing InventorySpool
        /// </summary>
        /// <remarks>Actual part that is stored in database</remarks>
        [XmlAttribute(AttributeName = "InvID"), BsonIgnore]
        public int InventorySpoolId { get; set; }

        private InventorySpool inventorySpool;
        /// <summary>
        /// Reference to containing InventorySpool
        /// </summary>
        [JsonIgnore, XmlIgnore, BsonIgnore]
        public InventorySpool InventorySpool
        {
            get => inventorySpool;
            set => Set<InventorySpool>(ref inventorySpool, value);
        }

        /// <summary>
        /// default constructor for DepthMeasurement
        /// </summary>
        public DepthMeasurement()
        {
        }
        public DepthMeasurement(double depth1, double depth2, DateTime date)
        {
            Depth1 = depth1;
            Depth2 = depth2;
            MeasureDateTime = date;
        }
        /// <summary>
        /// Determines the amount of filament remaining in millimeters
        /// </summary>
        protected double CalcRemaining()
        {
            if (Parent != null && Parent.Parent != null && Parent.FilamentDefn != null)
            {
                //var lostSpool = 10;
                var percentUtilization = .95;
                double length = 0.0;
                double windAmount = (Parent.Parent.SpoolWidth / Parent.FilamentDefn.Diameter) * percentUtilization;
                var maxDiameter = Parent.Parent.SpoolDiameter - (2 * AverageDepth);
                var curDiameter = Parent.Parent.DrumDiameter;
                var loop = 0;
                while (curDiameter < maxDiameter)
                {
                    length += windAmount * (double)Math.PI * curDiameter;
                    if (adjustForWind)
                    {
                        var windStep = loop > 0 ? stepAdjustment : 0;
                        length += windStep;
                    }
                    curDiameter += Parent.FilamentDefn.Diameter * 2 * PercentOffset / 100;
                    loop++;
                }
                Debug.WriteLine($"Loop count : {loop}");
                return length;
            }
            else
                return double.NaN;
            //throw new NotImplementedException();
        }
        /// <summary>
        /// Determines the grams remaining
        /// </summary>
        /// <remarks>Based on the Spool Definition parameters and filament diameter</remarks>
        protected double CalcGramsRemaining()
        {
            if (Parent.FilamentDefn != null && Parent.FilamentDefn.DensityAlias != null)
            {
                var gramsPerMillimeter = Math.Pow((double)Parent.FilamentDefn.Diameter / 2, 2) * Math.PI * Parent.FilamentDefn.DensityAlias.Density * FilamentMath.ConvertFromCubicMillimetersToCubicCentimeters;
                return gramsPerMillimeter * FilamentRemainingInMillimeters;
            }
            return
                double.NaN;
            //throw new NotImplementedException();
        }
        //public void LinkChildren<ParentType>(ParentType parent)
        //{
        //    if (parent is InventorySpool spool && spool.InventorySpoolId == InventorySpoolId)
        //        InventorySpool = spool;
        //}
        //internal override int KeyID
        //{
        //    get => DepthMeasurementId;
        //    set => DepthMeasurementId = value;
        //}
        /*
        #region Support For DataGrid (UWP) Editing
        struct BackupData
        {
            public DateTime MeasureDateTime { get; set; }
            public double Depth1 { get; set; }
            public double Depth2 { get; set; }
            internal BackupData(DepthMeasurement depthMeasurement)
            {
                Depth1 = depthMeasurement.Depth1;
                Depth2 = depthMeasurement.Depth2;
                MeasureDateTime = depthMeasurement.MeasureDateTime;
            }
            internal BackupData(double d1, double d2, DateTime dateTime)
            {
                Depth1 = d1;
                Depth2 = d2;
                MeasureDateTime = dateTime;
            }
        }
        private BackupData backupData;
        /// <summary>
        /// Begin editing in a UWP DataGrid
        /// </summary>
        void IEditableObject.BeginEdit()
        {
            backupData = new BackupData(this);
            InEdit = true;
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Cancel editing in a UWP DataGrid
        /// </summary>
        void IEditableObject.CancelEdit()
        {
            if (InEdit)
            {
                MeasureDateTime = backupData.MeasureDateTime;
                Depth1 = backupData.Depth1;
                Depth2 = backupData.Depth2;
                InEdit = false;
            }
            //throw new NotImplementedException();
        }

        /// <summary>
        /// End editing in a UWP DataGrid
        /// </summary>
        /// <remarks>Does not store object changes to the database.</remarks>
        void IEditableObject.EndEdit()
        {
            if (InEdit)
            {
                backupData = new BackupData(double.NaN, double.NaN, DateTime.MinValue);
                InEdit = false;
            }
            //throw new NotImplementedException();
        }
        #endregion
        //protected void UpdateCalcs()
        //{
        //    OnPropertyChanged(nameof(FilamentRemainingInMeters));
        //    OnPropertyChanged(nameof(FilamentRemainingInMillimeters));
        //    OnPropertyChanged(nameof(FilamentRemainingInGrams));
        //}
        */
    }
}
