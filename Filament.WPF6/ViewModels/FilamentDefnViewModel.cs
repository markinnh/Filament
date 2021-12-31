using Filament.WPF6.Helpers;
using DataDefinitions;

using DataDefinitions.Models;
using DataContext;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Filament.WPF6.ViewModels
{
    public class FilamentDefnViewModel : BaseBrowserViewModel<FilamentDefn, FilamentDefn>
    {
        protected override void DerivedInitItems()
        {
            PrepareForDataOperations();
            if (Singleton<DataLayer>.Instance.FilamentList is IEnumerable<FilamentDefn> filaments)
            {
                InitItems(filaments);
            }
            FinishedDataOperations();
        }

        protected override void FinishedDataOperations() => FilamentDefn.InDataOps = false;

        protected override void PrepareForDataOperations() => FilamentDefn.InDataOps = true;

        protected override IEnumerable<FilamentDefn>? GetAllItems() => Singleton<DataLayer>.Instance.FilamentList;
        protected override IEnumerable<FilamentDefn>? GetInUseItems() => Singleton<DataLayer>.Instance.GetFilteredFilaments(f => !f.StopUsing);

        protected override IEnumerable<FilamentDefn>? GetFilteredItems(Func<FilamentDefn, bool> predicate) =>
            Singleton<DataLayer>.Instance.GetFilteredFilaments(predicate);
        //protected override void ShowAllItems()
        //{
        //    if (FilamentContext.GetAllFilaments() is IEnumerable<FilamentDefn> filaments)
        //    {
        //        InitItems(filaments);
        //    }
        //}
        protected override void PostItemsInitialization()
        {
            if (Items != null)
                SelectedItem = Items.First();
        }

        //protected void InitItems(IEnumerable<FilamentDefn> filaments)
        //{
        //    if (Items != null)
        //    {
        //        RemoveEventLinks();
        //        Items?.Clear();
        //        foreach (var filament in filaments)
        //        {
        //            Items?.Add(filament);
        //            filament?.InitNotificationHandler();
        //        }
        //    }
        //    else
        //    {
        //        Items = new System.Collections.ObjectModel.ObservableCollection<FilamentDefn>(filaments);
        //        foreach (var filament in filaments)
        //            filament.InitNotificationHandler();
        //    }
        //    UpdateEventLinks();
        //    SelectedItem = Items.First();
        //}
        //protected override void ShowInUseItems()
        //{
        //    if (FilamentContext.GetFilaments(f => !f.StopUsing) is IEnumerable<FilamentDefn> filaments)
        //    {
        //        InitItems(filaments);
        //    }

        //}

        protected override void UpdateSelectedItemHander()
        {
            if (SelectedItem != null)
            {
                //PrepareForDataOperations();
                SelectedItem.UpdateItem<FilamentContext>();
                //FilamentContext.UpdateSpec(SelectedItem);
                SelectedItem.SetContainedModifiedState(false);
                //FinishedDataOperations();
            }
        }
#if DEBUG
        public bool IsDebug => true;
        private ICommand? prepopulate;

        public ICommand? Prepopulate
        {
            get => prepopulate ??= new RelayCommand(HandlePrepopulateCommand);
        }

        private void HandlePrepopulateCommand()
        {
            if (SelectedItem?.DensityAlias?.MeasuredDensity.Count == 0 && SelectedItem.MaterialType == MaterialType.PLA)
            {
                Random random = Singleton<Random>.Instance;
                const int minRandom = 990;
                const int maxRandom = 1030;
                SelectedItem?.DensityAlias?.MeasuredDensity.Add(new MeasuredDensity(2.98, random.Next(minRandom, maxRandom)));
                SelectedItem?.DensityAlias?.MeasuredDensity.Add(new MeasuredDensity(2.99, random.Next(minRandom, maxRandom)));
                SelectedItem?.DensityAlias?.MeasuredDensity.Add(new MeasuredDensity(3, random.Next(minRandom, maxRandom)));
            }
        }
#else
        public bool IsDebug=> false;
#endif    
    }
}
