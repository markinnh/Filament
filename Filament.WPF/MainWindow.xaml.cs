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

namespace Filament.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new ViewModel.MainWindowViewModel();
        }

        private void BrowseInventoryMenuItem_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Source = new Uri(@"Pages\BrowseInventoryPage.xaml",UriKind.Relative);
        }

        private void DefineVendorMenuItem_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Source = new Uri(@"Pages\DefineVendorPage.xaml",UriKind.Relative);
        }

        private void DefineFilamentMenuItem_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Source = new Uri(@"Pages\DefineFilamentPage.xaml",UriKind.Relative);
        }

        private void DefineSpoolMenuItem_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Source = new Uri(@"Pages\DefineSpoolPage.xaml", UriKind.Relative);
        }
    }
}
