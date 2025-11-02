using CommunityToolkit.Mvvm.Messaging;
using DataDefinitions;
using DataDefinitions.Filters;
using DataDefinitions.Interfaces;
using DataDefinitions.LiteDBSupport;
using DataDefinitions.Models;
using Filament.WPF6.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Filament.WPF6.ViewModels;

public abstract class BaseTagFilterViewModel<TBrowse, TSelect> : BaseBrowserViewModel<TBrowse, TSelect>, IViewModelTagCollate where TBrowse : DataDefinitions.DataObject, new()
    where TSelect : DataDefinitions.DataObject, new()
{
    //private IEnumerable<string>? _tags;
    //private bool _filterApplied;

    public abstract Guid Signature
    {
        get;
    }
    
    protected bool TagFilterApplied
    {
        get
        {
            if (filters.TryGetValue(IResolveFilter.Filters.Tag, out var filter))
                return filter.Applied;
            return false;
        }
    }

    public IEnumerable<WordWithOccuranceCount>? DistinctTagStats => Singleton<WordCollect>.Instance.OrganizeTags(ViewSource.View);

    public BaseTagFilterViewModel()
    {
        //DistinctTagStats = GetFilteredTags();
        InitFilterViewModel();
        //ViewSource.Filter += ViewSource_Filter;
    }
    public override void InitFilterUI()
    {
        UpdateFilterUI(true);
    }
    //private void ViewSource_Filter1(object sender, System.Windows.Data.FilterEventArgs e)
    //{
    //    throw new NotImplementedException();
    //}
    public override void ReleaseEventHandlers()
    {
        base.ReleaseEventHandlers();
        WeakReferenceMessenger.Default.Unregister<TagFilterChangedEventArgs>(this);
    }
    protected IEnumerable<WordWithOccuranceCount>? GetFilteredTags()
    {
        ViewSource.View.Refresh();
        return Singleton<WordCollect>.Instance.OrganizeTags(ViewSource.View);
    }
    /// <summary>
    /// default initialization is for filtering tags
    /// </summary>
    protected virtual void InitFilterViewModel()
    {
        //Signature = Singleton<LiteDBDal>.Instance.Vendors.Signature;
#if DEBUG
        filterSupported[(int)IResolveFilter.Filters.Tag] = filters.TryAdd(IResolveFilter.Filters.Tag, new WindowsFilter(new TagResolve() { Signature = Signature }));
#else
        filterSupported[(int)IResolveFilter.Filters.Tag] =filters.TryAdd(IResolveFilter.Filters.Tag, new WindowsFilter( new TagResolve() { Signature = Signature }));
#endif
        WeakReferenceMessenger.Default.Register<TagFilterChangedEventArgs>(this, HandleTagFilterMessages);

    }
    /// <summary>
    /// Handle tag filter messages for this view model.
    /// </summary>
    /// <param name="recipient"></param>
    /// <param name="message"></param>
    protected void HandleTagFilterMessages(object recipient, TagFilterChangedEventArgs message)
    {
        Trace.WriteLine($"In HandleTagFilterMessages, for {GetType().Name}, message will {(message.TargetViewModel == ViewModelGuid ? "not " : string.Empty)}be ignored",(message.TargetViewModel==ViewModelGuid?"Information":"Error"));
        if (filters.TryGetValue(IResolveFilter.Filters.Tag, out var filter) &&
            filter.Resolve is ICriteriaFilter criteria &&
            message.TagGuid == criteria.Signature &&
            message.TargetViewModel == ViewModelGuid)
        {

            if (message.Actions.HasFlag(FilterAction.Update))
            {
                if (criteria.CriteriaSet)
                    criteria.UpdateCriteria(message);
                else
                    criteria.SetCriteria(message);
            }

            //iKeywwordFilter.Keywords = message.SelectedTags;

            try
            {
                if (message.Actions.HasFlag(FilterAction.Apply) && !filter.Applied)
                {
                    try
                    {
                        ViewSource.Filter += new FilterEventHandler(filter.Filter);
                        UpdateFilterUI(TagFilterEnabled: true);
                    }
                    catch (Exception ex)
                    {
                        var msg = $"Unable to apply a filter in {GetType().Name}, message {ex.Message}";
                        
                        Trace.WriteLine(msg,"Error");
                        Debug.Assert(false);
                    }
                    filter.Applied = true;
                }
                else if (message.Actions.HasFlag(FilterAction.Remove) && filter.Applied)
                {
                    try
                    {
                        ViewSource.Filter -= new FilterEventHandler(filter.Filter);
                        UpdateFilterUI(TagFilterEnabled: true);
                    }
                    catch (Exception ex)
                    {
                        Trace.WriteLine($"Unable to remove a filter in {GetType().Name}, message {ex.Message}","Error");
                        Debug.Assert(false);
                    }
                    filter.Applied = false;
                }


            }
            catch
            {
                Trace.WriteLine("Error during filter operations.", "Error");
                Debug.Assert(false);
            }
            try
            {
                ViewSource?.View?.Refresh();
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"Problem refreshing the View on the {GetType().Name} view, {ex.Message}", "Error");
                Debug.Assert(false);
            }
        }


        //var tagFilter = filters[tagFilterKey];

        //ViewSource.Filter += ViewSource_Filter;
        //tagFilter.Applied = message.FilterState == FilterState.FilterApplied;
        //throw new NotImplementedException();
    }

    //private void ViewSource_Filter(object sender, System.Windows.Data.FilterEventArgs e)
    //{
    //    bool result = true;

    //    if (filters.Values.Count > 0)
    //        foreach (var filter in filters.Values.Where(f => f.Applied && f.CriteriaSet))
    //            result &= filter.Accepted(e.Item);

    //    e.Accepted = result;

    //    //throw new NotImplementedException();

    //}

}
