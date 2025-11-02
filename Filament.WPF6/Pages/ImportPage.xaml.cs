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

namespace Filament.WPF6.Pages;
/// <summary>
/// Interaction logic for ImportPage.xaml
/// </summary>
public partial class ImportPage : Page
{
    public ImportPage()
    {
        InitializeComponent();
    }

    private void Page_Drop(object sender, DragEventArgs e)
    {
        var files = (string[])e.Data.GetData(DataFormats.FileDrop);
        if(DataContext is ViewModels.ImportViewModel vm)
        {
            vm.ProcessFiles(files);
        }
    }
}
