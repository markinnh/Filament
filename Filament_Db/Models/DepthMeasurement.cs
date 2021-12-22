using Filament_Db.DataContext;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filament_Db.Models
{
    public class DepthMeasurement : DatabaseObject
    {
        const double FilamentDensityInCC = 1.24;
        const double ConvertFromCMMToCCM = 0.001;
        const double FilamentStartingDepth = 12;
        const double stepAdjustment = 4;
        const int digitsOfPrecision = 2;
        //public event EventHandler ValueChanged;
        public static event InDataOpsChangedHandler? InDataOpsChanged;

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
        public override bool InDataOperations => inDataOps;

        public override bool InDatabase => DepthMeasurementId != default;

        public int DepthMeasurementId { get; set; }
        private double depth1 = FilamentStartingDepth;
        public double Depth1
        {
            get => depth1;
            set
            {
                if (Set<double>(ref depth1, value))
                {
                    //depth1 = value;

                    //OnPropertyChanged(nameof(Depth1));
                    UpdateCalcs();
                }
            }
        }
        private double depth2 = FilamentStartingDepth;
        public double Depth2
        {
            get => depth2;
            set
            {
                if (Set<double>(ref depth2, value))
                {
                    UpdateCalcs();
                }
            }
        }

        private DateTime measureDateTime=DateTime.Today;

        public DateTime MeasureDateTime
        {
            get => measureDateTime;
            set => Set<DateTime>(ref measureDateTime, value);
        }

        [NotMapped]
        public double AverageDepth => (depth1 + depth2) / 2;

        private double percentOffset = 94;
        public double PercentOffset
        {
            get => percentOffset; set
            {
                if (Set<double>(ref percentOffset, value))
                {
                    UpdateCalcs();
                }
            }
        }
        private bool adjustForWind;

        public bool AdjustForWind
        {
            get { return adjustForWind; }
            set
            {
                if (Set<bool>(ref adjustForWind, value))
                {
                    UpdateCalcs();
                }
            }
        }
        
        public double FilamentRemainingInMillimeters => CalcRemaining();
        public double FilamentRemainingInMeters => (double)Math.Round(CalcRemaining() / 1000f, digitsOfPrecision);
        public double FilamentRemainingInGrams => CalcGramsRemaining();

        public int InventorySpoolId { get; set; }

        private InventorySpool inventorySpool;

        public InventorySpool InventorySpool
        {
            get => inventorySpool;
            set => Set<InventorySpool>(ref inventorySpool, value);
        }

        public DepthMeasurement()
        {
        }
        protected double CalcRemaining()
        {
            if (InventorySpool != null && InventorySpool.SpoolDefn != null && InventorySpool.FilamentDefn != null)
            {
                //var lostSpool = 10;
                var percentUtilization = .95f;
                double length = 0.0f;
                double windAmount = (InventorySpool.SpoolDefn.SpoolWidth / InventorySpool.FilamentDefn.Diameter) * percentUtilization;
                var maxDiameter = InventorySpool.SpoolDefn.SpoolDiameter - (2 * AverageDepth);
                var curDiameter = InventorySpool.SpoolDefn.DrumDiameter;
                var loop = 0;
                while (curDiameter < maxDiameter)
                {
                    length += (windAmount * (double)Math.PI * curDiameter);
                    if (adjustForWind)
                    {
                        var windStep = loop > 0 ? stepAdjustment : 0;
                        length += windStep;
                    }
                    curDiameter += InventorySpool.FilamentDefn.Diameter * 2 * PercentOffset / 100;
                    loop++;
                }
                Console.WriteLine($"Loop count : {loop}");
                return length;
            }
            else
                return double.NaN;
            //throw new NotImplementedException();
        }
        protected double CalcGramsRemaining()
        {
            if (InventorySpool.FilamentDefn != null && InventorySpool.FilamentDefn.DensityAlias != null)
            {
                var gramsPerMillimeter = Math.Pow((double)InventorySpool.FilamentDefn.Diameter / 2, 2) * Math.PI * InventorySpool.FilamentDefn.DensityAlias.Density * FilamentMath.ConvertFromCubicMillimetersToCubicCentimeters;
                return gramsPerMillimeter * FilamentRemainingInMillimeters;
            }
            return
                double.NaN;
            //throw new NotImplementedException();
        }
        protected void UpdateCalcs()
        {
            OnPropertyChanged(nameof(FilamentRemainingInMeters));
            OnPropertyChanged(nameof(FilamentRemainingInMillimeters));
            OnPropertyChanged(nameof(FilamentRemainingInGrams));
        }

        public override void UpdateItem()
        {
            
        }
    }
}
