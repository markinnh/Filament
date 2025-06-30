using DataDefinitions;
using Microsoft.UI.Xaml;

namespace FilamentWinUI3
{
    public partial class App : Application
    {
        private Window? _window;

        public App()
        {
            this.InitializeComponent();
        }

        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NAaF5cWWJCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdgWH1cdnVQQ2dYWUFwW0Y=");
            Singleton<DataDefinitions.LiteDBSupport.LiteDBDal>.Instance.Initialize("Test.3dpdb");
            _window = new MainWindow();
            _window.Activate();
        }
    }
}
