using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Filament_Db
{
    public class ConvertVolume : INotifyPropertyChanged
    {
        public double Measurement => FilamentMath.ConvertTo(inputVolume, supportedVolume, convertToVolume);

        private double inputVolume;

        public double InputVolume
        {
            get { return inputVolume; }
            set
            {
                if (inputVolume != value)
                {
                    inputVolume = value;
                    OnPropertyChanged();
                }

            }
        }

        private SupportedVolume supportedVolume;

        public SupportedVolume SupportedVolume
        {
            get { return supportedVolume; }
            set
            {
                if (supportedVolume != value)
                {
                    supportedVolume = value;
                    OnPropertyChanged();
                }

            }
        }

        private ConvertToVolume convertToVolume;

        public ConvertToVolume ConvertToVolume
        {
            get { return convertToVolume; }
            set
            {
                if (convertToVolume != value)
                {
                    convertToVolume = value;
                    OnPropertyChanged();
                }

            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
