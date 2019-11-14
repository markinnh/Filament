using System;

using Filament.UWP.ViewModels;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Filament.UWP.Views
{
    // For more info about the TreeView Control see
    // https://docs.microsoft.com/windows/uwp/design/controls-and-patterns/tree-view
    // For other samples, get the XAML Controls Gallery app http://aka.ms/XamlControlsGallery
    public sealed partial class FilamentPage : Page
    {
        public FilamentViewModel ViewModel { get; } = new FilamentViewModel();

        public FilamentPage()
        {
            InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            await ViewModel.LoadDataAsync();
        }
    }
}
