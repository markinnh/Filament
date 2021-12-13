using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;

namespace Filament_Data.Models
{
    public enum DensityType { Defined=0x8000,Measure}
    public class DensityAlias:Observable,IDensity
    {
        const int MinimumDensityMeasurementsRequired = 3;
        private int densityAliasId;

        public int DensityAliasId
        {
            get => densityAliasId;
            set => Set<int>(ref densityAliasId, value);
        }

        private DensityType densityType;

        public DensityType DensityType
        {
            get => densityType;
            set => Set<DensityType>(ref densityType, value);
        }

        private double definedDensity;

        public double DefinedDensity
        {
            get => definedDensity;
            set => Set<double>(ref definedDensity, value);
        }

        // TODO: make this property not mapped when the entityframework library is added.
        [NotMapped]
        public double Density
        {
            get => densityType==DensityType.Defined?definedDensity:
                MeasuredDensity?.Count(m=>!double.IsNaN(m.Density))>MinimumDensityMeasurementsRequired?MeasuredDensity.Average(m=>m.Density):
                double.NaN;
        }

        public ICollection<MeasuredDensity> MeasuredDensity { get; set; } = new ObservableCollection<MeasuredDensity>();
    }
}
