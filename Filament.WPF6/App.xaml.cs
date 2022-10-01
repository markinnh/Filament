using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Filament.WPF6
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            Trace.Listeners.Add(new TextWriterTraceListener(Console.Out));
            
            //var dataAccessSelector = Enum.Parse<DAL.DataAccessSelector>(WPF6.Properties.Settings.Default.DataAccessSelector);
            //DAL.Abstraction.DataAccessSelector = dataAccessSelector;
            bool NeedMigration = DAL.Abstraction.DataAccessSelector == DAL.DataAccessSelector.SqlServer ? WPF6.Properties.Settings.Default.SqlServerNeedsMigration : true;
            DAL.Abstraction.SeedData() ;
            DAL.Abstraction.VerifySeed();
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            Filament.WPF6.Properties.Settings.Default.Save();
        }
    }
}
