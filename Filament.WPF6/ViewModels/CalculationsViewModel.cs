using MyLibraryStandard.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filament.WPF6.ViewModels
{
    public class CalculationsViewModel: DataDefinitions.Observable
    {
        #region Calculate Drum Diameter
        private double spoolDiameter=200;
        [Affected(Names =new string[] {nameof(DrumDiameter)})]
        public double SpoolDiameter
        {
            get => spoolDiameter;
            set => Set<double>(ref spoolDiameter, value);
        }

        private double depth;
        [Affected(Names =new string[] {nameof(DrumDiameter)})]
        public double Depth
        {
            get => depth;
            set => Set<double>(ref depth, value);
        }

        public double DrumDiameter => !double.IsNaN(spoolDiameter) && !double.IsNaN(depth) ? SpoolDiameter - depth * 2 : double.NaN;
        #endregion
    }
}
