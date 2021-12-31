using System;

using Filament.UWP.ViewModels;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Filament.UWP.Views
{
    public sealed partial class VendorPage : Page
    {
        public VendorViewModel ViewModel { get; } = new VendorViewModel();

        public VendorPage()
        {
            InitializeComponent();
            Loaded += VendorPage_Loaded;
        }

        private void VendorPage_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel.LoadDataAsync();
        }
    }
}
