using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
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
            
            DataContext.DataSeed.Seed<DataContext.FilamentContext>();
            DataContext.DataSeed.VerifySeed();
        }
    }
}
