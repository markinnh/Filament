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

namespace Filament.WPF.Pages
{
    /// <summary>
    /// Interaction logic for DefineFilamentPage.xaml
    /// </summary>
    public partial class DefineFilamentPage : Page
    {
        public DefineFilamentPage()
        {
            InitializeComponent();
        }

        private void DataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if(DataContext is ViewModel.DefineFilamentViewModel vm)
            {
                //vm.UpdateForCellEdit();
            }
        }

        private void DataGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            if(e.EditAction == DataGridEditAction.Commit && DataContext is ViewModel.DefineFilamentViewModel vm)
            {
                vm.UpdateForCellEdit();
            }
        }
    }
}
