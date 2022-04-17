using DataDefinitions;
using DataDefinitions.Models;
using Filament.WPF6.Helpers;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Filament.WPF6.ViewModels
{
    public class PrintSettingsByVendorViewModel : BaseBrowserViewModel<VendorDefn, DatabaseObject>
    {
        public IEnumerable<FilamentDefn> Filaments { get => Singleton<DAL.DataLayer>.Instance.FilamentList; }
        public IEnumerable<PrintSettingDefn> PrintSettings { get => Singleton<DAL.DataLayer>.Instance.PrintSettingsList; }
        protected override bool SupportsFiltering => false;
        protected override void DerivedInitItems()
        {

            if (GetAllItems() is IEnumerable<VendorDefn> items)
                InitItems(items);
            //{
            //    if (Items == null)
            //        Items = new System.Collections.ObjectModel.ObservableCollection<VendorDefn>(items);
            //    else
            //    {
            //        Items.Clear();

            //        foreach (VendorDefn item in items)
            //            Items.Add(item);
            //    }
            //}
            WeakReferenceMessenger.Default.Register<ItemAdded<PrintSettingDefn>>(this, HandlePrintSettingAdded);
            WeakReferenceMessenger.Default.Register<ItemRemoved<PrintSettingDefn>>(this, HandlePrintSettingRemoved);
        }

        private void HandlePrintSettingRemoved(object recipient, ItemRemoved<PrintSettingDefn> message)
        {
            if (Items != null)
                foreach (var vendorDefn in Items)
                    foreach (var vendorSetting in vendorDefn.VendorSettings)
                    {
                        //var removeItems=;  commented out, and made into a single line of code, should work.
                        foreach (var ci in vendorSetting.ConfigItems.Where(ci=>ci.PrintSettingDefn== message.Value).ToArray())
                            vendorSetting.ConfigItems.Remove(ci);
                    }
            //throw new NotImplementedException();
        }

        private void HandlePrintSettingAdded(object recipient, ItemAdded<PrintSettingDefn> message)
        {
            OnPropertyChanged(nameof(PrintSettings));
            //throw new NotImplementedException();
        }

        private void HandlePrintSettingDefnListChanged(object recipient, PrintSettingDefnListChanged message)
        {
            if (message is PrintSettingDefnListChanged)
                OnPropertyChanged(nameof(PrintSettings));
            if (message.Value == DatabaseObjectActions.ItemRemoved && GetAllItems() is IEnumerable<VendorDefn> defns)
                InitItems(defns);
            //throw new NotImplementedException();
        }


        protected override void FinishedDataOperations()
        {
            //throw new NotImplementedException();
        }

        protected override IEnumerable<VendorDefn>? GetInUseItems() => Singleton<DAL.DataLayer>.Instance.GetFilteredVendors(v => !v.StopUsing);

        protected override IEnumerable<VendorDefn>? GetAllItems() => Singleton<DAL.DataLayer>.Instance.VendorList;
        protected override IEnumerable<VendorDefn>? GetFilteredItems(Func<VendorDefn, bool> predicate) => Singleton<DAL.DataLayer>.Instance.GetFilteredVendors(predicate);

        protected override void PrepareForDataOperations()
        {
            //throw new NotImplementedException();
        }
        private DatabaseObject? selectedRow;

        public DatabaseObject SelectedRow
        {
            get => selectedRow;
            set => Set<DatabaseObject>(ref selectedRow, value);
        }

        private ICommand deleteSelectedRow = new RelayCommand<DatabaseObject>(HandleDeleteSelectedRow);
        public ICommand DeleteSelectedRow => deleteSelectedRow;

        private static void HandleDeleteSelectedRow(DatabaseObject? obj)
        {
            if (obj != null && obj is DatabaseObject)
                DAL.Abstraction.Remove(obj);

            //throw new NotImplementedException();
        }
    }
}
