
using DataDefinitions.Models;
using Filament.WPF6.Helpers;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Filament.WPF6.ViewModels
{
    internal class FormBasedViewModel : ByVendorViewModel
    {
        #region Add New Depth Measurement Logic
        private DepthMeasurement? depthMeasurement;

        public DepthMeasurement? NewDepthMeasurement
        {
            get => depthMeasurement;
            set => base.Set(ref depthMeasurement, value);
        }

        //private bool inAddNew;

        //public bool InAddNew
        //{
        //    get => inAddNew;
        //    set => Set<bool>(ref inAddNew, value);
        //}

        private ICommand? createNewMeasurement;
        public ICommand CreateNewMeasurement => createNewMeasurement ??= new RelayCommand(() =>
        {
            if (!InAddNew && SelectedItem is InventorySpool spool)
            {
                NewDepthMeasurement = new DataDefinitions.Models.DepthMeasurement();
                InAddNew = true;
            }
        });
        private ICommand? saveNewMeasurement;
        public ICommand SaveNewMeasurement => saveNewMeasurement ??= new RelayCommand(() =>
        {
            if (InAddNew && SelectedItem is InventorySpool spool && NewDepthMeasurement != null)
            {
                if (NewDepthMeasurement.IsValid)
                {
                    spool.DepthMeasurements.Add(NewDepthMeasurement);
                    DAL.Abstraction.UpdateItem(NewDepthMeasurement);
                }
            }
        });
        #endregion
    }
}
