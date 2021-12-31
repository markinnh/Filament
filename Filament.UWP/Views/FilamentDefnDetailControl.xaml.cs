using System;

//using Filament.UWP.Core.Models;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using DataDefinitions.Models;
using Filament.UWP.Helpers;
using System.Collections.Generic;
using Filament.UWP.Core.Helpers;
using DataContext;

namespace Filament.UWP.Views
{
    public sealed partial class FilamentDefnDetailControl : UserControl
    {
        private bool inAddNew = false;
        public EnumListProvider<DataDefinitions.MaterialType> MaterialTypes { get; set; } = new EnumListProvider<DataDefinitions.MaterialType>();
        public EnumListProvider<DensityType> DensityTypes { get; set; } = new EnumListProvider<DensityType>();
        public List<double> SupportedFilamentDiameters { get; set; } = new List<double>(new double[] { 1.75, 3.0 });
#if DEBUG
        public bool SupportsPrepopulate => (ListMenuItem?.MaterialType == DataDefinitions.MaterialType.PLA) ;
#else
        public bool SupportsPrepopulate =>false;
#endif
        public FilamentDefn ListMenuItem
        {
            get { return GetValue(ListMenuItemProperty) as FilamentDefn; }
            set { SetValue(ListMenuItemProperty, value); }
        }

        public static readonly DependencyProperty ListMenuItemProperty = DependencyProperty.Register("ListMenuItem", typeof(FilamentDefn), typeof(FilamentDefnDetailControl), new PropertyMetadata(null, OnListMenuItemPropertyChanged));

        public FilamentDefnDetailControl()
        {
            InitializeComponent();
        }

        private static void OnListMenuItemPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as FilamentDefnDetailControl;
            control.ForegroundElement.ChangeView(0, 0, 1);
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            var needPostUpdateProcessing = !ListMenuItem.InDatabase;
            if (ListMenuItem.IsModified)
                ListMenuItem.UpdateItem<DataContext.FilamentContext>();

            if (needPostUpdateProcessing)
            {
                inAddNew = false;
                Singleton<DataLayer>.Instance.Add(ListMenuItem);
            }
        }

        private void AddRow_Click(object sender, RoutedEventArgs e)
        {
            ListMenuItem.DensityAlias.MeasuredDensity.Add(new MeasuredDensity());
        }

        private void Prepopulate_Click(object sender, RoutedEventArgs e)
        {

            ListMenuItem.DensityAlias.Prepopulate();
        }

        private void DeleteRow_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is MeasuredDensity md)
            {
                ListMenuItem.DensityAlias.MeasuredDensity.Remove(md);
            }
        }

        private void AddFilament_Click(object sender, RoutedEventArgs e)
        {
            if (!inAddNew)
            {
                ListMenuItem = new FilamentDefn();
                inAddNew = true;
            }
        }
    }
}
