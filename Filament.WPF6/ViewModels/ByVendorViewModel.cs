using DataDefinitions;
using DataDefinitions.Models;
//using DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Input;
using Filament.WPF6.Helpers;
using System.Reflection;
using DataDefinitions.JsonSupport;
using DataDefinitions.LiteDBSupport;
using Filament.WPF6.Pages;
using DataDefinitions.Interfaces;
using CommunityToolkit.Mvvm.Messaging;
using DataDefinitions.Filters;

namespace Filament.WPF6.ViewModels
{
    public class ByVendorViewModel : BaseTagFilterViewModel<VendorDefn, DataDefinitions.DataObject>
    {
        public override Guid Signature => Singleton<LiteDBDal>.Instance.Vendors.Signature;
        //private IEnumerable<string> _tags;
        //private bool _tagFilterApplied;
        public ByVendorViewModel() : base()
        {

        }
        ~ByVendorViewModel()
        {
            //if (_tagFilterApplied)
            //    ViewSource.Filter -= ViewSource_Filter;
        }
        //private object? selectedObject;

        //public object? SelectedObject
        //{
        //    get => selectedObject;
        //    set => Set<object?>(ref selectedObject, value);
        //}

        public IEnumerable<FilamentDefn> Filaments { get => Singleton<LiteDBDal>.Instance.Filaments; }
        public IEnumerable<string> SupportedColorsCode
        {
            get => from ty in typeof(System.Windows.Media.Colors).GetProperties(BindingFlags.Static | BindingFlags.Public)
                   select ty.Name;
        }

        public IEnumerable<string> Names
        {
            get
            {
                return from ty in typeof(System.Windows.Media.Colors).GetProperties(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public)
                       select ty.Name;
            }
        }
        public string ChildTypename => SelectedItem?.GetType().GetCustomAttribute<UIHintsAttribute>()?.AddType ?? "Nothing selected";
        protected override void DerivedInitItems()
        {
            ViewSource.Source = Singleton<LiteDBDal>.Instance.Vendors;

            //PrepareForDataOperations();
            //if (GetAllItems() is IEnumerable<VendorDefn> items)
            //{
            //    InitItems(items);
            //    //if (Items == null)
            //    //    Items = new System.Collections.ObjectModel.ObservableCollection<VendorDefn>(items);
            //    //else
            //    //{
            //    //    Items.Clear();

            //    //    foreach (VendorDefn item in items)
            //    //        Items.Add(item);
            //    //}
            //}

            //FinishedDataOperations();

        }
//        protected override void InitFilterViewModel()
//        {
//            //DistinctTagStats = Singleton<LiteDBDal>.Instance.Vendors.DistinctTagStats;
//            //DistinctTagStats = Singleton<WordCollect>.Instance.OrganizeTags(ViewSource.View);
//            Signature = Singleton<LiteDBDal>.Instance.Vendors.Signature;
//#if DEBUG
//            filterSupported[(int)IResolveFilter.Filters.Tag] = filters.TryAdd(IResolveFilter.Filters.Tag, new WindowsFilter(new TagResolve() { Signature = Signature }));
//#else
//            filterSupported[(int)IResolveFilter.Filters.Tag] =filters.TryAdd(IResolveFilter.Filters.Tag, new WindowsFilter( new TagResolve() { Signature = Signature }));
//#endif
//            WeakReferenceMessenger.Default.Register<TagFilterChangedEventArgs>(this, HandleTagFilterMessages);
//        }
        //private void HandleFilterMessages(object recipient, FilterChangedEventArgs message)
        //{
        //    _tags = message.SelectedTags;

        //    if (message.FilterState == TagFilterState.FilterApplied)
        //        ViewSource.Filter += ViewSource_Filter;
        //    else if (message.FilterState == TagFilterState.FilterRemoved)
        //        ViewSource.Filter -= ViewSource_Filter;
        //    else
        //        ViewSource.View.Refresh();

        //    _tagFilterApplied = message.FilterState == TagFilterState.FilterApplied;
        //    //throw new NotImplementedException();
        //}

        //private void ViewSource_Filter(object sender, System.Windows.Data.FilterEventArgs e)
        //{
        //    if (_tags != null && e.Item is VendorDefn defn)
        //    {
        //        e.Accepted = _tags.All(t => defn.Tags?.Contains(t)??false);
        //    }
        //    else
        //        e.Accepted = true;
        //    //throw new NotImplementedException();
        //}

        public bool ExpandTreeNodesOnStartup
        {
            get => Properties.Settings.Default.ExpandNodesOnStartup;
            set
            {
                Properties.Settings.Default.ExpandNodesOnStartup = value;
                Properties.Settings.Default.Save();
                OnPropertyChanged(nameof(ExpandTreeNodesOnStartup));
            }
        }

        //[Obsolete]
        //protected override IEnumerable<VendorDefn>? GetInUseItems() => throw new NotSupportedException();
        //[Obsolete]
        //protected override IEnumerable<VendorDefn>? GetAllItems() => Singleton<LiteDBDal>.Instance.Vendors;
        //[Obsolete]
        //protected override IEnumerable<VendorDefn>? GetFilteredItems(Func<VendorDefn, bool> predicate) => throw new NotSupportedException();

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
