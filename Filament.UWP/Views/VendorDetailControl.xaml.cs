using System;

using Filament.UWP.Core.Models;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Filament_Data;

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
    }
}
