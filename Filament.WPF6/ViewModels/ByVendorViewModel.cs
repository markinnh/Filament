using DataDefinitions;
using DataDefinitions.Models;
using DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Toolkit.Mvvm.Messaging;
using System.Windows.Input;
using Filament.WPF6.Helpers;

namespace Filament.WPF6.ViewModels
{
    internal class ByVendorViewModel : BaseBrowserViewModel<DataDefinitions.Models.VendorDefn, DatabaseObject>
    {
        //private object? selectedObject;

        //public object? SelectedObject
        //{
        //    get => selectedObject;
        //    set => Set<object?>(ref selectedObject, value);
        //}

        public IEnumerable<FilamentDefn> Filaments { get => Singleton<DataLayer>.Instance.FilamentList; }
        public IEnumerable<string> SupportedColorsCode { get => Properties.Settings.Default.SupportedColors.Split(','); }
        protected override void DerivedInitItems()
        {
            //PrepareForDataOperations();
            if (GetAllItems() is IEnumerable<VendorDefn> items)
            {
                if (Items == null)
                    Items = new System.Collections.ObjectModel.ObservableCollection<VendorDefn>(items);
                else
                {
                    Items.Clear();

                    foreach (VendorDefn item in items)
                        Items.Add(item);
                }
            }

            //FinishedDataOperations();

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
            //throw new NotImplementedException();
        }
        protected override void PrepareForDataOperations()
        {
            //throw new NotImplementedException();
        }
        //protected override void FinishedDataOperations()
        //{
        //    Filament_Db.Models.VendorDefn.InDataOps = false;
        //    SpoolDefn.InDataOps = false;
        //    InventorySpool.InDataOps = false;
        //    DepthMeasurement.InDataOps = false;
        //}

        //protected override void PrepareForDataOperations()
        //{
        //    Filament_Db.Models.VendorDefn.InDataOps = true;
        //    SpoolDefn.InDataOps = true;
        //    InventorySpool.InDataOps = true;
        //    DepthMeasurement.InDataOps = true;
        //}

        protected override void UpdateSelectedItemHander()
        {
            //base.UpdateSelectedItemHander();
            if (SelectedItem != null)
                if (SelectedItem.IsValid)
                {
                    SelectedItem.UpdateItem<FilamentContext>();

                    SelectedItem.SetContainedModifiedState(false);
                }
        }
    }
}
