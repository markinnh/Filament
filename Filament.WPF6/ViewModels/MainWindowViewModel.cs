using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using DataDefinitions;
using DataDefinitions.Filters;
using DataDefinitions.Interfaces;
using Filament.WPF6.Helpers;

namespace Filament.WPF6.ViewModels
{
    public enum InventoryDisplayStyle { Intuitive, Simple, Form }
    public class MainWindowViewModel : Observable
    {
        private static readonly IEnumerable<string> emptyKeywords = new string[] { ".*" };
        //[Flags]
        //internal enum FiltersSupported : short
        //{
        //    TagFilter = 0x100,
        //    KeywordFilter = 0x200
        //}
        private Guid _tagId;
        //private IEnumerable<WordWithOccuranceCount> _tagStatsSnapshot;
        private IEnumerable<string> _filterTags;
        private IEnumerable<string> _keywords;

        // TODO: Add attribution to FlatIcon.com for all the special icons used in the UX
        // TODO: Add support for a third inventory page type more of a list view and a form for creating new measurements.
        #region Settings Exposed to UI
        //private bool useSwissArmyKnifeUI;
        public bool UseSwissArmyKnifeUI
        {
            get => Enum.Parse<InventoryDisplayStyle>(Properties.Settings.Default.CurrentInventoryDisplayStyle) == InventoryDisplayStyle.Intuitive;
            //set
            //{
            //    if(Properties.Settings.Default.UseSwissArmyKnifeUI != value)
            //    {
            //        Properties.Settings.Default.UseSwissArmyKnifeUI = value;
            //        Properties.Settings.Default.Save();
            //        OnPropertyChanged(nameof(UseSwissArmyKnifeUI));
            //    }
            //}
        }
        public bool UseSimpleUI => Enum.Parse<InventoryDisplayStyle>(Properties.Settings.Default.CurrentInventoryDisplayStyle) == InventoryDisplayStyle.Simple;
        public bool UseFormUI => Enum.Parse<InventoryDisplayStyle>(Properties.Settings.Default.CurrentInventoryDisplayStyle) == InventoryDisplayStyle.Form;
        private bool hasEdits;

        public bool HasEdits
        {
            get => hasEdits;
            set => Set(ref hasEdits, value);
        }

        public int SelectedTabIndex
        {
            get => Properties.Settings.Default.SelectedTabIndex;
            set => Properties.Settings.Default.SelectedTabIndex = value;
        }
        #endregion
        public MainWindowViewModel()
        {
            CheckableTagStats = new ObservableCollection<CheckableWordStat>();
            WeakReferenceMessenger.Default.Register<NotifyContainerEventArgs>(this, HandleNotifyContainer);
            EndFilterStartDate = DateTime.Today.AddDays(1);
            FilterEndDate = EndFilterStartDate;
        }
        private ObservableCollection<CheckableWordStat> checkableTagStats;

        public ObservableCollection<CheckableWordStat> CheckableTagStats
        {
            get => checkableTagStats;
            set => Set(ref checkableTagStats, value);
        }

        private ObservableCollection<CheckableWordStat> checkableKeywords;

        public ObservableCollection<CheckableWordStat> CheckableKeywords
        {
            get => checkableKeywords;
            set => Set<ObservableCollection<CheckableWordStat>>(ref checkableKeywords, value);
        }

        private bool tagFilterApplied;

        public bool TagFilterApplied
        {
            get => tagFilterApplied;
            set => Set<bool>(ref tagFilterApplied, value);
        }
        private bool keywordFilterApplied;

        public bool KeywordFilterApplied
        {
            get => keywordFilterApplied;
            set => Set<bool>(ref keywordFilterApplied, value);
        }
        private bool dateFilterApplied;

        public bool DateFilterApplied
        {
            get => dateFilterApplied;
            set => Set<bool>(ref dateFilterApplied, value);
        }

        private bool tagMenuChecked = true;

        public bool TagMenuChecked
        {
            get => tagMenuChecked;
            set
            {
                Set<bool>(ref tagMenuChecked, value);
                Set(ref keywordMenuChecked, !tagMenuChecked, propertyName: nameof(KeywordMenuChecked));
            }
        }
        private bool keywordMenuChecked;

        public bool KeywordMenuChecked
        {
            get => keywordMenuChecked;
            set
            {
                Set<bool>(ref keywordMenuChecked, value);
                Set(ref tagMenuChecked, !keywordMenuChecked, propertyName: nameof(TagMenuChecked));
            }
        }
        private WhichCompare whichCompare;

        public WhichCompare WhichCompare
        {
            get => whichCompare;
            set
            {
                if (Set<WhichCompare>(ref whichCompare, value) && filterDateTime != default && whichCompare != default)
                    WeakReferenceMessenger.Default.Send(
                        new DateTimeFilterChangedEventArgs(
                            FilterAction.Update, IResolveFilter.Filters.Date, 
                            new DateTimeResolve(whichCompare, filterDateTime,filterEndDate))
                        );
            }
        }

        private DateTime filterDateTime = DateTime.Today;

        public DateTime FilterDateTime
        {
            get => filterDateTime;
            set
            {
                if (Set<DateTime>(ref filterDateTime, value) && filterDateTime != default && whichCompare != default)
                {
                    WeakReferenceMessenger.Default.Send(
                        new DateTimeFilterChangedEventArgs(
                            FilterAction.Update, IResolveFilter.Filters.Date, new DateTimeResolve(whichCompare, filterDateTime,filterEndDate)));
                    EndFilterStartDate = filterDateTime.AddDays(1);
                }
            }
        }
        private DateTime endFilterStartDate;

        public DateTime EndFilterStartDate
        {
            get => endFilterStartDate;
            set
            {
                if(Set<DateTime>(ref endFilterStartDate, value))
                {
                    WeakReferenceMessenger.Default.Send(
                        new DateTimeFilterChangedEventArgs(FilterAction.Update, IResolveFilter.Filters.Date,
                        new DateTimeResolve(whichCompare, filterDateTime, filterEndDate))
                        );
                }
            }
        }

        private DateTime filterEndDate;

        public DateTime FilterEndDate
        {
            get => filterEndDate;
            set => Set<DateTime>(ref filterEndDate, value);
        }

        private string keywords;

        public string Keywords
        {
            get => keywords;
            set
            {
                if (Set<string>(ref keywords, value))
                {
                    WeakReferenceMessenger.Default.Send(new KeywordFilterChangedEventArgs(FilterAction.Update, IResolveFilter.Filters.Keyword,
                        keywords?.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                        ?? emptyKeywords));
                }
            }
        }
        private bool tagFilterSupported;

        public bool TagFilterSupported
        {
            get => tagFilterSupported;
            set => Set<bool>(ref tagFilterSupported, value);
        }

        private bool keywordFilterSupported;

        public bool KeywordFilterSupported
        {
            get => keywordFilterSupported;
            set => Set<bool>(ref keywordFilterSupported, value);
        }
        private bool dateFilterSupported;

        public bool DateFilterSupported
        {
            get => dateFilterSupported;
            set => Set<bool>(ref dateFilterSupported, value);
        }

        //public MainWindowViewModel()
        //{
        //    EndFilterStartDate = DateTime.Today + new TimeSpan(1, 0, 0, 0);
        //}
        //private FiltersSupported filtersSupported;
        internal void UpdateUI(DataDefinitions.Interfaces.IFilterStatus filterStatus)
        {
            TagFilterApplied = filterStatus.IsFilterApplied(IResolveFilter.Filters.Tag);
            KeywordFilterApplied = filterStatus.IsFilterApplied(IResolveFilter.Filters.Keyword);
            DateFilterApplied = filterStatus.IsFilterApplied(IResolveFilter.Filters.Date);

            TagFilterSupported = filterStatus.IsFilterSupported(IResolveFilter.Filters.Tag);
            KeywordFilterSupported = filterStatus.IsFilterSupported(IResolveFilter.Filters.Keyword);
            DateFilterSupported = filterStatus.IsFilterSupported(IResolveFilter.Filters.Date);
        }
        internal void SetTagList(ITagCollate tagCollate)
        {
            if (tagCollate != null)
            {
                _tagId = tagCollate.Signature;
                //_tagStatsSnapshot = tagCollate.DistinctTagStats.ToArray();
                CheckableTagStats.Clear();
                foreach (var ele in from t in tagCollate.DistinctTagStats select new CheckableWordStat(false, t))
                    CheckableTagStats.Add(ele);
            }
            else
                CheckableTagStats.Clear();
        }
        internal void SetKeywordList(NoteViewModel noteViewModel)
        {
            // get the keywords for the passed viewmodel
            var keywords = noteViewModel.GetWords(); //Singleton<WordCollect>.Instance.OrganizeKeywords(noteViewModel.ViewSource.View);
            CheckableKeywords = new ObservableCollection<CheckableWordStat>(from kw in keywords select new CheckableWordStat(false, kw));
        }
        /// <summary>
        /// Does not account for a tag count changing as currently written
        /// </summary>
        /// <param name="tagStats">updated tag list</param>
        internal void UpdateTagList(ITagCollate tagCollate)
        {
            if (_tagId != tagCollate.Signature) return;
            // update tagstats where a new count is supplied (either a tag is added or removed)
            //var differByCount = DifferByCount(_tagStatsSnapshot, tagCollate.DistinctTagStats);
            //if (differByCount != null)
            //{
            //    foreach (var ts in differByCount)
            //    {
            //        if (CheckableTagStats.FirstOrDefault(cts => cts.Item.Word == ts.Word) is CheckableWordStat checkable)
            //            checkable.Item.OccuranceCount = ts.OccuranceCount;
            //    }
            //}
            // add new tags to the checklist
            //var difference = _tagStatsSnapshot.Except(tagCollate.DistinctTagStats);
            //foreach (var ele in difference)
            //    CheckableTagStats.Add(new CheckableWordStat(false, ele));
            var replaceTags = new ObservableCollection<CheckableWordStat>(
                from tag in tagCollate.DistinctTagStats
                select new CheckableWordStat(CheckableTagStats.FirstOrDefault(cws => cws.Item.Word == tag.Word)?.IsChecked ?? false, tag));
            CheckableTagStats = replaceTags;
            // update the 'snapshot' of tagstats for the view
            //_tagStatsSnapshot = tagCollate.DistinctTagStats.ToArray();
        }
        internal void UpdateKeywordList(NoteViewModel noteViewModel)
        {
            var newKeywords = noteViewModel.GetWords();
            var replaceKeywords = new ObservableCollection<CheckableWordStat>(
                from kw in newKeywords
                select new CheckableWordStat(CheckableKeywords.FirstOrDefault(cws => cws.Item.Word == kw.Word)?.IsChecked ?? false, kw));
            CheckableKeywords = replaceKeywords;
        }
        //private static IEnumerable<WordWithOccuranceCount> DifferByCount(IEnumerable<WordWithOccuranceCount> original, IEnumerable<WordWithOccuranceCount> modified)
        //{
        //    return from m in modified
        //           join t in original on m.Word equals t.Word
        //           where t.OccuranceCount != m.OccuranceCount
        //           select m;
        //    //throw new NotImplementedException();
        //}
        private void HandleNotifyContainer(object recipient, NotifyContainerEventArgs message)
        {
            System.Diagnostics.Debug.Assert(message != null, "Message object is required.");

            if (message != null)
            {
                foreach (var element in message.Elements)
                    OnPropertyChanged(element);
            }
        }
        #region Commands and Handlers
        private ICommand listChangeCommand;
        public ICommand ListChangeCommand { get => listChangeCommand ??= new RelayCommand<string?>(HandleListChange); }

        private void HandleListChange(string? obj)
        {
            if (string.IsNullOrEmpty(obj)) return;

            Debug.WriteLine($"List Changed command received {obj}");
            switch (obj)
            {
                case "Tag":
                    _filterTags = CheckableTagStats.Where(ck => ck.IsChecked).Select(ck => ck.Item.Word);
                    WeakReferenceMessenger.Default.Send(new TagFilterChangedEventArgs(FilterAction.Update, IResolveFilter.Filters.Tag, _filterTags, _tagId));
                    break;
                case "Keyword":
                    _keywords = CheckableKeywords.Where(ck => ck.IsChecked).Select(ck => ck.Item.Word);
                    WeakReferenceMessenger.Default.Send(new KeywordFilterChangedEventArgs(FilterAction.Update, IResolveFilter.Filters.Keyword, _keywords));
                    break;
            }
            UpdateLists();
            //throw new NotImplementedException();
        }

        private void UpdateLists()
        {

            WeakReferenceMessenger.Default.Send(new TagInteractionNotification(ContentInteraction.ContentUpdated));
            WeakReferenceMessenger.Default.Send(new KeywordInteractionNotification(ContentInteraction.ContentUpdated));

        }

        private ICommand applyFilterCommand;
        public ICommand ApplyTagFilterCommand { get => applyFilterCommand ??= new RelayCommand<bool>(HandleApplyFilter); }

        private void HandleApplyFilter(bool obj)
        {
            Debug.WriteLine($"Received ApplyTagFilter command, state = {obj}");
            if (obj)
            {
                WeakReferenceMessenger.Default.Send(new TagFilterChangedEventArgs(FilterAction.Apply | FilterAction.Update, IResolveFilter.Filters.Tag, _filterTags, _tagId));
            }
            else
                WeakReferenceMessenger.Default.Send(new TagFilterChangedEventArgs(FilterAction.Remove, IResolveFilter.Filters.Tag, _filterTags, _tagId));
            //throw new NotImplementedException();
        }

        private ICommand applyKeywordFilterCommand;
        public ICommand ApplyKeywordFilterCommand { get => applyKeywordFilterCommand ??= new RelayCommand<bool>(HandleKeywordFilter); }

        private void HandleKeywordFilter(bool obj)
        {
            Debug.WriteLine($"Received ApplyKeywordFilter command, state = {obj}");

            if (obj)
            {
                WeakReferenceMessenger.Default.Send(new KeywordFilterChangedEventArgs(FilterAction.Apply | FilterAction.Update, IResolveFilter.Filters.Keyword,
                    Keywords?.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                    ?? emptyKeywords));
            }
            else
                WeakReferenceMessenger.Default.Send(new KeywordFilterChangedEventArgs(FilterAction.Remove, IResolveFilter.Filters.Keyword,
                    Keywords?.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                    ?? emptyKeywords));
            //throw new NotImplementedException();
        }
        private ICommand toggleFilterCommand;
        public ICommand ToggleFilterCommand { get => toggleFilterCommand ??= new RelayCommand<object>(HandleToggleFilter); }

        private void HandleToggleFilter(object? obj)
        {
            if (obj is Tuple<object, object> tpl && tpl.Item1 is bool bln && tpl.Item2 is string str)
            {
                if (Enum.TryParse<IResolveFilter.Filters>(str, out var filter))
                {
                    WeakReferenceMessenger.Default.Send(new FilterChangedEventArgs(bln ? FilterAction.Apply : FilterAction.Remove, filter));
                    switch (filter)
                    {
                        case IResolveFilter.Filters.Tag:
                            TagFilterApplied = bln;
                            break;
                        case IResolveFilter.Filters.Keyword:
                            KeywordFilterApplied = bln;
                            break;
                        case IResolveFilter.Filters.Date:
                            DateFilterApplied = bln;
                            break;
                        default:
                            break;
                    }
                    UpdateLists();
                }
            }
            //throw new NotImplementedException();
        }
        #endregion
    }
}
