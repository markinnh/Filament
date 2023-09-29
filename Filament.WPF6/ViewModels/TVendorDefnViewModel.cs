using Filament.WPF6.Helpers;
using DataDefinitions;
//using DataContext;
using DataDefinitions.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
//using DAL;
using System.Windows.Controls;
using DataDefinitions.JsonSupport;
using System.Collections.ObjectModel;
using DataDefinitions.LiteDBSupport;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using DataDefinitions.Filters;
using DataDefinitions.Interfaces;

namespace Filament.WPF6.ViewModels
{
    public class TVendorDefnViewModel : BaseTagFilterViewModel<VendorDefn,VendorDefn> //BaseBrowserViewModel<VendorDefn, VendorDefn>
    {
        public override Guid Signature => Singleton<LiteDBDal>.Instance.Vendors.Signature;
        DataGridLengthConverter dglConvert = new DataGridLengthConverter();
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
        public bool ListHasModifiedItems => ((IEnumerable<VendorDefn>)ViewSource.Source).Count(i => i.IsModified) > 0;
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
            ViewSource.Source=Singleton<LiteDBDal>.Instance.Vendors;
            //InitItems(GetAllItems());
#pragma warning restore CS8604
            // Possible null reference argument.
            //var defns = FilamentContext.GetAllVendors();
            //if (defns != null)
            //{
            //    InitItems(defns);
            //}
            FinishedDataOperations();
        }
        //protected override IEnumerable<VendorDefn>? GetAllItems() => throw new NotSupportedException();
        //protected override IEnumerable<VendorDefn>? GetInUseItems() => throw new NotSupportedException();

        //protected override IEnumerable<VendorDefn>? GetFilteredItems(Func<VendorDefn, bool> predicate) => throw new NotSupportedException();
        public IEnumerable<FilamentDefn> Filaments => Singleton<LiteDBDal>.Instance.Filaments;

        public string DescriptionColumnWidth
        {
            get => Properties.Settings.Default.DescriptionColumnWidth;
            set => Properties.Settings.Default.DescriptionColumnWidth = dglConvert.ConvertToString(value);
        }
        //protected override void UpdateSelectedItemHander()
        //{
        //    if (SelectedItem != null)
        //    {
        //        //var needAdd = !SelectedItem.InDatabase;

        //        //DAL.Abstraction.UpdateItem(SelectedItem);
        //        SelectedItem.UpdateItem(Singleton<LiteDBDal>.Instance);
        //        //if (Items?.Count(i => i.VendorDefnId == SelectedItem.VendorDefnId) == 0 && needAdd)
        //        //    Items.Add(SelectedItem);

        //        //if (needAdd)
        //        //    Singleton<LiteDBDal>.Instance.Add(SelectedItem);

        //    }
        //}
        protected override void PostItemsInitialization()
        {
            //if (Items != null)
            //    SelectedItem = Items.First();
            if(ViewSource.View.CurrentItem is VendorDefn defn)
                SelectedItem= defn; 
        }
        public TVendorDefnViewModel()
        {
            //WeakReferenceMessenger.Default.Register<Helpers.VendorDefnListChanged>(this, HandleVendorDefnCollectionChanged);
        }
        ~TVendorDefnViewModel()
        {
            //WeakReferenceMessenger.Default.Unregister<Helpers.VendorDefnListChanged>(this);
        }
        private void HandleVendorDefnCollectionChanged(object recipient, VendorDefnListChanged message)
        {
            //if (message.Value != null)
            //{
            //    Items = message.Value;
            //    SelectedItem = Items.FirstOrDefault();
            //}
            //throw new NotImplementedException();
        }

//        protected override void InitFilterViewModel()
//        {
//            Signature = Singleton<LiteDBDal>.Instance.Vendors.Signature;
//#if DEBUG
//            filterSupported[(int)IResolveFilter.Filters.Tag] = filters.TryAdd(IResolveFilter.Filters.Tag, new WindowsFilter(new TagResolve() { Signature = Signature }));
//#else
//            filterSupported[(int)IResolveFilter.Filters.Tag] =filters.TryAdd(IResolveFilter.Filters.Tag, new WindowsFilter( new TagResolve() { Signature = Signature }));
//#endif
//            WeakReferenceMessenger.Default.Register<TagFilterChangedEventArgs>(this, HandleTagFilterMessages);
//        }
    }
}
