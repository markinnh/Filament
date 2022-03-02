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
using System.Reflection;

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

        public IEnumerable<FilamentDefn> Filaments { get => Singleton<DAL.DataLayer>.Instance.FilamentList; }
        public IEnumerable<string> SupportedColorsCode { get => from ty in typeof(System.Windows.Media.Colors).GetProperties(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public)
                                                                select ty.Name; }

        public IEnumerable<string> Names
        {
            get
            {
                return from ty in typeof(System.Windows.Media.Colors).GetProperties(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public)
                       select ty.Name;
            }
        }
        public string ChildTypename => SelectedItem?.GetType().GetCustomAttribute<UIHintsAttribute>()?.AddType??"Nothing selected";
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

        protected override IEnumerable<VendorDefn>? GetInUseItems() => Singleton<DAL.DataLayer>.Instance.GetFilteredVendors(v => !v.StopUsing);

        protected override IEnumerable<VendorDefn>? GetAllItems() => Singleton<DAL.DataLayer>.Instance.VendorList;
        protected override IEnumerable<VendorDefn>? GetFilteredItems(Func<VendorDefn, bool> predicate) => Singleton<DAL.DataLayer>.Instance.GetFilteredVendors(predicate);

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
        /*
        protected override void UpdateSelectedItemHander()
        {
            //base.UpdateSelectedItemHander();
            if (SelectedItem != null)
                if (SelectedItem.IsValid)
                {
                    DAL.Abstraction.UpdateItem(SelectedItem);
                    //SelectedItem.UpdateItem<FilamentContext>();

                    SelectedItem.SetContainedModifiedState(false);
                }
        }*/
    }
}
