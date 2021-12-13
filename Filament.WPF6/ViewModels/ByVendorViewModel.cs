using Filament_Db;
using Filament_Db.DataContext;
using Filament_Db.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Toolkit.Mvvm.Messaging;

namespace Filament.WPF6.ViewModels
{
    internal class ByVendorViewModel : BaseBrowserViewModel<Filament_Db.Models.VendorDefn, DatabaseObject>
    {
        private object? selectedObject;

        public object? SelectedObject
        {
            get => selectedObject;
            set => Set<object?>(ref selectedObject, value);
        }


        protected override void DerivedInitItems()
        {
            PrepareForDataOperations();
            if (Filament_Db.DataContext.FilamentContext.GetAllVendors() is IEnumerable<Filament_Db.Models.VendorDefn> filas)
            {
                Items = new System.Collections.ObjectModel.ObservableCollection<Filament_Db.Models.VendorDefn>(filas);

            }
            FinishedDataOperations();
        }
        protected override void ShowAllItems()
        {
            base.ShowAllItems();
            if (Items != null)
                WeakReferenceMessenger.Default.Send<Helpers.VendorDefnListChanged>(new Helpers.VendorDefnListChanged(Items));
        }

        protected override IEnumerable<VendorDefn>? GetInUseItems() => FilamentContext.GetSomeVendors(v => !v.StopUsing);

        protected override IEnumerable<VendorDefn>? GetAllItems() => FilamentContext.GetAllVendors();
        protected override IEnumerable<VendorDefn>? GetFilteredItems(Func<VendorDefn, bool> predicate) => FilamentContext.GetSomeVendors(predicate);

        protected override void ShowInUseItems()
        {
            base.ShowInUseItems();
            if (Items != null)
                WeakReferenceMessenger.Default.Send(new Helpers.VendorDefnListChanged(Items));
        }

        protected override void FinishedDataOperations()
        {
            Filament_Db.Models.VendorDefn.InDataOps = false;
            SpoolDefn.InDataOps = false;
        }

        protected override void PrepareForDataOperations()
        {
            Filament_Db.Models.VendorDefn.InDataOps = true;
            SpoolDefn.InDataOps = true;
        }
    }
}
