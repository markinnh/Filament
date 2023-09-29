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

namespace Filament.WPF6.ViewModels
{
    public abstract class BaseTagFilterViewModel<TBrowse, TSelect> : BaseBrowserViewModel<TBrowse, TSelect>, ITagCollate where TBrowse : DataDefinitions.DataObject, new()
        where TSelect : DataDefinitions.DataObject, new()
    {
        //private IEnumerable<string>? _tags;
        //private bool _filterApplied;

        public abstract Guid Signature { get;  }
        public IEnumerable<WordWithOccuranceCount>? DistinctTagStats { get=> Singleton<WordCollect>.Instance.OrganizeTags(ViewSource.View);  }

        public BaseTagFilterViewModel()
        {
            //DistinctTagStats = GetFilteredTags();
            InitFilterViewModel();
            //ViewSource.Filter += ViewSource_Filter;
        }

        //private void ViewSource_Filter1(object sender, System.Windows.Data.FilterEventArgs e)
        //{
        //    throw new NotImplementedException();
        //}
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
        protected void HandleTagFilterMessages(object recipient, TagFilterChangedEventArgs message)
        {
            if (filters.TryGetValue(IResolveFilter.Filters.Tag, out var filter) && filter.Resolve is ICriteriaFilter criteria)
            {
                if (message.TagGuid != criteria.Signature) return;

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
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine($"Unable to apply a filter in {GetType().Name}, message {ex.Message}");
                        }
                        filter.Applied = true;
                    }
                    else if (message.Actions.HasFlag(FilterAction.Remove) && filter.Applied)
                    {
                        try
                        {
                            ViewSource.Filter -= new FilterEventHandler(filter.Filter);
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine($"Unable to remove a filter in {GetType().Name}, message {ex.Message}");
                        }
                        filter.Applied = false;
                    }


                }
                catch
                {
                    Debug.WriteLine("Error during filter operations.");
                }
            }
            try
            {
                ViewSource?.View?.Refresh();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Problem refreshing the View on the {GetType().Name} view, {ex.Message}");
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
}
