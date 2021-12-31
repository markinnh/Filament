using System;

using Filament.UWP.Core.Models;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using DataDefinitions.Models;
using DataContext;

namespace Filament.UWP.Views
{
    public sealed partial class VendorDetailControl : UserControl
    {
        public VendorDefn MasterMenuItem
        {
            get { return GetValue(MasterMenuItemProperty) as VendorDefn; }
            set { SetValue(MasterMenuItemProperty, value); }
        }

        public static readonly DependencyProperty MasterMenuItemProperty = DependencyProperty.Register("MasterMenuItem", typeof(VendorDefn), typeof(VendorDetailControl), new PropertyMetadata(null, OnMasterMenuItemPropertyChanged));

        public VendorDetailControl()
        {
            InitializeComponent();
        }

        private static void OnMasterMenuItemPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as VendorDetailControl;
            control.ForegroundElement.ChangeView(0, 0, 1);
        }

        private void AddSpoolDefn_Click(object sender, RoutedEventArgs e)
        {
            SpoolDefn spoolDefn = new SpoolDefn() { Vendor=MasterMenuItem,VendorDefnId=MasterMenuItem.VendorDefnId};
            MasterMenuItem.SpoolDefns.Add(spoolDefn);
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (MasterMenuItem.IsModified)
                MasterMenuItem.UpdateItem<FilamentContext>();
        }

        private void SaveSpoolDefn_Click(object sender, RoutedEventArgs e)
        {
            if(sender is SpoolDefn spoolDefn)
                spoolDefn.UpdateItem<FilamentContext>();
        }
    }
}
