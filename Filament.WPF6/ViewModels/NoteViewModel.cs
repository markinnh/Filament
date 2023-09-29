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
using System.Threading.Tasks;

namespace Filament.WPF6.ViewModels
{
    public class NoteViewModel : BaseTagFilterViewModel<NoteDefn, NoteDefn>
    {
        //private bool _keywordFilteringSupported;
        public override Guid Signature => Singleton<LiteDBDal>.Instance.Notes.Signature;
        protected override void DerivedInitItems()
        {
            ViewSource.Source = Singleton<LiteDBDal>.Instance.Notes;
            //throw new NotImplementedException();
        }

        protected override void FinishedDataOperations()
        {
            //throw new NotImplementedException();
        }

        protected override void InitFilterViewModel()
        {
            base.InitFilterViewModel();
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
            filterSupported[(int)IResolveFilter.Filters.Date] = filters.TryAdd(IResolveFilter.Filters.Date,new WindowsFilter(new DateTimeResolve()));
            WeakReferenceMessenger.Default.Register<DateTimeFilterChangedEventArgs>(this, HandleDateFilterChanged);
        }

        private void HandleDateFilterChanged(object recipient, DateTimeFilterChangedEventArgs message)
        {
            if(message != null && filters.TryGetValue(message.Filter,out var filter))
            {
                filter.Resolve = message.Payload;
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
                    }
                    catch (Exception ex)
                    {
                        filter.Applied = false;
                        Debug.WriteLine($"Unable to apply a keyword filter, {ex.Message}");
                    }

                }
                else if (message.Actions.HasFlag(FilterAction.Remove) && filter.Applied)
                {
                    filter.Applied = false;
                    try
                    {
                        ViewSource.Filter -= new System.Windows.Data.FilterEventHandler(filter.Filter);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"unable to remove the keyword filter, {ex.Message}");
                        //throw;
                    }
                    finally { filter.Applied = false; }
                }
            }
            try
            {
                ViewSource.View.Refresh();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"unable to refresh the data view, {ex.Message}");
                //throw;
            }
            //throw new NotImplementedException();
        }

        protected override void PrepareForDataOperations()
        {
            //throw new NotImplementedException();
        }
    }
}
