using System;
using static System.Diagnostics.Trace;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using DataDefinitions.Models;
using Filament.WPF6.Helpers;
using DataDefinitions.JsonSupport;
using System.Collections.ObjectModel;
using DataDefinitions.Interfaces;
using DataDefinitions.LiteDBSupport;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using DataDefinitions;

namespace Filament.WPF6.ViewModels
{
    internal class DefinePrintSettingsViewModel : BaseBrowserViewModel<PrintSettingDefn, PrintSettingDefn>
    {
        protected override bool SupportsShowAllFiltering { get => false; }
        protected override void DerivedInitItems()
        {
            ViewSource.Source = Singleton<LiteDBDal>.Instance.PrintSettings;
            //if (Singleton<JsonDAL>.Instance.Document.PrintSettingsDefn is ObservableCollection<PrintSettingDefn> printSettingsList)
            //{
            //    Items=printSettingsList; 
            //    //InitItems(printSettingsList);

            //    //if(Items==null)
            //    //    Items = new System.Collections.ObjectModel.ObservableCollection<PrintSettingDefn>(printSettingsList);
            //    //else
            //    //{
            //    //    Items.Clear();
            //    //    foreach(var item in printSettingsList)
            //    //        Items.Add(item);
            //    //}
            //}
            //throw new NotImplementedException();
        }

        //protected override IEnumerable<PrintSettingDefn>? GetAllItems() => throw new NotImplementedException();


        //protected override IEnumerable<PrintSettingDefn>? GetFilteredItems(Func<PrintSettingDefn, bool> predicate)
        //{
        //    throw new NotImplementedException();
        //}

        //protected override IEnumerable<PrintSettingDefn>? GetInUseItems()
        //{
        //    throw new NotImplementedException();
        //}

        protected override void PrepareForDataOperations()
        {
            //throw new NotImplementedException();
        }

        private DataDefinitions.DatabaseObject? selectedRow;

        public DataDefinitions.DatabaseObject SelectedRow
        {
            get => selectedRow;
            set => Set<DataDefinitions.DatabaseObject>(ref selectedRow, value);
        }

        private ICommand? deleteRowCommand;
        public ICommand DeleteRowCommand => deleteRowCommand ??= new RelayCommand(HandleDeleteRow);

        private void HandleDeleteRow()
        {
            if (SelectedItem != null)
            {
                var selItem = SelectedItem;
                if (SelectedItem.InDatabase)
                    WeakReferenceMessenger.Default.Send(new ItemRemoved<PrintSettingDefn>(selItem));
                if (SelectedItem is ITrackUsable track)
                    track.StopUsing = true;
                //Singleton<DAL.DataLayer>.Instance.Remove(SelectedItem);
                //Items?.Remove(SelectedItem);
                //DAL.Abstraction.Remove(selItem);
                selItem = null;
            }
            //throw new NotImplementedException();
        }

        private ICommand? saveChanges;
        public ICommand SaveChanges { get => saveChanges ??= new RelayCommand(HandleSaveChanges); }

        private void HandleSaveChanges()
        {
            if (ViewSource.Source is IEnumerable<PrintSettingDefn> items && items != null)
            {
                WriteLine($"HandleSaveChanges called, items modified : {items.Count(it => it.InDatabase && it.IsModified)}, items added : {items.Count(it => !it.InDatabase && it.IsValid)}");
                foreach (var item in items.Where(it => it.IsModified))
                {
                    bool needsAdd = !item.InDatabase;
                    item.UpdateItem(Singleton<LiteDBDal>.Instance);
                    if (needsAdd)
                    {
                        //Singleton<DAL.DataLayer>.Instance.Add(item);
                        WeakReferenceMessenger.Default.Send(new ItemAdded<PrintSettingDefn>(item));
                    }
                }
                //throw new NotImplementedException();
            }
        }

        protected override void FinishedDataOperations()
        {
            //throw new NotImplementedException();
        }
    }
}
