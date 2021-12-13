using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Filament_Data.JsonModel;

namespace Filament.WPF.ViewModel
{
    public enum DensityEditType { Defined, Measured };
    [ViewModelDescriptor(Description = "Filament Definition View", Title = "Filament", Category = "Definitions")]
    public class DefineFilamentViewModel : BaseViewModel<FilamentDefn>, IDataErrorInfo
    {
        public override FilamentDefn EditItem
        {
            get => editItem;
            set
            {
                if (Set(ref editItem, value))
                {
                    EditorType = editItem.DensityUnion.V1 == default ? DensityEditType.Measured : DensityEditType.Defined;
                    EditDefinedDensity = editItem.DensityUnion.V1;
                    EditMeasuredDensity = editItem.DensityUnion.V2;
                }
            }
        }
        private DefinedDensity definedDensity;

        public DefinedDensity EditDefinedDensity
        {
            get => definedDensity;
            set
            {
                if(Set(ref definedDensity, value)) { 
                    EditItem.DensityUnion.V1 = value;
                    NotifyPropertyChanged(nameof(EditItem.Density));
                }
            }
        }

        private AverageMeasuredDensity measuredDensities = new AverageMeasuredDensity();

        public AverageMeasuredDensity EditMeasuredDensity
        {
            get
            {
                if (measuredDensities == null)
                    measuredDensities = new AverageMeasuredDensity();
                return measuredDensities;
            }
            set => Set(ref measuredDensities, value, nameof(EditMeasuredDensity));
        }

        //public ObservableCollection<MeasuredDensity> EditMeasureDensity { get; set; } = new ObservableCollection<MeasuredDensity>();
        private DensityEditType densityEditType;

        public DensityEditType EditorType
        {
            get => densityEditType;
            set
            {
                Set(ref densityEditType, value, nameof(EditorType));
                DefinedDensityVisible = densityEditType == DensityEditType.Defined;
                MeasuredDensityVisible = densityEditType == DensityEditType.Measured;
                if (densityEditType == DensityEditType.Measured && (ReferenceEquals(measuredDensities, null) || EditMeasuredDensity?.Count == 0))
                {
                    if (ReferenceEquals(measuredDensities, null))
                        measuredDensities = new AverageMeasuredDensity();
#if DEBUG
                    if (EditMeasuredDensity.Count == 0)
                    {

                        EditMeasuredDensity.Add(new MeasuredDensity(2.98f, 1.75f, 1000f));
                        EditMeasuredDensity.Add(new MeasuredDensity(2.99f, 1.75f, 1000f));
                        EditMeasuredDensity.Add(new MeasuredDensity(3f, 1.75f, 1010f));
                        NotifyPropertyChanged(nameof(EditMeasuredDensity));
                    }
#endif

                }
            }
        }
        private bool definedDensityVisible = true;

        public bool DefinedDensityVisible
        {
            get => definedDensityVisible;
            set => Set(ref definedDensityVisible, value);
        }

        private bool measuredDensityVisible = false;

        public bool MeasuredDensityVisible
        {
            get => measuredDensityVisible;
            set => Set<bool>(ref measuredDensityVisible, value);
        }

        public string Error => string.Empty;

        public string this[string columnName]
        {
            get
            {
                if (columnName == nameof(EditDefinedDensity) && EditorType == DensityEditType.Defined && EditDefinedDensity.Density == 0.0f)
                    return $"Defined density must be greater than zero.";
                else if (columnName == nameof(EditMeasuredDensity) && !ReferenceEquals(EditMeasuredDensity, null))
                {
                    if (EditMeasuredDensity.Count < AverageMeasuredDensity.MeasurementCountForAverage)
                        return $"Measured density requires at least {AverageMeasuredDensity.MeasurementCountForAverage} measurements to determine an accurate density.";
                    else
                        return string.Empty;
                }
                else
                    return string.Empty;
            }
        }

        protected override void SaveNew()
        {
            if (editItem.FilamentID == default)
                DataFile.Document.Filaments.Add(EditItem);
            //throw new NotImplementedException();
        }

        internal void UpdateForCellEdit()
        {
            if (EditorType == DensityEditType.Measured)
                EditMeasuredDensity.RefreshDensity();
        }
    }
}
