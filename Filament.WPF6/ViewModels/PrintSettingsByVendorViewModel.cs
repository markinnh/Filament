using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using DataDefinitions;
using DataDefinitions.JsonSupport;
using DataDefinitions.LiteDBSupport;
using DataDefinitions.Models;
using Filament.WPF6.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Filament.WPF6.ViewModels
{
    public class PrintSettingsByVendorViewModel : BaseBrowserViewModel<VendorDefn, DataDefinitions.DatabaseObject>
    {
        public IEnumerable<FilamentDefn> Filaments { get => Singleton<LiteDBDal>.Instance.Filaments; }
        public IEnumerable<PrintSettingDefn> PrintSettings { get => Singleton<LiteDBDal>.Instance.PrintSettings; }
        protected override bool SupportsShowAllFiltering => false;
        protected override void DerivedInitItems()
        {
            ViewSource.Source = Singleton<LiteDBDal>.Instance.Vendors;
            //if (GetAllItems() is IEnumerable<VendorDefn> items)
            //    InitItems(items);
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
            //if (Items != null)
            //    foreach (var vendorDefn in Items)
            //        foreach (var vendorSetting in vendorDefn.VendorSettings)
            //        {
            //            //var removeItems=;  commented out, and made into a single line of code, should work.
            //            foreach (var ci in vendorSetting.ConfigItems.Where(ci=>ci.PrintSettingDefn== message.Value).ToArray())
            //                vendorSetting.ConfigItems.Remove(ci);
            //        }
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
            //if (message.Value == DatabaseObjectActions.ItemRemoved && GetAllItems() is IEnumerable<VendorDefn> defns)
            //    InitItems(defns);
            //throw new NotImplementedException();
        }


        protected override void FinishedDataOperations()
        {
            //throw new NotImplementedException();
        }
        //[Obsolete]
        //protected override IEnumerable<VendorDefn>? GetInUseItems() => throw new NotImplementedException();
        //[Obsolete]
        //protected override IEnumerable<VendorDefn>? GetAllItems() => throw new NotImplementedException();
        //[Obsolete]
        //protected override IEnumerable<VendorDefn>? GetFilteredItems(Func<VendorDefn, bool> predicate) => throw new NotImplementedException();

        protected override void PrepareForDataOperations()
        {
            //throw new NotImplementedException();
        }

        private DataDefinitions.DatabaseObject? selectedRow;

        public DataDefinitions.DatabaseObject SelectedRow
        {
            get => selectedRow;
            set => base.Set<DataDefinitions.DatabaseObject>(ref selectedRow, value);
        }

        private ICommand deleteSelectedRow = new RelayCommand<DataDefinitions.DatabaseObject>(HandleDeleteSelectedRow);
        public ICommand DeleteSelectedRow => deleteSelectedRow;

        private static void HandleDeleteSelectedRow(DataDefinitions.DatabaseObject? obj)
        {
            if (obj != null && obj is DataDefinitions.DatabaseObject)
                obj = null;

            //throw new NotImplementedException();
        }
    }
}
