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

        public ObservableCollection<VendorDefn> Vendors { get; } = new ObservableCollection<VendorDefn>(Core.Helpers.Singleton<DataContext.DataLayer>.Instance.VendorList);
        private ICommand addVendor;
        public ICommand AddVendor
        {
            get
            {
                if (addVendor == null)
                    addVendor = new RelayCommand(HandleAddVendor, HandleCanAdd);
                return addVendor;
            }
            set { addVendor = value; }
        }

        private bool HandleCanAdd()
        {
            return true;
            //throw new NotImplementedException();
        }

        private void HandleAddVendor()
        {
            Selected = new VendorDefn();
            //Vendors.Add(Selected, true);
        }

        public VendorViewModel()
        {
            AddVendor = new RelayCommand(HandleAddVendor);
        }

        public void LoadDataAsync()
        {
            Selected = Vendors.First();
        }
    }
}
