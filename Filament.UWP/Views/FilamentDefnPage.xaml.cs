using Filament.UWP.ViewModels;
using System;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Filament.UWP.Views
{
    public sealed partial class FilamentDefnPage : Page
    {
        
        public FilamentDefnViewModel ViewModel { get; } = new FilamentDefnViewModel();
        public FilamentDefnPage()
        {
            InitializeComponent();
            Loaded += FilamentDefnPage_Loaded;
        }

        private  void FilamentDefnPage_Loaded(object sender, RoutedEventArgs e)
        {
             ViewModel.LoadData(ListDetailsViewControl.ViewState);
        }

        private void AddFilament_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
