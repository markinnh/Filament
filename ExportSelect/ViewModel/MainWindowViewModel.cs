using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ExportSelect.ViewModel;

public partial class MainWindowViewModel:ObservableObject
{
    [ObservableProperty]
    private string _title = "Export Select";
    [ObservableProperty]
    private string _status = "Ready";
    [RelayCommand]
    private void GenerateCode()
    {
        Status = "Generating code...";
        // Code generation logic here
        Thread.Sleep(2000); // Simulate time-consuming task
        Status = "Code generation completed.";
    }
}
