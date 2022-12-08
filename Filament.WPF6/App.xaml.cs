using DataDefinitions.JsonSupport;
using DataDefinitions.LiteDBSupport;
using Filament.WPF6.Helpers;
using Filament.WPF6.Properties;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
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
            var dataDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), Settings.Default.DataPath);

            if (!Directory.Exists(dataDir))
            {
                try
                {
                    Directory.CreateDirectory(dataDir);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message+ " may not be able to continue.");
                    //throw;
                }

            }
            //var dataAccessSelector = Enum.Parse<DAL.DataAccessSelector>(WPF6.Properties.Settings.Default.DataAccessSelector);
            //DAL.Abstraction.DataAccessSelector = dataAccessSelector;
            //bool NeedMigration = DAL.Abstraction.DataAccessSelector == DAL.DataAccessSelector.SqlServer ? WPF6.Properties.Settings.Default.SqlServerNeedsMigration : true;
            //DAL.Abstraction.SeedData() ;
            //DAL.Abstraction.VerifySeed();

            //Singleton<DataDefinitions.JsonSupport.JsonDAL>.Instance.InitializeFromFile(Path.Combine(dataDir, "TestFilamentData.JSON"));
            Singleton<LiteDBDal>.Instance.Initialize(Path.Combine( dataDir,"TestingFilamentDataLDB.db"));
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            Filament.WPF6.Properties.Settings.Default.Save();
            try
            {
                //Singleton<JsonDAL>.Instance.SaveDocument();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }
        }
    }
}
