using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using DataContext;
using DataDefinitions.Models;
using Microsoft.EntityFrameworkCore;
using MyLibraryStandard.Attributes;
using Microsoft.Toolkit.Mvvm.Input;
using Filament.WPF6.Helpers;
using DataDefinitions;

namespace Filament.WPF6.ViewModels
{
    public class FilamentDefnPageViewModel : DataDefinitions.Observable
    {
        public ObservableCollection<FilamentDefn>? Items { get; set; }
        private FilamentDefn? selectedItem;
        //[Affected(Names = new[] { nameof(DefinedDensityShown), nameof(MeasuredDensityShown) })]

        public FilamentDefn? SelectedItem
        {
            get => selectedItem;
            set
            {
                Set<FilamentDefn>(ref selectedItem, value);
            }
        }
        //[Affected(Names = new[] { nameof(MeasuredDensityShown), nameof(DefinedDensityShown) })]
        //        public DensityType DensityType
        //        {
        //            get => selectedItem?.DensityAlias?.DensityType ?? DensityType.Defined;
        //            set
        //            {
        //                if (selectedItem != null)
        //                {
        //                    if (selectedItem?.DensityAlias?.DensityType != value)
        //                    {
        //#pragma warning disable CS8602 // Dereference of a possibly null reference.
        //                        selectedItem.DensityAlias.DensityType = value;
        //#pragma warning restore CS8602 // Dereference of a possibly null reference.

        //                        // allows passing the current diameter for the filament to the MeasuredDensity Initializer member
        //                        // can be used to pass other initializers dynamically
        //                        if (selectedItem.DensityAlias.DensityType == DensityType.Measured)
        //                        {
        //                            dynamic dyn = new ExpandoObject();
        //                            dyn.Diameter = selectedItem.Diameter;
        //                            MeasuredDensity.Initializer = dyn;
        //                        }
        //                        else
        //                            MeasuredDensity.Initializer = null;

        //                        UpdateAffected(nameof(DensityType));
        //                    }
        //                }
        //            }
        //        }
        //public bool DefinedDensityShown => SelectedItem?.DensityAlias?.DensityType == DensityType.Defined;
        //public bool MeasuredDensityShown => SelectedItem?.DensityAlias?.DensityType == DensityType.Measured;

#if DEBUG
        public bool IsDebug => true;
        private ICommand? prepopulate;

        public ICommand? Prepopulate
        {
            get => prepopulate ??= new RelayCommand(HandlePrepopulateCommand);
        }

        private void HandlePrepopulateCommand()
        {
            if (selectedItem?.DensityAlias?.MeasuredDensity.Count == 0 && selectedItem.MaterialType == MaterialType.PLA)
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

        public FilamentDefnPageViewModel()
        {
            if (Singleton<DataLayer>.Instance.FilamentList is IEnumerable<FilamentDefn> filaments)
            {
                Items = new(filaments);
                foreach (var filament in filaments)
                {
                    filament.InitNotificationHandler();
                }
                SelectedItem = Items.First();
            }
        }
    }
}
