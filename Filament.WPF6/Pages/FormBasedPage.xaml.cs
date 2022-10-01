using DataDefinitions;
using Filament.WPF6.ViewModels;
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
    /// Interaction logic for FormBasedPage.xaml
    /// </summary>
    public partial class FormBasedPage : Page
    {
        public FormBasedPage()
        {
            InitializeComponent();
        }

        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (e.NewValue is DatabaseObject databaseObject && DataContext is FormBasedViewModel model)
                model.SelectedItem = databaseObject;

        }
    }
}
