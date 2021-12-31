using System;
using System.Collections.Generic;
using Filament.UWP.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Filament.UWP.Views
{
    // For more info about the TreeView Control see
    // https://docs.microsoft.com/windows/uwp/design/controls-and-patterns/tree-view
    // For other samples, get the XAML Controls Gallery app http://aka.ms/XamlControlsGallery
    public sealed partial class InventoryPage : Page
    {


        public InventoryViewModel ViewModel
        {
            get { return (InventoryViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ViewModel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(InventoryViewModel), typeof(InventoryPage), new PropertyMetadata(null));


        //public InventoryViewModel ViewModel { get; } = new InventoryViewModel();

        public InventoryPage()
        {
            InitializeComponent();
            ViewModel = new InventoryViewModel();
            DataContext = ViewModel;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
             ViewModel.LoadVendorsAsync();
        }

        private void DeleteChild_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Made it to delete child event.");
            if(sender is Button button)
                ViewModel.HandleDeleteChildCommand(button.Tag);
        }
    }
}
