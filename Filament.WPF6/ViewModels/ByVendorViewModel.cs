using Filament_Db;
using Filament_Db.DataContext;
using Filament_Db.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Toolkit.Mvvm.Messaging;
using System.Windows.Input;

namespace Filament.WPF6.ViewModels
{
    internal class ByVendorViewModel : BaseBrowserViewModel<Filament_Db.Models.VendorDefn, DatabaseObject>
    {
        //private object? selectedObject;

        //public object? SelectedObject
        //{
        //    get => selectedObject;
        //    set => Set<object?>(ref selectedObject, value);
        //}
        

        protected override void DerivedInitItems()
        {
            PrepareForDataOperations();
            if (GetAllItems() is IEnumerable<VendorDefn> items)
                Items = new System.Collections.ObjectModel.ObservableCollection<Filament_Db.Models.VendorDefn>(items);
            
            FinishedDataOperations();

        }

        protected override void ShowAllItems()
        {
            base.ShowAllItems();
        }

        protected override void ShowInUseItems()
        {
            base.ShowInUseItems();
        }

        protected override IEnumerable<VendorDefn>? GetInUseItems() => Singleton<DataLayer>.Instance.GetFilteredVendors(v => !v.StopUsing);

        protected override IEnumerable<VendorDefn>? GetAllItems() => Singleton<DataLayer>.Instance.VendorList;
        protected override IEnumerable<VendorDefn>? GetFilteredItems(Func<VendorDefn, bool> predicate) => Singleton<DataLayer>.Instance.GetFilteredVendors(predicate);



        protected override void FinishedDataOperations()
        {
            Filament_Db.Models.VendorDefn.InDataOps = false;
            SpoolDefn.InDataOps = false;
            InventorySpool.InDataOps = false;
            DepthMeasurement.InDataOps = false;
        }

        protected override void PrepareForDataOperations()
        {
            Filament_Db.Models.VendorDefn.InDataOps = true;
            SpoolDefn.InDataOps = true;
            InventorySpool.InDataOps = true;
            DepthMeasurement.InDataOps = true;
        }

        protected override void UpdateSelectedItemHander()
        {
            //base.UpdateSelectedItemHander();
            if (SelectedItem != null)
                if (SelectedItem.IsValid)
                {
                    SelectedItem.UpdateItem();

                    SelectedItem.SetContainedModifiedState(false);
                }
        }
    }
}
