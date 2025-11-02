using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filament.WPF6.Helpers;
/// <summary>
/// Not all viewmodels support exporting data, this interface is a marker interface to indicate the viewmodel supports exporting
/// </summary>
internal interface IExportUI
{
    ImportExportUIState ImportExportUIState { get;
    }
}
