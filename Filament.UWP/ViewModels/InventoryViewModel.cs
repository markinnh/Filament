using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using DataContext;

using DataDefinitions.Models;
using Filament.UWP.Core.Helpers;
using Filament.UWP.Core.Models;
using Filament.UWP.Core.Services;
using Filament.UWP.Helpers;

using WinUI = Microsoft.UI.Xaml.Controls;

namespace Filament.UWP.ViewModels
{
    public class InventoryViewModel : Observable
    {
        public List<string> SupportedColors { get; set; } = new List<string>(new string[] { "Black", "White", "Red", "Green", "Blue", "Silver", "Grey", "Orange" });
        public ObservableCollection<FilamentDefn> Filaments { get; set; }=new ObservableCollection<FilamentDefn>();
        private DataDefinitions.DatabaseObject _selectedItem;

        public DataDefinitions.DatabaseObject SelectedItem
        {
            get => _selectedItem;
            set => Set(ref _selectedItem, value);
        }

        //public ObservableCollection<SampleCompany> SampleItems { get; } = new ObservableCollection<SampleCompany>();

        public ObservableCollection<VendorDefn> Vendors { get; } = new ObservableCollection<VendorDefn>();
        private ICommand _itemInvokedCommand;
        public ICommand ItemInvokedCommand => _itemInvokedCommand ?? (_itemInvokedCommand = new RelayCommand<WinUI.TreeViewItemInvokedEventArgs>(OnItemInvoked));

        private ICommand _addChildCommand;
        public ICommand AddChildCommand => _addChildCommand ?? (_addChildCommand = new RelayCommand(HandleAddChildCommand));

        private ICommand _saveSelectedCommand;
        public ICommand SaveSelectedCommand => _saveSelectedCommand ?? (_saveSelectedCommand = new RelayCommand(HandleSaveSelectedCommand));
        private ICommand _deleteChildCommand;
        public ICommand DeleteChildCommand => _deleteChildCommand ?? (_deleteChildCommand = new RelayCommand<object>(HandleDeleteChildCommand));

        internal void HandleDeleteChildCommand(object obj)
        {
            System.Diagnostics.Debug.WriteLine($"Made it the delete child command, item passed is a {obj?.GetType().Name??"null object"}");
            //throw new NotImplementedException();
        }

        private void HandleSaveSelectedCommand()
        {
            if (_selectedItem is DataDefinitions.DatabaseObject databaseObject)
            {
                if (databaseObject.IsModified)
                    databaseObject.UpdateItem<FilamentContext>();
            }
            //throw new NotImplementedException();
        }

        private void HandleAddChildCommand()
        {
            if (_selectedItem != null)
            {
                if (_selectedItem is VendorDefn vendor)
                    vendor.SpoolDefns.Add(new SpoolDefn() { Vendor = vendor, VendorDefnId = vendor.VendorDefnId });
                else if (_selectedItem is SpoolDefn spool)
                    spool.Inventory.Add(new InventorySpool() { SpoolDefn = spool, SpoolDefnId = spool.SpoolDefnId });
                else if (_selectedItem is InventorySpool inventory)
                    inventory.DepthMeasurements.Add(new DepthMeasurement() { InventorySpool = inventory, InventorySpoolId = inventory.InventorySpoolId });
            }
            //throw new NotImplementedException();
        }

        public InventoryViewModel()
        {
        }
        public void LoadVendorsAsync()
        {
            //var data = Singleton<DataLayer>.Instance.VendorList;
            if (Singleton<DataLayer>.Instance.VendorList is IEnumerable<VendorDefn> data)
            {
                System.Diagnostics.Debug.WriteLine($"Adding {data.Count()} items to the database.");
                foreach (var item in data)
                {
                    Vendors.Add(item);
                }
                SelectedItem = Vendors.First();
            }
            if(Singleton<DataLayer>.Instance.FilamentList is IEnumerable<FilamentDefn> filaments)
            {
                foreach (var item in filaments)
                    Filaments.Add(item);
            }
        }
        //public async Task LoadDataAsync()
        //{
        //    var data = await SampleDataService.GetTreeViewDataAsync();
        //    foreach (var item in data)
        //    {
        //        SampleItems.Add(item);
        //    }

        //}

        private void OnItemInvoked(WinUI.TreeViewItemInvokedEventArgs args)
            => SelectedItem = (DataDefinitions.DatabaseObject)args.InvokedItem;
    }
}
