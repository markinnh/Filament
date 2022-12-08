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
using DataDefinitions.Interfaces;
using Filament.WPF6.Helpers;

namespace Filament.WPF6.ViewModels
{
    public enum InventoryDisplayStyle { Intuitive, Simple, Form }
    public class MainWindowViewModel : Observable
    {
        private Guid _tagId;
        private IEnumerable<TagStat> _tagStatsSnapshot;
        private IEnumerable<string> _filterTags;
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
        public int SelectedTabIndex
        {
            get => Properties.Settings.Default.SelectedTabIndex;
            set => Properties.Settings.Default.SelectedTabIndex = value;
        }
        #endregion
        public MainWindowViewModel()
        {
            CheckableTagStats = new ObservableCollection<CheckableTagStat>();
            WeakReferenceMessenger.Default.Register<NotifyContainerEventArgs>(this, HandleNotifyContainer);
        }
        private ObservableCollection<CheckableTagStat> checkableTagStats;

        public ObservableCollection<CheckableTagStat> CheckableTagStats
        {
            get => checkableTagStats;
            set => Set(ref checkableTagStats, value);
        }

        internal void SetTagList(ITagCollate tagCollate)
        {
            if (tagCollate.DistinctTagStats != null)
            {
                _tagId = tagCollate.Signature;
                _tagStatsSnapshot = tagCollate.DistinctTagStats.ToArray();
                CheckableTagStats.Clear();
                foreach (var ele in from t in tagCollate.DistinctTagStats select new CheckableTagStat(false, t))
                    CheckableTagStats.Add(ele);
            }
            else
                CheckableTagStats.Clear();
        }
        /// <summary>
        /// Does not account for a tag count changing as currently written
        /// </summary>
        /// <param name="tagStats">updated tag list</param>
        internal void UpdateTagList(ITagCollate tagCollate)
        {
            if (_tagId != tagCollate.Signature) return;
            // update tagstats where a new count is supplied (either a tag is added or removed)
            var differByCount = DifferByCount(_tagStatsSnapshot, tagCollate.DistinctTagStats);
            if (differByCount != null)
            {
                foreach (var ts in differByCount) { 
                    if(CheckableTagStats.FirstOrDefault(cts => cts.Item.Tag == ts.Tag) is CheckableTagStat checkable)
                        checkable.Item.Count= ts.Count;
                }
            }
            // add new tags to the checklist
            var difference = _tagStatsSnapshot.Except(tagCollate.DistinctTagStats);
            foreach (var ele in difference)
                CheckableTagStats.Add(new CheckableTagStat(false, ele));
            // update the 'snapshot' of tagstats for the view
            _tagStatsSnapshot = tagCollate.DistinctTagStats.ToArray();
        }
        private static IEnumerable<TagStat> DifferByCount(IEnumerable<TagStat> original, IEnumerable<TagStat> modified)
        {
            return from m in modified
                   join t in original on m.Tag equals t.Tag
                   where t.Count != m.Count
                   select m;
            //throw new NotImplementedException();
        }
        private void HandleNotifyContainer(object recipient, NotifyContainerEventArgs message)
        {
            System.Diagnostics.Debug.Assert(message != null, "Message object is required.");

            if (message != null)
            {
                foreach (var element in message.Elements)
                    OnPropertyChanged(element);
            }
        }

        private ICommand listChangeCommand;
        public ICommand ListChangeCommand { get => listChangeCommand ??= new RelayCommand(HandleListChange); }

        private void HandleListChange()
        {
            Debug.WriteLine("List Changed command received");
            _filterTags = CheckableTagStats.Where(ck => ck.IsChecked).Select(ck => ck.Item.Tag);
            WeakReferenceMessenger.Default.Send(new TagFilterChangedEventArgs(FilterState.FilterUpdated, _filterTags,_tagId));
            //throw new NotImplementedException();
        }
        private ICommand applyFilterCommand;
        public ICommand ApplyFilterCommand { get => applyFilterCommand ??= new RelayCommand<bool>(HandleApplyFilter); }

        private void HandleApplyFilter(bool obj)
        {
            Debug.WriteLine($"Received ApplyFilter command, state = {obj}");
            if (obj)
            {
                WeakReferenceMessenger.Default.Send(new TagFilterChangedEventArgs(FilterState.FilterApplied, _filterTags,_tagId));
            }
            else
                WeakReferenceMessenger.Default.Send(new TagFilterChangedEventArgs(FilterState.FilterRemoved, _filterTags, _tagId));
            //throw new NotImplementedException();
        }
    }
}
