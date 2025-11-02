using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ExportSelect.ViewModel;
public partial class BaseViewModel : ObservableObject
{
    [ObservableProperty]
    private bool _inSelectionMode;
}
