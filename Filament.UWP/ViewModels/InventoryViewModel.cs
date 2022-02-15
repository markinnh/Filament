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
using MyLibraryStandard.Attributes;
using WinUI = Microsoft.UI.Xaml.Controls;
using DataDefinitions;

namespace Filament.UWP.ViewModels
{
    public class InventoryViewModel : Observable
    {
        public List<string> SupportedColors { get; set; } = new List<string>(new string[] { "Black", "White", "Red", "Green", "Blue", "Silver", "Grey", "Orange", "Pink", "Purple" });
        public ObservableCollection<FilamentDefn> Filaments { get; set; } = new ObservableCollection<FilamentDefn>();
        private DataDefinitions.DatabaseObject _selectedItem;
        [Affected(Names = new string[] {nameof(AddHintDescription) ,nameof(DatabaseObject)  })]
        public DataDefinitions.DatabaseObject SelectedItem
        {
            get => _selectedItem;
            set => Set(ref _selectedItem, value);
        }
        public string AddHintDescription
        {
            get
            {
                return SelectedItem?.UIHintAddType() ?? "Not selected";
            }
        }
        public DataDefinitions.DatabaseObject DatabaseObject => _selectedItem is DataDefinitions.DatabaseObject ? (DataDefinitions.DatabaseObject)_selectedItem : null;
        //public ObservableCollection<SampleCompany> SampleItems { get; } = new ObservableCollection<SampleCompany>();

        public ObservableCollection<VendorDefn> Vendors { get; protected set; } = new ObservableCollection<VendorDefn>();
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
            System.Diagnostics.Debug.WriteLine($"Made it the delete child command, item passed is a {obj?.GetType().Name ?? "null object"}");
            //throw new NotImplementedException();
            if (obj is InventorySpool inventory && inventory.SupportsDelete && inventory.InDatabase)
                //TODO: Write code to delete the spool from inventory
                inventory.Delete<FilamentContext>();
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
                {
                    var tFila = Filaments.First(fil => fil.MaterialType == DataDefinitions.MaterialType.PLA && fil.IsIntrinsic);
                    spool.Inventory.Add(new InventorySpool() { SpoolDefn = spool, SpoolDefnId = spool.SpoolDefnId, FilamentDefn = tFila, FilamentDefnId = tFila.FilamentDefnId, ColorName = "Black" });
                }
                else if (_selectedItem is InventorySpool inventory)
                {
                    var initialDepth = inventory.CalcInitialDepth();
                    inventory.DepthMeasurements.Add(new DepthMeasurement() { InventorySpool = inventory, InventorySpoolId = inventory.InventorySpoolId, Depth1 = initialDepth, Depth2 = initialDepth });
                }
            }
            //throw new NotImplementedException();
        }

        public InventoryViewModel()
        {
            LoadVendorsAsync();
        }
        public void LoadVendorsAsync()
        {
            var data = Singleton<DataLayer>.Instance.VendorList;
            if (Singleton<DataLayer>.Instance.VendorList is ObservableCollection<VendorDefn> odata)
            {
                System.Diagnostics.Debug.WriteLine($"Adding {data.Count()} items to the database.");
                Vendors = odata;
            }
            else
                foreach (var item in data)
                {
                    if (item is VendorDefn vendor)
                        Vendors.Add(vendor);
                    else
                        System.Diagnostics.Debug.WriteLine($"Item state is{(item == null ? string.Empty : " not")} null");
                }
            //SelectedItem = Vendors.First();
            var tfila = Singleton<DataLayer>.Instance.FilamentList;
            if (tfila is ObservableCollection<FilamentDefn> filaments)
            {
                Filaments = filaments;
            }
            else
                foreach (var item in tfila)
                    Filaments.Add(item);
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
        {
            if (args.InvokedItem is DataDefinitions.DatabaseObject dbobj)
                SelectedItem = dbobj;
        }
    }
}
