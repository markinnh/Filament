using DataDefinitions;
using DataDefinitions.Interfaces;
using DataDefinitions.Models;
using Filament.WPF6.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Filament.WPF6.ViewModels
{
    public abstract class BaseTagFilterViewModel<TBrowse, TSelect> : BaseBrowserViewModel<TBrowse, TSelect>, ITagCollate where TBrowse : DataDefinitions.DataObject, new()
        where TSelect : DataDefinitions.DataObject, new()
    {
        //private IEnumerable<string>? _tags;
        //private bool _filterApplied;
        public Guid Signature { get; set; }
        public IEnumerable<TagStat>? DistinctTagStats { get; set; }

        public BaseTagFilterViewModel()
        {
            InitFilterViewModel();
            //ViewSource.Filter += ViewSource_Filter;
        }

        //private void ViewSource_Filter1(object sender, System.Windows.Data.FilterEventArgs e)
        //{
        //    throw new NotImplementedException();
        //}

        protected abstract void InitFilterViewModel();
        protected void HandleTagFilterMessages(object recipient, TagFilterChangedEventArgs message)
        {
            if (filters.TryGetValue(tagFilterKey, out var filter))
            {
                if (filter is IKeywordFilter kwFilter)
                {
                    if (message.TagGuid != kwFilter.Signature) return;

                    kwFilter.Keywords = message.SelectedTags;

                    if (message.FilterState == FilterState.FilterApplied && !kwFilter.Applied)
                    {
                        ViewSource.Filter += kwFilter.Filter;
                        kwFilter.Applied = true;
                    }
                    else if (message.FilterState == FilterState.FilterRemoved && kwFilter.Applied)
                    {
                        ViewSource.Filter -= kwFilter.Filter;
                        kwFilter.Applied = false;
                    }
                }
            }
            ViewSource.View.Refresh();

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
