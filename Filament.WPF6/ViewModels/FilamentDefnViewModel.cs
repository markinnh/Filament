using Filament.WPF6.Helpers;
using Filament_Db.DataContext;
using Filament_Db.Models;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Filament.WPF6.ViewModels
{
    public class FilamentDefnViewModel : BaseBrowserViewModel<FilamentDefn,FilamentDefn>
    {
        protected override void DerivedInitItems()
        {
            PrepareForDataOperations();
            if (Filament_Db.DataContext.FilamentContext.GetAllFilaments() is IEnumerable<FilamentDefn> filaments)
            {
                InitItems(filaments);
            }
            FinishedDataOperations();
        }

        protected override void FinishedDataOperations() => FilamentDefn.InDataOps = false;

        protected override void PrepareForDataOperations() => FilamentDefn.InDataOps = true;

        protected override IEnumerable<FilamentDefn>? GetAllItems()=>FilamentContext.GetAllFilaments();
        protected override IEnumerable<FilamentDefn>? GetInUseItems() => FilamentContext.GetFilaments(f => !f.StopUsing);

        protected override IEnumerable<FilamentDefn>? GetFilteredItems(Func<FilamentDefn, bool> predicate)=>FilamentContext.GetFilaments(predicate);
        //protected override void ShowAllItems()
        //{
        //    if (FilamentContext.GetAllFilaments() is IEnumerable<FilamentDefn> filaments)
        //    {
        //        InitItems(filaments);
        //    }
        //}
        protected override void PostItemsInitialization()
        {
            if(Items != null)
            foreach (var filament in Items)
                filament.InitNotificationHandler();
        }
        protected override void PreItemsInitialization()
        {
            if (Items != null)
                foreach (var filament in Items)
                    filament.ReleaseNotificationHandler();
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
                FilamentContext.UpdateSpec(SelectedItem);
                SelectedItem.IsModified = false;
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
            if (SelectedItem?.DensityAlias?.MeasuredDensity.Count == 0 && SelectedItem.MaterialType == Filament_Db.MaterialType.PLA)
            {
                Random random = new Random();
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
