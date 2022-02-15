using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

using Filament.UWP.Core.Models;
using Filament.UWP.Core.Services;
using Filament.UWP.Helpers;

using Microsoft.Toolkit.Uwp.UI.Controls;
using DataDefinitions.Models;
using System.Windows.Input;
using DataDefinitions;

namespace Filament.UWP.ViewModels
{
    public class VendorViewModel : Observable
    {
        // TODO: this is temporary
        //private static readonly DataDocument data = Core.Helpers.Singleton<DataFile>.Instance.Document;
        private VendorDefn _selected;

        public VendorDefn Selected
        {
            get { return _selected; }
            set { Set(ref _selected, value); }
        }

        private ObservableCollection<VendorDefn> vendors;

        public ObservableCollection<VendorDefn> Vendors
        {
            get => vendors;
            set => Set<ObservableCollection<VendorDefn>>(ref vendors, value);
        }

        //public ObservableCollection<VendorDefn> Vendors { get; protected set; } = new ObservableCollection<VendorDefn>();
        private ICommand addVendor;
        public ICommand AddVendor
        {
            get => addVendor ?? (addVendor = new RelayCommand(HandleAddVendor, HandleCanAdd));
            
        }

        private bool HandleCanAdd()
        {
            return true;
            //throw new NotImplementedException();
        }

        private void HandleAddVendor()
        {
            var newVendor = new VendorDefn();
            Vendors.Add(newVendor);
            Selected = newVendor;
            //Vendors.Add(Selected, true);
        }

        public VendorViewModel()
        {
            //AddVendor = new RelayCommand(HandleAddVendor);
        }

        public void LoadDataAsync()
        {
            var data = Core.Helpers.Singleton<DataContext.DataLayer>.Instance.VendorList;
            if (data is ObservableCollection<VendorDefn> odata)
                Vendors = odata;
            else
                foreach(var v in data)
                    Vendors.Add(v);

            Selected = Vendors.First();
        }
    }
}
