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
using static System.Diagnostics.Trace;
using System.Diagnostics;
using DataDefinitions.JsonSupport;
using System.Windows.Data;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Input;
using DataDefinitions.Interfaces;
using DataDefinitions.LiteDBSupport;
using System.Collections.Concurrent;
using DataDefinitions.Filters;
using System.Windows;

namespace Filament.WPF6.ViewModels;

public abstract partial class BaseBrowserViewModel<TBrowse, TSelect> : Observable, DataDefinitions.Interfaces.IFilterStatus,IViewModel where TBrowse : DataDefinitions.DataObject, new()
    where TSelect : DataDefinitions.DataObject, new()
{
#if DEBUG
    public bool ShowDebugElements => true;
#else
    public bool ShowDebugElements => false;
#endif
    public Guid ViewModelGuid
    {
        get;
    }= Guid.NewGuid();
    public virtual void ReleaseEventHandlers()
    {
        if (SupportsShowAllFiltering)
            WeakReferenceMessenger.Default.Unregister<ShowAllFlagChanged>(this);

        WeakReferenceMessenger.Default.Unregister<FilterChangedEventArgs>(this);

        if (filters.Values.Count > 0)
            foreach (var filter in filters.Values.Where(f => f.Applied))
            {
                try
                {
                    lock (filter)
                    {
                        ViewSource.Filter -= new FilterEventHandler(filter.Filter);
                        filter.Applied = false;
                    }
                }
                catch (Exception ex)
                {
                    WriteLine($"Unable to release a filter in {this.GetType().Name}, will probably result in a memory leak.  {ex.Message}","Error");
                    Assert(false);
                }
            }

    }
    protected bool processingEvents = true;
    //protected const string showAllFilterKey = "ShowAll";
    //protected const string tagFilterKey = "Tags";

    protected ConcurrentDictionary<IResolveFilter.Filters, IWindowsFilter> filters = new();
    protected ConcurrentDictionary<string,IEnumerable> menus = new();
    protected static bool ready = true;
    protected virtual bool SupportsShowAllFiltering { get; } = false;

    public bool Ready
    {
        get=>ready; 
        set=> Set(ref ready, value);
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
    public virtual void InitFilterUI()
    {
        UpdateFilterUI();// overridden where filters are used
        //throw new NotImplementedException();
    }
    protected void UpdateFilterUI(bool TagFilterEnabled = false, bool DateFilterEnabled = false, bool KeywordFilterEnabled = false)
    {
        var args = new FilterUIChangedEventArgs(TagFilterEnabled, DateFilterEnabled, KeywordFilterEnabled);
        WeakReferenceMessenger.Default.Send(args);
    }

    //private event EventHandler<SelectedItemChangedEventArgs> SelectedItemChanged;
    private TSelect? selectedItem;
    [Affected(nameof(CanDelete), nameof(SelectedItemNotNull))]
    public TSelect? SelectedItem
    {
        get => selectedItem;
        set => Set(ref selectedItem, value);
    }
    public virtual bool CanDelete => (SelectedItem?.SupportsDelete ?? false) && !SelectedItemHasDependencies() ;
    protected bool SelectedItemHasDependencies()
    {
        if(SelectedItem is DatabaseObject db)
            return db.HasDependencies(Singleton<LiteDBDal>.Instance);
        else
            return false;
    }
    public bool SelectedItemNotNull => SelectedItem != null;

    public bool HasModifiedItems => ((IEnumerable<TBrowse>)ViewSource.Source).Any(it => it.IsModified) && !InAddNew;
    public bool CanAdd => !InAddNew;
    protected BaseBrowserViewModel()
    {
        ViewSource = new( ) { IsLiveFilteringRequested=true};
        if (CollectionViewSource.GetDefaultView(ViewSource) is IEditableCollectionView icv)
        {
            icv.NewItemPlaceholderPosition = NewItemPlaceholderPosition.None;
        }
        //Trace.WriteLine($"CollectionViewType = {ViewSource.CollectionViewType?.GetType().Name}", "Information");
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
            //filters.TryAdd(IResolveFilter.Filters.ShowAll, new WindowsFilter(new ShowAllResolve()));
            WeakReferenceMessenger.Default.Register<ShowAllFlagChanged>(this, HandleShowAllFlagChanged);
        }
        WeakReferenceMessenger.Default.Register<FilterChangedEventArgs>(this, HandleFilterChanged);
        DerivedInitItems();
        //var showState = Enum.Parse<ShowAllFlag>(Properties.Settings.Default.ShowStateFlag);

        //if (showState == ShowAllFlag.ShowAll)
        //    ShowAllItems();
        //else
        //    ShowInUseItems();

        UpdateEventLinks();

    }

    private void HandleFilterChanged(object recipient, FilterChangedEventArgs message)
    {
        WriteLine($"Filter change received in {GetType().Name}, Actions {message.Actions}","Information");
        if (filters.TryGetValue(message.Filter, out var filter) && processingEvents)
        {
            if (filter.Applied && message.Actions.HasFlag(FilterAction.Remove))
            {
                try
                {
                    ViewSource.Filter -= new FilterEventHandler(filter.Filter);

                }
                catch (Exception)
                {
                    WriteLine($"unable to remove the {message.Filter} from {GetType().Name}","Error");
                    Assert(false);
                    //throw;
                }
                filter.Applied = false;
            }
            else if (!filter.Applied && message.Actions.HasFlag(FilterAction.Apply))
            {
                try
                {
                    ViewSource.Filter += new FilterEventHandler(filter.Filter);
                }
                catch (Exception)
                {
                    WriteLine($"unable to apply the {message.Filter} from {GetType().Name}","Error");
                    Assert(false);
                    //throw;
                }
                filter.Applied = true;
            }
            try
            {
                ViewSource.View.Refresh();
            }
            catch (Exception)
            {
                WriteLine($"unable to refresh the view for {message.Filter} from {GetType().Name}","Error");
                Assert(false);
                //throw;
            }
        }
        //throw new NotImplementedException();
    }

    ~BaseBrowserViewModel()
    {
        if (SupportsShowAllFiltering)
            WeakReferenceMessenger.Default.Unregister<ShowAllFlagChanged>(this);

        WeakReferenceMessenger.Default.Unregister<FilterChangedEventArgs>(this);

        if (filters.Values.Count > 0)
            foreach (var filter in filters.Values.Where(f => f.Applied))
            {
                try
                {
                    lock (this)
                    {
                        ViewSource.Filter -= new FilterEventHandler(filter.Filter);
                        filter.Applied = false;
                    }
                }
                catch (Exception ex)
                {
                    WriteLine($"Unable to release a filter in {this.GetType().Name}, will probably result in a memory leak.  {ex.Message}","Exception");
                    Assert(false);
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
        //if (filters.TryGetValue(IResolveFilter.Filters.ShowAll, out var filter))
        //    if (filter.Applied)
        //    {
        //        filter.Applied = false;
        //        ViewSource.Filter -= new FilterEventHandler(filter.Filter);
        //    }

        //ViewSource.Filter -= FilterForITrackUsage;
    }
    protected virtual void ShowInUseItems()
    {
        //ViewSource.Filter += FilterForITrackUsage;
        //if (filters.TryGetValue(IResolveFilter.Filters.ShowAll, out var filter))
        //    if (!filter.Applied)
        //    {
        //        if (filter.Resolve is ICriteriaFilter criteria)
        //            criteria.UpdateCriteria(false);

        //        ViewSource.Filter += new FilterEventHandler(filter.Filter);
        //        filter.Applied = true;
        //    }
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
        WriteLine($"Show all flag changed to : {message.Value} in {this.GetType().Name}","Information");
    }
    private bool inAddNew;
    [Affected(nameof(CanAdd))]
    public bool InAddNew
    {
        get => inAddNew;
        set => Set(ref inAddNew, value);
    }

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
       WriteLine($"No pre-processing has been conducted in {GetType().Name}.","Information");
    }
    protected virtual void PostItemsInitialization()
    {
        WriteLine($"No post processing has been conducted in {GetType().Name}.","Information");
    }

    protected abstract void DerivedInitItems();

    //protected abstract void PrepareForDataOperations();
    //protected abstract void FinishedDataOperations();
    protected virtual void Def_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        OnPropertyChanged(nameof(HasModifiedItems));
    }

    //private ICommand? updateCommand;
    //public ICommand UpdateCommand  => updateCommand ??= new RelayCommand(Update); 

    //private ICommand? newItemCommand;

    //public ICommand NewItemCommand => newItemCommand ??= new RelayCommand(NewItem);

    //private ICommand? saveAllChangesCommand;
    //public ICommand SaveAllChangesCommand => saveAllChangesCommand ??= new RelayCommand(SaveAllChanges);

    //private ICommand? deleteSelectedCommand;
    //public ICommand DeleteSelectedCommand => deleteSelectedCommand ??= new RelayCommand(DeleteSelected);
    [RelayCommand]
    protected virtual void SaveAllChanges()
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
            
            TraceError($"Unable to complete SaveAllChanges data operations.  The following error was returned.  {ex.Message}, Inner Exception : {ex.InnerException?.Message}");
            Debug.Assert(false);
        }
        //System.Diagnostics.Debug.WriteLine("Save all changes not implemented.");
    }
    [RelayCommand]
    protected virtual void NewItem()
    {
        InAddNew = true;
        SelectedItem = new TSelect();
        OnPropertyChanged(nameof(HasModifiedItems));
        OnPropertyChanged(nameof(CanAdd));
        //System.Diagnostics.Debug.WriteLine("Add new item not implemented.");
    }
    [RelayCommand]
    protected virtual void Update()
    {
        if (SelectedItem != null)
        {
            ///<remarks>
            /// No unnecessary trips to the database
            ///</remarks>
            if (SelectedItem.IsValid && SelectedItem.IsModified)
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
                InAddNew = false;
                
            }
            else
            {
                TraceInformation($"Selected Item \"{SelectedItem.GetType().Name}\" is {(SelectedItem.IsValid?string.Empty:"not ")}valid, IsModified = {SelectedItem.IsModified}.");
            }
        }
        else
            TraceInformation("SelectedItem is null");

        //System.Diagnostics.Debug.WriteLine($"Update selected item for {typeof(TBrowse).Name} not implemented.");
    }
    protected virtual void UpdateAfterItemDelete() { }
    [RelayCommand]
    protected virtual void DeleteSelected()
    {
        if (SelectedItem != null)
        {
            //Singleton<DAL.DataLayer>.Instance.Remove(SelectedItem);
            // TODO: Develop code for removing an item from the database, or stopping the deletion if it has references in other objects,
            // currently if the Item supports ITrackUsable it is flagged to stop using it.
            if (SelectedItem is IDeleteSupport iSupportDelete && SelectedItem.SupportsDelete)
                try
                {
                    iSupportDelete.Delete(Singleton<LiteDBDal>.Instance);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Unable to delete the selected item.  The following error was returned.  {ex.Message}, Inner Exception : {ex.InnerException?.Message}", "Error Deleting Item", MessageBoxButton.OK, MessageBoxImage.Error);
                }
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
    //private ICommand generateCode;
    //public ICommand GenerateCode => generateCode ??= new RelayCommand(CodeGenerate);
#if DEBUG
    public bool CodeGenSupported => true;
    [RelayCommand]
    private void CodeGenerate()
    {
        if (SelectedItem!=null)
            WriteLine(SelectedItem.GenerateCode());
    }
#else

    public bool CodeGenSupported=>false;
    [RelayCommand]
    private void CodeGenerate(){
        MessageBox.Show("Code Generation Not Supported in the release version of this app", "Not Supported", MessageBoxButton.OK, MessageBoxImage.Stop);
    }
#endif
}
