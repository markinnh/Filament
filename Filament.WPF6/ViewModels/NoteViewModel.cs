using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using DataDefinitions;
using DataDefinitions.Filters;
using DataDefinitions.Interfaces;
using DataDefinitions.LiteDBSupport;
using DataDefinitions.Models;
using Filament.WPF6.Helpers;
using Filament.WPF6.Properties;

namespace Filament.WPF6.ViewModels;

public partial class NoteViewModel : BaseTagFilterViewModel<NoteDefn, NoteDefn>
{
    //private bool _keywordFilteringSupported;
    public override Guid Signature => Singleton<LiteDBDal>.Instance.Notes.Signature;
    protected override void DerivedInitItems()
    {
        ViewSource.Source = Singleton<LiteDBDal>.Instance.Notes;
        //throw new NotImplementedException();
    }
    public bool KeywordFilterApplied
    {
        get
        {
            if (filters.TryGetValue(IResolveFilter.Filters.Keyword, out var filter))
                return filter.Applied;
            return false;
        }
    }
    public bool DateFilterApplied
    {
        get
        {
            if (filters.TryGetValue(IResolveFilter.Filters.Date, out var filter))
                return filter.Applied;
            return false;
        }
    }
    public bool CanFilterByDate => !TagFilterApplied&&!KeywordFilterApplied ;
    public bool CanFilterByKeyword => !TagFilterApplied&&!DateFilterApplied;
    public bool CanFilterByTag => !KeywordFilterApplied&&!DateFilterApplied;
    //protected override void FinishedDataOperations()
    //{
    //    //throw new NotImplementedException();
    //}
    public override void ReleaseEventHandlers()
    {
        base.ReleaseEventHandlers();
        WeakReferenceMessenger.Default.Unregister<KeywordFilterChangedEventArgs>(this);
        WeakReferenceMessenger.Default.Unregister<DateTimeFilterChangedEventArgs>(this);
    }
    protected override void InitFilterViewModel()
    {
        base.InitFilterViewModel();
        sortDirection= Enum.Parse<ListSortDirection>(Settings.Default.NotePageSortDirection);
        currentSortDescription = GetSortDirectionDescription();
        ViewSource.SortDescriptions.Add(currentSortDescription);
        //DistinctTagStats = Singleton<LiteDBDal>.Instance.Notes.DistinctTagStats;
        //DistinctTagStats = Singleton<WordCollect>.Instance.OrganizeTags(ViewSource.View);
        //Signature = Singleton<LiteDBDal>.Instance.Notes.Signature;

        //filterSupported[(int)IResolveFilter.Filters.Tag] = filters.TryAdd(IResolveFilter.Filters.Tag, new WindowsFilter(
        //    new TagResolve()
        //    {
        //        Signature = Signature
        //    }));

        filterSupported[(int)IResolveFilter.Filters.Keyword] = filters.TryAdd(IResolveFilter.Filters.Keyword, new WindowsFilter(new KeywordResolve()));
        //WeakReferenceMessenger.Default.Register<TagFilterChangedEventArgs>(this,
        //                                                                   HandleTagFilterMessages);
        WeakReferenceMessenger.Default.Register<KeywordFilterChangedEventArgs>(this,
                                                                               HandleKeywordFilterMessages);
        filterSupported[(int)IResolveFilter.Filters.Date] = filters.TryAdd(IResolveFilter.Filters.Date, new WindowsFilter(new DateTimeResolve()));
        WeakReferenceMessenger.Default.Register<DateTimeFilterChangedEventArgs>(this, HandleDateFilterChanged);
    }
    #region Sorting
    protected string sortField = nameof(NoteDefn.DateTime);
    private bool sortAscending = true;

    public bool SortAscending
    {
        get => sortAscending;
        set => Set(ref sortAscending, value);
    }
    protected ListSortDirection sortDirection = ListSortDirection.Ascending;
    protected SortDescription currentSortDescription
    {
        get; private set;
    }
    [RelayCommand]
    protected void ToggleSortDirection(object p)
    {
        ViewSource.SortDescriptions.Remove(currentSortDescription);

        sortDirection = SwitchSortDirection();
        Settings.Default.NotePageSortDirection = sortDirection.ToString();

        currentSortDescription = GetSortDirectionDescription();

        ViewSource.SortDescriptions.Add(currentSortDescription);
        if (p is bool b)
            SortAscending = b;
    }
    protected ListSortDirection SwitchSortDirection() => sortDirection == ListSortDirection.Ascending ? ListSortDirection.Descending : ListSortDirection.Ascending;
    protected SortDescription GetSortDirectionDescription() => new(sortField, sortDirection);
    #endregion
    public override void InitFilterUI()
    {
        var tagStatus=!KeywordFilterApplied&&!DateFilterApplied;
        var dateStatus = !TagFilterApplied && !KeywordFilterApplied;
        var keywordStatus = !TagFilterApplied && !DateFilterApplied;
        UpdateFilterUI(tagStatus, dateStatus, keywordStatus);
    }
    private void HandleDateFilterChanged(object recipient, DateTimeFilterChangedEventArgs message)
    {
        if (message != null && filters.TryGetValue(message.Filter, out var filter))
        {
            if(message.Actions.HasFlag(FilterAction.Apply) && !filter.Applied)
            {
                filter.Applied = true;
                try
                {
                    ViewSource.Filter += new System.Windows.Data.FilterEventHandler(filter.Filter);
                    OnPropertyChanged(nameof(CanFilterByTag));
                    OnPropertyChanged(nameof(CanFilterByKeyword));
                    UpdateFilterUI(false,true,false);
                }
                catch (Exception ex)
                {
                    filter.Applied = false;
                    Debug.WriteLine($"Unable to apply a date filter, {ex.Message}");
                    Debug.Assert(false);
                }
            }
            else if (message.Actions.HasFlag(FilterAction.Remove) && filter.Applied)
            {
                filter.Applied = false;
                try
                {
                    ViewSource.Filter -= new System.Windows.Data.FilterEventHandler(filter.Filter);
                    OnPropertyChanged(nameof(CanFilterByTag));
                    OnPropertyChanged(nameof(CanFilterByKeyword));
                    UpdateFilterUI(false);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"unable to remove the date filter, {ex.Message}");
                    Debug.Assert(false);
                    //throw;
                }
                finally { filter.Applied = false; }
            }
            else if(message.Actions.HasFlag(FilterAction.Update))
            {
                if (filter.Applied)
                {
                    filter.Resolve = message.Payload;
                }
            }
        }
        ViewSource.View.Refresh();
        //throw new NotImplementedException();
    }
    internal IEnumerable<WordWithOccuranceCount> GetWords()
    {
        ViewSource.View.Refresh();
        return Singleton<WordCollect>.Instance.OrganizeKeywords(ViewSource.View);
    }

    private void HandleKeywordFilterMessages(object recipient, KeywordFilterChangedEventArgs message)
    {
        if (message == null) return;
        if (filters.TryGetValue(IResolveFilter.Filters.Keyword, out var filter) && filter.Resolve is ICriteriaFilter criteria)
        {
            if (message.Actions.HasFlag(FilterAction.Update))
            {
                if (criteria.CriteriaSet)
                    criteria.UpdateCriteria(message);
                else
                    criteria.SetCriteria(message);
            }


            if (message.Actions.HasFlag(FilterAction.Apply) && !filter.Applied)
            {
                filter.Applied = true;
                try
                {
                    ViewSource.Filter += new System.Windows.Data.FilterEventHandler(filter.Filter);
                    OnPropertyChanged(nameof(CanFilterByDate));
                    OnPropertyChanged(nameof(CanFilterByTag));
                    UpdateFilterUI(false,false,true);
                }
                catch (Exception ex)
                {
                    filter.Applied = false;
                    Debug.WriteLine($"Unable to apply a keyword filter, {ex.Message}");
                    Debug.Assert(false);
                }

            }
            else if (message.Actions.HasFlag(FilterAction.Remove) && filter.Applied)
            {
                filter.Applied = false;
                try
                {
                    ViewSource.Filter -= new System.Windows.Data.FilterEventHandler(filter.Filter);
                    OnPropertyChanged(nameof(CanFilterByDate));
                    OnPropertyChanged(nameof(CanFilterByTag));
                    UpdateFilterUI(false);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"unable to remove the keyword filter, {ex.Message}");
                    Debug.Assert(false);
                    //throw;
                }
                finally { filter.Applied = false; }
            }
            try
            {
                ViewSource.View.Refresh();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"unable to refresh the data view, {ex.Message}");
                Debug.Assert( false );
                //throw;
            }
        }

        //throw new NotImplementedException();
    }

    //protected override void PrepareForDataOperations()
    //{
    //    //throw new NotImplementedException();
    //}
}
