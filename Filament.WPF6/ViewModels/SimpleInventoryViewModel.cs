using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using DataDefinitions.Models;
using Filament.WPF6.Helpers;
using Microsoft.Toolkit.Mvvm.Input;

namespace Filament.WPF6.ViewModels
{
    internal class SimpleInventoryViewModel : ByVendorViewModel
    {
        
        public IEnumerable<InventorySpool>? Inventory
        {
            get => GetInventory();
        }


        private IEnumerable<InventorySpool> GetInventory()
        {
            if (Items != null)
                foreach (var item in Items)
                    foreach (var defn in item.SpoolDefns)
                        foreach (var spool in defn.Inventory)
                            yield return spool;
        }

        private ICommand? refreshListCommand;
        public ICommand RefreshListCommand => refreshListCommand ??= new RelayCommand(() => { OnPropertyChanged(nameof(Inventory)); });

        protected override void UpdateAfterItemDelete()
        {
            OnPropertyChanged(nameof(Inventory));
        }
        

        #region New Inventory Support
        public IEnumerable<SpoolDefn> SpoolDefns => GetSpoolDefns();
        private IEnumerable<SpoolDefn> GetSpoolDefns()
        {
            foreach(var vendor in Singleton<DAL.DataLayer>.Instance.VendorList)
                foreach(var sd in vendor.SpoolDefns)
                    yield return sd;
        }
        public IEnumerable<FilamentDefn> FilamentDefns => Singleton<DAL.DataLayer>.Instance.FilamentList;

        private InventorySpool? newInventorySpool;

        public InventorySpool? NewInventorySpool
        {
            get => newInventorySpool;
            set => Set<InventorySpool?>(ref newInventorySpool, value);
        }
        private bool inAddInventory;

        public bool InAddInventory
        {
            get => inAddInventory;
            set => Set<bool>(ref inAddInventory, value);
        }
        private ICommand? addNewInventoryCommand;
        public ICommand AddNewInventoryCommand => addNewInventoryCommand ??= new RelayCommand(() =>
        {
            InAddInventory = true;
            if ((newInventorySpool != null && newInventorySpool.IsValid && newInventorySpool.InDatabase)||newInventorySpool==null)
                NewInventorySpool = new InventorySpool();
        });
        private ICommand? saveNewInventoryCommand;
        public ICommand SaveNewInventoryCommand => saveNewInventoryCommand ??= new RelayCommand(() =>
        {
            if (NewInventorySpool?.IsValid ?? false)
            {
                bool needsAdd = !NewInventorySpool.InDatabase;
                DAL.Abstraction.UpdateItem(NewInventorySpool);
                if (needsAdd)
                {
                    //Singleton<DAL.DataLayer>.Instance.Add(NewInventorySpool);
                    NewInventorySpool.SpoolDefn.Inventory.Add(NewInventorySpool);
                    OnPropertyChanged(nameof(Inventory));
                }
                InAddInventory = false;
            }
            else
                System.Windows.MessageBox.Show($"Unable to add new inventory to the database. The NewInventorySpool is {((newInventorySpool?.IsValid??false)?string.Empty:"not ")}valid.");
        });

        private ICommand? undoNewInventoryCommand;
        public ICommand UndoNewInventoryCommand => undoNewInventoryCommand ??= new RelayCommand(() =>
        {
            NewInventorySpool = null;
            InAddInventory = false;
        });
        #endregion
    }
}
