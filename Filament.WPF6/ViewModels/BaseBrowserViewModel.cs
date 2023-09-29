using Filament.WPF6.Helpers;
using DataDefinitions;
using DataDefinitions.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MyLibraryStandard.Attributes;
using static System.Diagnostics.Debug;
using System.Diagnostics;
using DataDefinitions.JsonSupport;
using System.Windows.Data;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Input;
using DataDefinitions.Interfaces;
using DataDefinitions.LiteDBSupport;
using System.Collections.Concurrent;
using DataDefinitions.Filters;

namespace Filament.WPF6.ViewModels
{
    public abstract class BaseBrowserViewModel<TBrowse, TSelect> : Observable,IUIProvider, DataDefinitions.Interfaces.IFilterStatus where TBrowse : DataDefinitions.DataObject, new()
        where TSelect : DataDefinitions.DataObject, new()
    {
#if DEBUG
        public bool ShowDebugElements => true;
#else
        public bool ShowDebugElements => false;
#endif
        protected const string showAllFilterKey = "ShowAll";
        protected const string tagFilterKey = "Tags";

        protected ConcurrentDictionary<IResolveFilter.Filters, IWindowsFilter> filters = new();
        protected ConcurrentDictionary<string,IEnumerable> menus = new();
        protected static bool ready = true;
        protected virtual bool SupportsShowAllFiltering { get; } = true;

        public bool Ready
        {
            get { return ready; }
            set { Set(ref ready, value); }
        }
        protected bool[] filterSupported = new bool[Enum.GetNames<IResolveFilter.Filters>().Length];
        public bool IsFilterSupported(IResolveFilter.Filters filter)
        {
            return filterSupported[(int)filter];
        }
        //protected bool _keywordFilteringSupported = false;
        //public bool KeywordFilteringSupported { get => _keywordFilteringSupported; protected set => Set(ref _keywordFilteringSupported, value); }
        //protected bool _tagFilteringSupported = false;
        //public bool TagFilteringSupported { get => _tagFilteringSupported; protected set => Set(ref _tagFilteringSupported, value); }
        //private bool dateFilteringSupported=false;

        //public bool DateFilteringSupported
        //{
        //    get => dateFilteringSupported;
        //    set => Set<bool>(ref dateFilteringSupported, value);
        //}


        //private ObservableCollection<TBrowse>? _browse;
        //public ObservableCollection<TBrowse>? Items { get => _browse; set => Set(ref _browse, value); }
        public CollectionViewSource ViewSource { get; set; }
        private TSelect? selectedItem;
        [Affected(Names = new string[] { nameof(CanDelete), nameof(SelectedItemNotNull) })]
        public TSelect? SelectedItem
        {
            get => selectedItem;
            set
            {
                Set(ref selectedItem, value);
            }
        }
        public bool CanDelete => SelectedItem?.SupportsDelete ?? false;
        public bool SelectedItemNotNull => SelectedItem != null;

        public bool HasModifiedItems => ((IEnumerable<TBrowse>)ViewSource.Source).Any(it => it.IsModified) && !InAddNew;
        public bool CanAdd => !InAddNew;
        protected BaseBrowserViewModel()
        {
            ViewSource = new CollectionViewSource();
            if (SupportsShowAllFiltering)
            {
                //if (Singleton<JsonDAL>.Instance.Document.Settings.FirstOrDefault(s => s.Name == nameof(MainWindow.SelectShowFlag)) is Setting setting)
                //{
                //    if (Enum.Parse<ShowAllFlag>(setting.Value) is ShowAllFlag flag)
                //    {
                //        if (flag == ShowAllFlag.ShowAll)
                //            ShowAllItems();
                //        else
                //            ShowInUseItems();
                //    }
                //}
                filters.TryAdd(IResolveFilter.Filters.ShowAll, new WindowsFilter(new ShowAllResolve()));
                WeakReferenceMessenger.Default.Register<ShowAllFlagChanged>(this, HandleShowAllFlagChanged);
            }
            WeakReferenceMessenger.Default.Register<FilterChangedEventArgs>(this, HandleFilterChanged);
            DerivedInitItems();
            var showState = Enum.Parse<ShowAllFlag>(Properties.Settings.Default.ShowStateFlag);

            if (showState == ShowAllFlag.ShowAll)
                ShowAllItems();
            else
                ShowInUseItems();

            UpdateEventLinks();

        }

        private void HandleFilterChanged(object recipient, FilterChangedEventArgs message)
        {
            if (filters.TryGetValue(message.Filter, out var filter))
            {
                if (filter.Applied && message.Actions.HasFlag(FilterAction.Remove))
                {
                    filter.Applied = false;
                    ViewSource.Filter -= new FilterEventHandler(filter.Filter);
                }
                else if (!filter.Applied && message.Actions.HasFlag(FilterAction.Apply))
                {
                    filter.Applied = true;
                    ViewSource.Filter += new FilterEventHandler(filter.Filter);
                }
                ViewSource.View.Refresh();
            }
            //throw new NotImplementedException();
        }

        ~BaseBrowserViewModel()
        {
            if (SupportsShowAllFiltering)
                WeakReferenceMessenger.Default.Unregister<ShowAllFlagChanged>(this);
            if (filters.Values.Count > 0)
                foreach (var filter in filters.Values.Where(f => f.Applied))
                {
                    try
                    {
                        ViewSource.Filter -= new FilterEventHandler(filter.Filter);
                    }
                    catch (Exception ex)
                    {
                        WriteLine($"Unable to release a filter in {this.GetType().Name}, will probably result in a memory leak.  {ex.Message}");
                    }
                }
        }
        protected virtual void UpdateEventLinks()
        {
        }
        protected virtual void RemoveEventLinks()
        {
        }
        protected virtual void ShowAllItems()
        {
            if (filters.TryGetValue(IResolveFilter.Filters.ShowAll, out var filter))
                if (filter.Applied)
                {
                    filter.Applied = false;
                    ViewSource.Filter -= new FilterEventHandler(filter.Filter);
                }

            //ViewSource.Filter -= FilterForITrackUsage;
        }
        protected virtual void ShowInUseItems()
        {
            //ViewSource.Filter += FilterForITrackUsage;
            if (filters.TryGetValue(IResolveFilter.Filters.ShowAll, out var filter))
                if (!filter.Applied)
                {
                    if (filter.Resolve is ICriteriaFilter criteria)
                        criteria.UpdateCriteria(false);

                    ViewSource.Filter += new FilterEventHandler(filter.Filter);
                    filter.Applied = true;
                }
        }

        private void FilterForITrackUsage(object sender, FilterEventArgs e)
        {
            if (e.Item is ITrackUsable track)
            {
                e.Accepted = !track.StopUsing ?? true;
            }
        }

        protected virtual void HandleShowAllFlagChanged(object recipient, ShowAllFlagChanged message)
        {
            if (message.Value == ShowAllFlag.ShowAll)
                ShowAllItems();
            else
                ShowInUseItems();

            //SelectedItem = Items?.FirstOrDefault();
            WriteLine($"Show all flag changed to : {message.Value} in {this.GetType().Name}");
        }

        protected bool InAddNew { get; set; }
        //private void Items_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        //{
        //    if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add && e.NewItems != null)
        //        LinkRange(e.NewItems);
        //    else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove && e.OldItems != null)
        //        UnlinkRange(e.OldItems);
        //    else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Reset)
        //    {
        //        if (e.OldItems != null)
        //            UnlinkRange(e.OldItems);
        //        if (e.NewItems != null)
        //            LinkRange(e.NewItems);
        //    }
        //}
        //[Obsolete]
        //protected abstract IEnumerable<TBrowse>? GetInUseItems();
        //[Obsolete]
        //protected abstract IEnumerable<TBrowse>? GetAllItems();
        //[Obsolete]
        //protected abstract IEnumerable<TBrowse>? GetFilteredItems(Func<TBrowse, bool> predicate);
        protected void InitItems(IEnumerable<TBrowse> newItems)
        {
            //if (newItems != null)
            //{
            //    PreItemsInitialization();
            //    if (Items != null)
            //    {
            //        RemoveEventLinks();
            //        Items.Clear();
            //        foreach (var filament in newItems)
            //        {
            //            Items.Add(filament);
            //            //filament?.InitNotificationHandler();
            //        }
            //    }
            //    else
            //    {
            //        Items = new System.Collections.ObjectModel.ObservableCollection<TBrowse>(newItems);
            //        //foreach (var filament in filaments)
            //        //    filament.InitNotificationHandler();
            //    }
            //    UpdateEventLinks();
            //    //SelectedItem = Items.First();
            //    PostItemsInitialization();
            //}
        }
        protected virtual void PreItemsInitialization()
        {
            System.Diagnostics.Debug.WriteLine($"No pre-processing has been conducted in {GetType().Name}.");
        }
        protected virtual void PostItemsInitialization()
        {
            System.Diagnostics.Debug.WriteLine($"No post processing has been conducted in {GetType().Name}.");
        }

        protected abstract void DerivedInitItems();

        protected abstract void PrepareForDataOperations();
        protected abstract void FinishedDataOperations();
        protected virtual void Def_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(nameof(HasModifiedItems));
        }

        private ICommand? updateCommand;
        public ICommand UpdateCommand { get => updateCommand ??= new RelayCommand(UpdateSelectedItemHander); }

        private ICommand? newItemCommand;

        public ICommand NewItemCommand { get => newItemCommand ??= new RelayCommand(NewItemHandler); }

        private ICommand? saveAllChangesCommand;
        public ICommand SaveAllChangesCommand { get => saveAllChangesCommand ??= new RelayCommand(SaveAllChangesHandler); }

        private ICommand? deleteSelectedCommand;
        public ICommand DeleteSelectedCommand { get => deleteSelectedCommand ??= new RelayCommand(DeleteSelectedHandler); }
        protected virtual void SaveAllChangesHandler()
        {
            try
            {
                //var updateItems = ;
                if (((IEnumerable<TBrowse>)ViewSource.Source).Where(it => it.IsModified).ToList() is IEnumerable<TBrowse> tbrowse)
                {
                    foreach (TBrowse tb in tbrowse)
                        tb.UpdateItem(Singleton<LiteDBDal>.Instance);

                    OnPropertyChanged(nameof(HasModifiedItems));
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Unable to complete data operations.  The following error was returned.  {ex.Message}, Inner Exception : {ex.InnerException?.Message}");
            }
            //System.Diagnostics.Debug.WriteLine("Save all changes not implemented.");
        }

        protected virtual void NewItemHandler()
        {
            InAddNew = true;
            SelectedItem = new TSelect();
            OnPropertyChanged(nameof(HasModifiedItems));
            OnPropertyChanged(nameof(CanAdd));
            //System.Diagnostics.Debug.WriteLine("Add new item not implemented.");
        }

        protected virtual void UpdateSelectedItemHander()
        {
            if (SelectedItem != null)
            {
                if (SelectedItem.IsValid)
                {
                    //bool needsAdd;

                    //needsAdd = !SelectedItem.InDatabase;
                    //DAL.Abstraction.UpdateItem(SelectedItem);
                    SelectedItem.UpdateItem(Singleton<LiteDBDal>.Instance);
                    if (SelectedItem is ITag)
                        //TODO: Develop a notification that the tag list has changed
                        WeakReferenceMessenger.Default.Send(new TagInteractionNotification(ContentInteraction.ContentUpdated));
                    //if (needsAdd)
                    //{
                    //    //Singleton<JsonDAL>.Instance.Document.Add(SelectedItem);
                    //    //if (SelectedItem is TBrowse select)
                    //    //    Singleton<JsonDAL>.Instance.Document.Add(select);
                    //}
                    SelectedItem.SetContainedModifiedState(false);
                    OnPropertyChanged(nameof(SelectedItemNotNull));
                    OnPropertyChanged(nameof(CanDelete));
                }
                else
                {
                    WriteLine("Selected Item is not valid.");
                }
            }
            else
                WriteLine("SelectedItem is null");

            //System.Diagnostics.Debug.WriteLine($"Update selected item for {typeof(TBrowse).Name} not implemented.");
        }
        protected virtual void UpdateAfterItemDelete() { }
        protected virtual void DeleteSelectedHandler()
        {
            if (SelectedItem != null)
            {
                //Singleton<DAL.DataLayer>.Instance.Remove(SelectedItem);
                // TODO: Develop code for removing an item from the database, or stopping the deletion if it has references in other objects,
                // currently if the Item supports ITrackUsable it is flagged to stop using it.
                if (SelectedItem is ISupportDelete supportDelete)
                    supportDelete.Delete();
                else if (SelectedItem is ITrackUsable track)
                    track.StopUsing = true;

                //DAL.Abstraction.Remove(SelectedItem);
                UpdateAfterItemDelete();
            }
            // TODO: This needs a lot of work and consideration whether to support deleting items.
            //try
            //{
            //    if (SelectedItem != null)
            //    {
            //        if (SelectedItem is FilamentDefn filament)
            //        {
            //            if (!filament.IsIntrinsic)
            //            {
            //                PrepareForDataOperations();

            //                FilamentContext.DeleteItems(SelectedItem);
            //                FinishedDataOperations();
            //            }
            //        }


            //    }
            //}
            //catch (Exception ex)
            //{
            //    System.Diagnostics.Debug.WriteLine($"Unable to complete data operations.  The following error was returned.  {ex.Message}, Inner Exception : {ex.InnerException?.Message}");
            //}
        }
        protected void LinkRange(IList ts)
        {
            if (ts != null)
            {
                foreach (var item in ts)
                {
                    if (item is Observable obs)
                        obs.PropertyChanged += Def_PropertyChanged;
                }
            }
        }
        protected void UnlinkRange(IList ts)
        {
            if (ts != null)
            {
                foreach (var item in ts)
                {
                    if (item is Observable observable)
                        observable.PropertyChanged -= Def_PropertyChanged;
                }
            }
        }

        public bool IsFilterApplied(IResolveFilter.Filters whichFilter)
        {
            if (filters.TryGetValue(whichFilter, out var filter))
                return filter.Applied;
            else
                return false;
            //throw new NotImplementedException();
        }
        /// <summary>
        /// Retrieve any UIElements specific to this view
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException">currently not implemented in base view</exception>
        public virtual IEnumerable GetMenuElements(string Key)
        {
            throw new NotImplementedException();
        }
        public bool TryGetElements<T>(string Key, out T Value)
        {
            try
            {
                if(GetMenuElements(Key) is T result)
                {
                    Value= result;
                    return true;
                }
            }
            catch (NotImplementedException)
            {
                Value = default;
                return false;
            }
            throw new NotImplementedException ();
        }
    }
}
