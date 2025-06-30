using DataDefinitions;
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

namespace Filament.WPF6;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    
    public App()
    {
        Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NMaF5cXmBCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdnWH1ceHVUQmlZUEdwWEY=");
        // TODO: implement a trace log to be stored in the MyDocuments directory in the Default DataPath
        // TODO: move trace log functionality to a separate class
        //Trace.Listeners.Add(new TextWriterTraceListener(Console.Out));
        var dataDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), Settings.Default.DataPath);
        if (!Directory.Exists(dataDir))
        {
            try
            {
                Directory.CreateDirectory(dataDir);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " may not be able to continue.");
                //throw;
            }

        }
        //var traceLog = ;
        Singleton<TraceToFileSupport>.Instance.Initialize(Path.Combine(dataDir, "TextLog.txt"));
        
        if (Settings.Default.EnableDebugToFile)
            Singleton<TraceToFileSupport>.Instance.StartListening();

        Trace.WriteLine($"{DateTime.Now} Application Start", "Information");
        //var dataAccessSelector = Enum.Parse<DAL.DataAccessSelector>(WPF6.Properties.Settings.Default.DataAccessSelector);
        //DAL.Abstraction.DataAccessSelector = dataAccessSelector;
        //bool NeedMigration = DAL.Abstraction.DataAccessSelector == DAL.DataAccessSelector.SqlServer ? WPF6.Properties.Settings.Default.SqlServerNeedsMigration : true;
        //DAL.Abstraction.SeedData() ;
        //DAL.Abstraction.VerifySeed();

        //Singleton<DataDefinitions.JsonSupport.JsonDAL>.Instance.InitializeFromFile(Path.Combine(dataDir, "TestFilamentData.JSON"));
        //Trace.WriteLine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),"Information");
        Singleton<LiteDBDal>.Instance.Initialize(FileLocator.DatabasePath(UIPlatform.WPF, Settings.Default.DataPath));
    }

    private void Application_Exit(object sender, ExitEventArgs e)
    {
        Settings.Default.Save();
        if (Settings.Default.EnableDebugToFile) { 
            Singleton<LiteDBDal>.Instance.ReleaseEventHandlers();
            Trace.WriteLine($"{DateTime.Now} Application Exit", "Information");
        }
        Singleton<TraceToFileSupport>.Instance.FinishListening();
        
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
