using Filament.WPF6.ViewModels;
using Filament_Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Filament.WPF6.Pages
{
    /// <summary>
    /// Interaction logic for ByFilamentPage.xaml
    /// </summary>
    public partial class ByFilamentPage : Page
    {
        public ByFilamentPage()
        {
            InitializeComponent();
        }

        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (DataContext is ByVendorViewModel viewModel && e.NewValue is DatabaseObject databaseObject)
                viewModel.SelectedItem = databaseObject;
        }
    }
}
