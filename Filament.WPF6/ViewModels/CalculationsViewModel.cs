using CommunityToolkit.Mvvm.ComponentModel;
using MyLibraryStandard.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filament.WPF6.ViewModels;

public partial class CalculationsViewModel: ObservableRecipient
{
    #region Calculate Drum Diameter
    [ObservableProperty,NotifyPropertyChangedFor(nameof(DrumDiameter))]
    private double spoolDiameter=200;
    [ObservableProperty,NotifyPropertyChangedFor(nameof(DrumDiameter))]
    private double depth;

    public double DrumDiameter => !double.IsNaN(SpoolDiameter) && !double.IsNaN(Depth) ? SpoolDiameter - Depth * 2 : double.NaN;
    #endregion
}
