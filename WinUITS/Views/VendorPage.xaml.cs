using Microsoft.UI.Xaml.Controls;

using WinUITS.ViewModels;

namespace WinUITS.Views;

public sealed partial class VendorPage : Page
{
    public VendorViewModel ViewModel
    {
        get;
    }

    public VendorPage()
    {
        ViewModel = App.GetService<VendorViewModel>();
        InitializeComponent();
    }
}
