using System;
using Filament.UWP.Core.Helpers;
using Filament.UWP.Services;

using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Filament.UWP.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Filament.UWP
{
    public sealed partial class App : Application
    {
        private Lazy<ActivationService> _activationService;

        private ActivationService ActivationService
        {
            get { return _activationService.Value; }
        }

        public App()
        {
            InitializeComponent();
            if (Singleton<RoamingAppSettings>.Instance.UpdateMigration)
            {
                using(DataContext.FilamentContext context=new DataContext.FilamentContext())
                {
                    System.Diagnostics.Debug.WriteLine("In migrate database routine.");
                    //context.Database.Migrate();
                    DataContext.DataSeed.Seed<DataContext.FilamentContext>();
                    Singleton<RoamingAppSettings>.Instance.UpdateMigration = false;
                }
            }
            // Deferred execution until used. Check https://msdn.microsoft.com/library/dd642331(v=vs.110).aspx for further info on Lazy<T> class.
            _activationService = new Lazy<ActivationService>(CreateActivationService);
            
        }

        protected override async void OnLaunched(LaunchActivatedEventArgs args)
        {
            if (!args.PrelaunchActivated)
            {
                await ActivationService.ActivateAsync(args);
            }
        }

        protected override async void OnActivated(IActivatedEventArgs args)
        {
            await ActivationService.ActivateAsync(args);
        }

        private ActivationService CreateActivationService()
        {
            return new ActivationService(this, typeof(Views.MainPage), new Lazy<UIElement>(CreateShell));
        }

        private UIElement CreateShell()
        {
            return new Views.ShellPage();
        }
    }
}
