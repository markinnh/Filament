using System;
using System.Collections.Generic;
using System.Text;

namespace Filament_Data
{
    public class FilamentCalc:NotifyPropertyChanged
    {
        const double FilamentDensityInCC = 1.24;
        const double ConvertFromCCToCMM = 0.001;
        const decimal FilamentStartingDepth = 12;


        private decimal depth1 = FilamentStartingDepth;
        public decimal Depth1
        {
            get => depth1;
            set
            {
                if (depth1 != value)
                {
                    depth1 = value;
                    OnPropertyChanged(nameof(Depth1));
                    UpdateCalcs();
                }
            }
        }
        private decimal depth2 = FilamentStartingDepth;
        public decimal Depth2
        {
            get => depth2;
            set
            {
                if (depth2 != value)
                {
                    depth2 = value;
                    OnPropertyChanged(nameof(Depth2));
                    UpdateCalcs();
                }
            }
        }
        private decimal percentOffset = 94;
        public decimal PercentOffset
        {
            get => percentOffset; set
            {
                if (percentOffset != value)
                {
                    percentOffset = value;
                    OnPropertyChanged(nameof(PercentOffset));
                    UpdateCalcs();
                }
            }
        }
        public decimal FilamentRemainingInMillimeters => CalcRemaining();
        public decimal FilamentRemainingInMeters => (decimal)Math.Round(CalcRemaining() / 1000m, 3);
        public decimal FilamentRemainingInGrams => CalcGramsRemaining();
        public decimal DepthAverage { get => (Depth1 + Depth2) / 2; }
        public SpoolDefinition SpoolDefinition { get; set; }
        protected decimal CalcRemaining()
        {
            //var lostSpool = 10;
            var percentUtilization = .95m;
            decimal length = 0.0m;
            decimal windAmount = (SpoolDefinition.SpoolWidth / SpoolDefinition.Filament.Diameter) * percentUtilization;
            var maxDiameter = SpoolDefinition.SpoolDiameter - (2 * DepthAverage);
            var curDiameter = SpoolDefinition.MinimumDiameter;
            var loop = 0;
            while (curDiameter < maxDiameter)
            {
                length += windAmount * (decimal)Math.PI * curDiameter;
                curDiameter += SpoolDefinition.Filament.Diameter * 2 * PercentOffset / 100;
                loop++;
            }
            Console.WriteLine($"Loop count : {loop}");
            return length;
            //throw new NotImplementedException();
        }
        protected decimal CalcGramsRemaining()
        {
            var gramsPerMillimeter = Math.Pow((double)SpoolDefinition.Filament.Diameter / 2, 2) * Math.PI * (double)SpoolDefinition.Filament.Density * ConvertFromCCToCMM;
            return (decimal)Math.Round((decimal)gramsPerMillimeter * FilamentRemainingInMillimeters, 3);
            //throw new NotImplementedException();
        }
        protected void UpdateCalcs()
        {
            OnPropertyChanged(nameof(FilamentRemainingInMeters));
            OnPropertyChanged(nameof(FilamentRemainingInMillimeters));
            OnPropertyChanged(nameof(FilamentRemainingInGrams));
        }

    }
}
