using Filament.WPF6.Helpers;
using DataDefinitions;
using DataContext;
using DataDefinitions.Models;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Mvvm.Messaging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using DAL;

namespace Filament.WPF6.ViewModels
{
    public class TVendorDefnViewModel : BaseBrowserViewModel<VendorDefn,VendorDefn>
    {
        private ICommand? webNavigate;
        public ICommand WebNavigate { get => webNavigate ??= new RelayCommand<object>(HandleWebNavigate); }

        private void HandleWebNavigate(object? obj)
        {
            if (obj is Uri uri)
            {

                //var processStartInfo = new ProcessStartInfo("msedge.exe",uri.AbsoluteUri);
                Process.Start(new ProcessStartInfo("cmd", $"/c start {uri.AbsoluteUri}") { CreateNoWindow = true });
            }
            //throw new NotImplementedException();
        }
        public bool ListHasModifiedItems => Items?.Count(i => i.IsModified) > 0;
        protected override void PrepareForDataOperations()
        {
            VendorDefn.InDataOps = true;
            SpoolDefn.InDataOps = true;
        }

        protected override void FinishedDataOperations()
        {
            VendorDefn.InDataOps = false;
            SpoolDefn.InDataOps = false;
        }
        protected override void DerivedInitItems()
        {
            PrepareForDataOperations();
#pragma warning disable CS8604 // Possible null reference argument.
            InitItems(GetAllItems());
#pragma warning restore CS8604
            // Possible null reference argument.
            //var defns = FilamentContext.GetAllVendors();
            //if (defns != null)
            //{
            //    InitItems(defns);
            //}
            FinishedDataOperations();
        }
        protected override IEnumerable<VendorDefn>? GetAllItems() => Singleton<DataLayer>.Instance.VendorList;
        protected override IEnumerable<VendorDefn>? GetInUseItems() => Singleton<DataLayer>.Instance.GetFilteredVendors(v => !v.StopUsing);

        protected override IEnumerable<VendorDefn>? GetFilteredItems(Func<VendorDefn, bool> predicate) => Singleton<DataLayer>.Instance.GetFilteredVendors(predicate);
        public IEnumerable<FilamentDefn> Filaments => Singleton<DataLayer>.Instance.FilamentList;
        protected override void UpdateSelectedItemHander()
        {
            if (SelectedItem != null)
            {
                var needAdd = !SelectedItem.InDatabase;

                DAL.Abstraction.UpdateItem(SelectedItem);
                //SelectedItem.UpdateItem<FilamentContext>();
                if (Items?.Count(i => i.VendorDefnId == SelectedItem.VendorDefnId) == 0 && needAdd)
                    Items.Add(SelectedItem);

                if (needAdd)
                    Singleton<DAL.DataLayer>.Instance.Add(SelectedItem);

                SelectedItem.IsModified = false;
                SelectedItem.SetContainedModifiedState(false);
            }
        }
        protected override void PostItemsInitialization()
        {
            if (Items != null)
                SelectedItem = Items.First();
        }
        public TVendorDefnViewModel()
        {
            WeakReferenceMessenger.Default.Register<Helpers.VendorDefnListChanged>(this,HandleVendorDefnCollectionChanged);
        }
        ~TVendorDefnViewModel()
        {
            WeakReferenceMessenger.Default.Unregister<Helpers.VendorDefnListChanged>(this);
        }
        private void HandleVendorDefnCollectionChanged(object recipient, VendorDefnListChanged message)
        {
            if(message.Value != null)
            {
                Items=message.Value;
                SelectedItem=Items.FirstOrDefault();
            }    
            //throw new NotImplementedException();
        }
    }
}
