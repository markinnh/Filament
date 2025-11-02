using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Filament.WPF6.Helpers;
/// <summary>
/// Maintains state logic for the Export UI elements
/// </summary>
public partial class ImportExportUIState:ObservableObject
{
    [ObservableProperty]
    private bool _supportsSingleExport;
    [ObservableProperty]
    private bool _supportsMultiExport;
    [ObservableProperty]
    private bool _supportsImport;
    [ObservableProperty]
    private bool _canExportSingle;
    [ObservableProperty]
    private bool _canExportMulti;
    internal ImportExportUIState(bool supportsSingleExport, bool supportsMultiExport, bool supportsImport=false)
    {
        _supportsSingleExport = supportsSingleExport;
        _supportsMultiExport = supportsMultiExport;
        _supportsImport = supportsImport;
    }
    internal void UpdateCanExportState(bool canExportSingle, bool canExportMulti)
    {
        CanExportSingle = canExportSingle;
        CanExportMulti =  canExportMulti;
    }
}
public class ExchangeActionEventArgs : EventArgs
{
    public ExchangeActions Action { get; set;
    }
    public ExchangeActionEventArgs(ExchangeActions action)
    {
        Action = action;
    }
}
