using System;
using static System.Diagnostics.Trace;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using DataDefinitions.Models;
using Filament.WPF6.Helpers;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Mvvm.Messaging;

namespace Filament.WPF6.ViewModels
{
    internal class DefinePrintSettingsViewModel : BaseBrowserViewModel<PrintSettingDefn, PrintSettingDefn>
    {
        protected override bool SupportsFiltering { get => false; }
        protected override void DerivedInitItems()
        {
            if (Singleton<DAL.DataLayer>.Instance.PrintSettingsList is IEnumerable<PrintSettingDefn> printSettingsList)
            {
                InitItems(printSettingsList);

                //if(Items==null)
                //    Items = new System.Collections.ObjectModel.ObservableCollection<PrintSettingDefn>(printSettingsList);
                //else
                //{
                //    Items.Clear();
                //    foreach(var item in printSettingsList)
                //        Items.Add(item);
                //}
            }
            //throw new NotImplementedException();
        }

        protected override void FinishedDataOperations()
        {
            //throw new NotImplementedException();
        }

        protected override IEnumerable<PrintSettingDefn>? GetAllItems() => Singleton<DAL.DataLayer>.Instance.PrintSettingsList;


        protected override IEnumerable<PrintSettingDefn>? GetFilteredItems(Func<PrintSettingDefn, bool> predicate)
        {
            throw new NotImplementedException();
        }

        protected override IEnumerable<PrintSettingDefn>? GetInUseItems()
        {
            throw new NotImplementedException();
        }

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
                var selItem=SelectedItem ;
                if (SelectedItem.InDatabase)
                    WeakReferenceMessenger.Default.Send(new ItemRemoved<PrintSettingDefn>(selItem));
                Singleton<DAL.DataLayer>.Instance.Remove(SelectedItem);
                Items?.Remove(SelectedItem);
                DAL.Abstraction.Remove(selItem);
                
            }
            //throw new NotImplementedException();
        }

        private ICommand saveChanges;
        public ICommand SaveChanges { get => saveChanges ??= new RelayCommand(HandleSaveChanges); }

        private void HandleSaveChanges()
        {
            WriteLine($"HandleSaveChanges called, items modified : {Items?.Count(it => it.InDatabase && it.IsModified)}, items added : {Items?.Count(it => !it.InDatabase && it.IsValid)}");
            if (Items != null)
                foreach (var item in Items.Where(it => it.IsModified)) { 
                    bool needsAdd=!item.InDatabase;
                    DAL.Abstraction.UpdateItem(item);
                    if (needsAdd)
                    {
                        Singleton<DAL.DataLayer>.Instance.Add(item);
                        WeakReferenceMessenger.Default.Send(new ItemAdded<PrintSettingDefn>(item));
                    }
                }
            //throw new NotImplementedException();
        }
    }
}
