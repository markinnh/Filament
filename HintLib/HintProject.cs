using HintLib.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HintLib
{
    public class HintProject : HashedObject
    {
        [JsonIgnore]
        public int HintProjectId { get; set; }
        [JsonIgnore, NotMapped]
        public override bool InDatabase => HintProjectId != default;
        [JsonIgnore, NotMapped]
        protected override bool WatchingCrc { get; } = false;
        [NotMapped,JsonIgnore]
        public override string DisplayType => "Hint Project";
        private string projectName;
        [MaxLength(100), Required]
        public string ProjectName
        {
            get => projectName;
            set => Set<string>(ref projectName, value);
        }

        [JsonIgnore, NotMapped]
        protected bool DataCollectionEventLinked { get; set; }
        [JsonIgnore, NotMapped]
        public override bool IsModified
        {
            get => base.IsModified ||
                    Hints?.Count(h => h.IsModified) > 0 ||
                    projectFilenames?.Count(pd => pd.IsModified) > 0;
            set => base.IsModified = value;
        }

        protected override bool HasDependentCollections => true;

        public ICollection<Hint> data;
        public ICollection<Hint> Hints
        {
            get => data;
            set
            {
                if (data != null)
                {
                    if (data is ObservableCollection<Hint> hints)
                    {
                        hints.CollectionChanged -= BaseObject_CollectionChanged;
                        DataCollectionEventLinked = false;
                    }
                    else
                        DataCollectionEventLinked = false;
                }

                Set(ref data, value);

                if (data != null)
                {
                    System.Diagnostics.Debug.WriteLine($"Data updated.  In data operations : {InDataOps}, IsModified {IsModified}");
                    if (data is ObservableCollection<Hint> hints)
                    {
                        hints.CollectionChanged += BaseObject_CollectionChanged;
                        DataCollectionEventLinked = true;
                    }
                    foreach (Hint h in data)
                        h.PropertyChanged += Contained_PropertyChanged;
                }
            }
        }
        [NotMapped]
        public ICommand? AddHintToProject { get; set; }
        private ICollection<ProjectFilename> projectFilenames;
        [JsonIgnore]
        public ICollection<ProjectFilename> ProjectFilenames
        {
            get => projectFilenames;
            set
            {
                if (projectFilenames != null)
                {
                    if (projectFilenames is ObservableCollection<ProjectFilename> t)
                    {
                        t.CollectionChanged -= ProjectDirectoriesChanged;
                    }
                }

                Set<ICollection<ProjectFilename>>(ref projectFilenames, value);

                if (projectFilenames != null)
                {
                    if (projectFilenames is ObservableCollection<ProjectFilename> t)
                    {
                        t.CollectionChanged += ProjectDirectoriesChanged;
                    }
                }
            }
        }

        private void ProjectDirectoriesChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (!InDataOps)
                IsModified = true;
            //throw new NotImplementedException();
        }

        public HintProject()
        {
            var data = new ObservableCollection<Hint>();
            Hints = data;

            data.CollectionChanged += BaseObject_CollectionChanged;
            DataCollectionEventLinked = true;

            var projectFilenames = new ObservableCollection<ProjectFilename>();
            ProjectFilenames = projectFilenames;
            projectFilenames.CollectionChanged += BaseObject_CollectionChanged;
        }
        ~HintProject()
        {
            if (Hints is ObservableCollection<Hint> hints)
            {
                hints.CollectionChanged -= BaseObject_CollectionChanged;
                UnwindContained(hints);
            }
            if (ProjectFilenames is ObservableCollection<ProjectFilename> projectFNames)
            {
                projectFNames.CollectionChanged -= BaseObject_CollectionChanged;
                UnwindContained(projectFNames);
            }

        }
        private void UnwindContained<TEnumerable>(IEnumerable<TEnumerable> items) where TEnumerable : BaseObject
        {
            foreach (var item in items)
                item.PropertyChanged -= Contained_PropertyChanged;
        }
        //private void ProjectFilenames_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        //{
        //    if (!InDataOps)
        //        IsModified = true;
        //    if (e.Action == NotifyCollectionChangedAction.Add && e.NewItems != null)
        //        foreach (var item in e.NewItems)
        //            if (item is BaseObject baseObject)
        //                baseObject.PropertyChanged += Contained_PropertyChanged;
        //    //throw new NotImplementedException();
        //}

        private void BaseObject_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (!InDataOps)
                IsModified = true;

            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add && e.NewItems != null)
            {
                foreach (var item in e.NewItems)
                {
                    if (item is BaseObject baseObj)
                        baseObj.PropertyChanged += Contained_PropertyChanged;
                }
            }
            //throw new NotImplementedException();
        }

        private void Contained_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(IsModified))
                OnPropertyChanged(nameof(IsModified));
            //throw new NotImplementedException();
        }
        public override void SetItemModifiedState(bool state)
        {
            IsModified = state;

            foreach (var item in Hints)
                item.IsModified = state;

            foreach (var it in ProjectFilenames)
                it.IsModified = state;
        }
        public void PrepareToStoreInDatabase()
        {
            foreach (Hint item in data)
            {
                item.SavedCrc = item.CurrentCrc;
            }
        }
        protected override void SetEntityStateOnDependentItems(HintContext context)
        {
            SetDependentState(Hints.Where(h => Modified(h)), context, EntityState.Modified);
            SetDependentState(Hints.Where(h => Added(h)), context, EntityState.Added);

            SetDependentState(ProjectFilenames.Where(h => Added(h)), context, EntityState.Added);
            SetDependentState(ProjectFilenames.Where(h => Modified(h)), context, EntityState.Modified);
        }
        protected void SetDependentState<TItems>(IEnumerable<TItems> items, HintContext context, EntityState state)
        {
            if (items != null)
            {
                foreach (var item in items)
                    if (item != null)
                        context.Entry(item).State = state;
            }
        }
    }
}
