using System.Configuration;
using System.Data;
using System.Windows;
using DataDefinitions;
using DataDefinitions.LiteDBSupport;

namespace ExportSelect;
/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public App()
    {
        // Initialize the application
        // You can add any startup logic here if needed
        // For example, you might want to set up logging or load settings
        // Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("YourLicenseKeyHere");
        Singleton<LiteDBDal>.Instance.Initialize(FileLocator.DatabasePath(UIPlatform.WPF, Settings1.Default.DataPath));

    }
}

