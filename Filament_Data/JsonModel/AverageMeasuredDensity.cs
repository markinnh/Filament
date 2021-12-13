using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Collections.ObjectModel;
using Filament_Data.ProjectAttributes;
using Newtonsoft.Json;
using System.Collections.Specialized;
using System.ComponentModel;
using static System.Diagnostics.Debug;

namespace Filament_Data.JsonModel
{
    // TODO: Develop a UI for AverageMeasuredDensity, it should be part of the FilamentDefn UI.
    /// <summary>
    /// A collection of MeasureDensity, the average is taken
    /// </summary>
    /// <seealso cref="System.Collections.ObjectModel.ObservableCollection{Filament_Data.MeasuredDensity}" />
    /// <seealso cref="Filament_Data.IDensity" />
    /// <seealso cref="System.IComparable{Filament_Data.IDensity}" />
    /// <seealso cref="System.IEquatable{Filament_Data.IDensity}" />
    public class AverageMeasuredDensity : ObservableCollection<MeasuredDensity>, INotifyPropertyChanged, IDensity, IComparable<IDensity>, IEquatable<IDensity>
    {
        // TODO: Map this to a user setting
        /// <summary>
        /// Gets or sets the digits.
        /// </summary>
        /// <value>
        /// The digits.
        /// </value>
        [UserEditableSetting("User Editable", "Averaging", "Fraction digits",
            Tooltip = "Digits to right of decimal point",
            SettingKey = "ei3M3A3UgEq9eAmA4h5r4g=="), JsonIgnore()]
        public static byte FractionalDigits { get; set; } = 6;
        [UserEditableSetting("User Editable", "Averaging", "Measurement Count",
            Tooltip = "Number of measurements needed to determine an average",
            SettingKey = "56DwgvkhT0GyoW+34b+l5w=="), JsonIgnore()]
        public static byte MeasurementCountForAverage { get; set; } = 3;
        public AverageMeasuredDensity(IEnumerable<MeasuredDensity> measuredDensities) : base(measuredDensities) { }
        public AverageMeasuredDensity()
        {
            this.CollectionChanged += RefreshDensity;
        }

        private void RefreshDensity(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                RefreshDensity();
            }
            else
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(Density)));
        }
        public void RefreshDensity()
        {
            WriteLineIf(CompletedCount >= MeasurementCountForAverage, $"Current Density is : {Density}, Count : {Count}");
            WriteLineIf(CompletedCount >= MeasurementCountForAverage, $"Current Density is : {CompletedItems.Average(md => md.Density)}, Completed Count :{CompletedCount}");
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(Density)));
        }

        [JsonIgnore]
        public double Density => CompletedCount >= MeasurementCountForAverage ?
            Math.Round(CompletedItems.Average(md => md.Density), FractionalDigits) :
            0.0f;
        //public int CompareTo(float other)
        //{
        //    return Density.CompareTo(other);
        //}
        [JsonIgnore]
        protected IEnumerable<MeasuredDensity> CompletedItems => this.Where(md => !double.IsNaN(md.Density));
        [JsonIgnore]
        protected int CompletedCount => this.Where(md => !double.IsNaN(md.Density)).Count();

        public bool Equals(float other)
        {
            return Density == other;
        }

        public static implicit operator double(AverageMeasuredDensity measuredDensities) => measuredDensities.Density;

        public static bool operator ==(AverageMeasuredDensity left, AverageMeasuredDensity right)
        {
            if (ReferenceEquals(left, null) || ReferenceEquals(right, null))
                return false;
            if (left.Count != right.Count)
                return false;
            foreach (var item in left)
            {
                if (!right.Contains(item))
                    return false;
            }
            return true;
        }

        public static bool operator !=(AverageMeasuredDensity left, AverageMeasuredDensity right)
        {
            return !(left == right);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            else if (ReferenceEquals(obj, null))
            {
                return false;
            }
            else if (obj is IDensity iDensity)
                return this.Density == iDensity.Density;
            else
                return false;

            //throw new NotImplementedException();
        }

        public override int GetHashCode()
        {
            int hash = 0;
            foreach (var item in this)
            {
                hash ^= item.GetHashCode();
            }
            return hash;
            //throw new NotImplementedException();
        }

        public bool Equals(IDensity other)
        {
            return other == null ? false : this.Density == other.Density;
        }

        public int CompareTo(IDensity other)
        {
            if (ReferenceEquals(other, null))
                return 1;
            else if (this < other)
                return -1;
            else if (this > other)
                return 1;
            else
                return 0;
        }

        public static bool operator <(AverageMeasuredDensity left, IDensity density)
        {
            return ReferenceEquals(left, null) ? !ReferenceEquals(density, null) : left.CompareTo(density) < 0;
        }
        public static bool operator >(AverageMeasuredDensity left, IDensity density)
        {
            return ReferenceEquals(left, null) ? !ReferenceEquals(density, null) : left.CompareTo(density) > 0;
        }
        public static bool operator <=(AverageMeasuredDensity left, IDensity density)
        {
            return ReferenceEquals(left, null) ? !ReferenceEquals(density, null) : left.CompareTo(density) <= 0;
        }
        public static bool operator >=(AverageMeasuredDensity left, IDensity density)
        {
            return ReferenceEquals(left, null) ? !ReferenceEquals(density, null) : left.CompareTo(density) >= 0;
        }

        public static bool operator <(AverageMeasuredDensity left, AverageMeasuredDensity right)
        {
            return ReferenceEquals(left, null) ? !ReferenceEquals(right, null) : left.CompareTo(right) < 0;
        }

        public static bool operator <=(AverageMeasuredDensity left, AverageMeasuredDensity right)
        {
            return ReferenceEquals(left, null) || left.CompareTo(right) <= 0;
        }

        public static bool operator >(AverageMeasuredDensity left, AverageMeasuredDensity right)
        {
            return !ReferenceEquals(left, null) && left.CompareTo(right) > 0;
        }

        public static bool operator >=(AverageMeasuredDensity left, AverageMeasuredDensity right)
        {
            return ReferenceEquals(left, null) ? ReferenceEquals(right, null) : left.CompareTo(right) >= 0;
        }
    }
}
