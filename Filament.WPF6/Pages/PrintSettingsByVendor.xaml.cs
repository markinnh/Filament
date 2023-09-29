using DataDefinitions;
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
    /// Interaction logic for PrintSettingsByVendor.xaml
    /// </summary>
    public partial class PrintSettingsByVendor : Page
    {
        public PrintSettingsByVendor()
        {
            InitializeComponent();
        }

        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if(e.NewValue is DataDefinitions.DataObject dataObject && DataContext is ViewModels.PrintSettingsByVendorViewModel ps)
                ps.SelectedItem=dataObject;
        }
    }
}
