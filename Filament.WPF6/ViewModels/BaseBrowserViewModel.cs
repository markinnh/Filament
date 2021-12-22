using Filament.WPF6.Helpers;
using Filament_Db;
using Filament_Db.DataContext;
using Filament_Db.Models;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Mvvm.Messaging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Filament.WPF6.ViewModels
{
    public abstract class BaseBrowserViewModel<TBrowse, TSelect> : Observable where TBrowse : DatabaseObject, new()
        where TSelect : DatabaseObject, new()
    {
        private ObservableCollection<TBrowse> _browse;
        public ObservableCollection<TBrowse>? Items { get => _browse; set => Set(ref _browse, value); }
        private TSelect? selectedItem;

        public TSelect? SelectedItem
        {
            get => selectedItem;
            set => Set<TSelect?>(ref selectedItem, value);
        }

        public bool HasModifiedItems => Items?.Count(it => it.IsModified) > 0 && !InAddNew;
        public bool CanAdd => !InAddNew;
        protected BaseBrowserViewModel()
        {
            if (Setting.GetSetting(nameof(MainWindow.SelectShowFlag)) is Setting setting)
            {
                if (Enum.Parse<ShowAllFlag>(setting.Value) is ShowAllFlag flag)
                {
                    if (flag == ShowAllFlag.ShowAll)
                        ShowAllItems();
                    else
                        ShowInUseItems();
                }
            }
            else
            {
                DerivedInitItems();
                UpdateEventLinks();
            }
            WeakReferenceMessenger.Default.Register<ShowAllFlagChanged>(this, HandleShowAllFlagChanged);
        }
        ~BaseBrowserViewModel()
        {
            WeakReferenceMessenger.Default.Unregister<ShowAllFlagChanged>(this);
        }
        protected virtual void UpdateEventLinks()
        {
            if (Items != null)
            {
                foreach (var item in Items)
                    if (item is Observable observable)
                        observable.PropertyChanged += Def_PropertyChanged;

                Items.CollectionChanged += Items_CollectionChanged;
            }
        }
        protected virtual void RemoveEventLinks()
        {
            if (Items != null)
            {
                foreach (var item in Items)
                    if (item is Observable observable)
                        observable.PropertyChanged -= Def_PropertyChanged;

                Items.CollectionChanged -= Items_CollectionChanged;
            }
        }
        protected virtual void ShowAllItems()
        {
            PrepareForDataOperations();
#pragma warning disable CS8604 // Possible null reference argument.
            InitItems(GetAllItems());
#pragma warning restore CS8604 // Possible null reference argument.
            FinishedDataOperations();
            PostItemsInitialization();
        }
        protected virtual void ShowInUseItems()
        {
            PrepareForDataOperations();
#pragma warning disable CS8604 // Possible null reference argument.
            InitItems(GetInUseItems());
#pragma warning restore CS8604 // Possible null reference argument.
            FinishedDataOperations();
            PostItemsInitialization();

        }
        protected virtual void HandleShowAllFlagChanged(object recipient, ShowAllFlagChanged message)
        {
            if (message.Value == ShowAllFlag.ShowAll)
                ShowAllItems();
            else
                ShowInUseItems();

            //SelectedItem = Items?.FirstOrDefault();
            System.Diagnostics.Debug.WriteLine($"Show all flag changed to : {message.Value} in {this.GetType().Name}");
        }

        protected bool InAddNew { get; set; }
        private void Items_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add && e.NewItems != null)
                LinkRange(e.NewItems);
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove && e.OldItems != null)
                UnlinkRange(e.OldItems);
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Reset)
            {
                if (e.OldItems != null)
                    UnlinkRange(e.OldItems);
                if (e.NewItems != null)
                    LinkRange(e.NewItems);
            }
        }
        protected abstract IEnumerable<TBrowse>? GetInUseItems();
        protected abstract IEnumerable<TBrowse>? GetAllItems();
        protected abstract IEnumerable<TBrowse>? GetFilteredItems(Func<TBrowse, bool> predicate);
        protected void InitItems(IEnumerable<TBrowse> newItems)
        {
            if (newItems != null)
            {
                PreItemsInitialization();
                if (Items != null)
                {
                    RemoveEventLinks();
                    Items?.Clear();
                    foreach (var filament in newItems)
                    {
                        Items?.Add(filament);
                        //filament?.InitNotificationHandler();
                    }
                }
                else
                {
                    Items = new System.Collections.ObjectModel.ObservableCollection<TBrowse>(newItems);
                    //foreach (var filament in filaments)
                    //    filament.InitNotificationHandler();
                }
                UpdateEventLinks();
                //SelectedItem = Items.First();
                PostItemsInitialization();
            }
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
                if (Items?.Where(it => it.IsModified).ToList() is IEnumerable<TBrowse> tbrowse)
                {
                    foreach (TBrowse tb in tbrowse)
                        tb.UpdateItem();

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
            //try
            //{
            //    if (selectedItem != null)
            //    {
            //        PrepareForDataOperations();
            //        if (!InAddNew)
            //        {

            //            if (FilamentContext.UpdateL(selectedItem) == 1) { 
            //                selectedItem.IsModified = false;
            //                //Items?.Add(selectedItem);
            //            }
            //        }
            //        else
            //        {
            //            if (FilamentContext.AddAll(1,selectedItem) == 1)
            //            {
            //                selectedItem.IsModified = false;
            //                Items?.Add(selectedItem);
            //            }
            //        }
            //        FinishedDataOperations();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    System.Diagnostics.Debug.WriteLine($"Unable to complete data operations.  The following error was returned.  {ex.Message}, Inner Exception : {ex.InnerException?.Message}");
            //}
            System.Diagnostics.Debug.WriteLine($"Update selected item for {typeof(TBrowse).Name} not implemented.");
        }
        protected virtual void DeleteSelectedHandler()
        {
            if (SelectedItem != null)
            {
                if (SelectedItem.InDatabase)
                    FilamentContext.DeleteItems(SelectedItem);
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
    }
}
