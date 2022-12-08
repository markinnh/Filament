using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataDefinitions.Models;
//using DataContext;
using System.Windows.Input;

using System.Diagnostics;
using Filament.WPF6.Helpers;
using DataDefinitions.JsonSupport;
using DataDefinitions.LiteDBSupport;
using CommunityToolkit.Mvvm.Input;

namespace Filament.WPF6.ViewModels
{
    internal class VendorDefnPageViewModel : DataDefinitions.Observable
    {
        public ObservableCollection<VendorDefn>? VendorDefns { get; set; }
        private VendorDefn selectedVendorDefn;

        public VendorDefn SelectedVendorDefn
        {
            get => selectedVendorDefn;
            set => Set<VendorDefn>(ref selectedVendorDefn, value);
        }


        private ICommand? webNavigate;
        public ICommand WebNavigate { get => webNavigate ??= new RelayCommand<object>(HandleWebNavigate); }

        private void HandleWebNavigate(object? obj)
        {
            if (obj is Uri uri)
            {

                //var processStartInfo = new ProcessStartInfo("msedge.exe",uri.AbsoluteUri);
                Process.Start(new ProcessStartInfo("cmd", $"/c start {uri.AbsoluteUri}") { CreateNoWindow = true });
            }
            //throw new NotImplementedException();
        }
       
        public VendorDefnPageViewModel()
        {
            // TODO: Define all the data to seed the database with the vendor definitions
            VendorDefn.InDataOps = true;
            //var defns = DAL.Abstraction.GetAllVendors();
            if (Singleton<LiteDBDal>.Instance.Vendors is ObservableCollection<VendorDefn> defns)
            {
                VendorDefns = defns;
                SelectedVendorDefn= VendorDefns.First();
            }
            else
                VendorDefns = new ObservableCollection<VendorDefn>();
            VendorDefn.InDataOps = false;
        }
    }
}
