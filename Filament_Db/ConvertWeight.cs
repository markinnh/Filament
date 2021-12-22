using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Filament_Db
{
    public class ConvertWeight : INotifyPropertyChanged
    {

        public double Measurement => FilamentMath.ConvertTo(inputWeight, supportedWeight, convertToWeight);

        private double inputWeight;

        public double InputWeight
        {
            get { return inputWeight; }
            set
            {
                if (inputWeight != value)
                {
                    inputWeight = value;
                    OnPropertyChanged();
                }

            }
        }

        private SupportedWeight supportedWeight= SupportedWeight.Gram;

        public SupportedWeight SupportedWeight
        {
            get { return supportedWeight; }
            set
            {
                if (supportedWeight != value)
                {
                    supportedWeight = value;
                    OnPropertyChanged();
                }

            }
        }

        private ConvertToWeight convertToWeight= ConvertToWeight.Grams ;

        public ConvertToWeight ConvertToWeight
        {
            get { return convertToWeight; }
            set
            {
                if (convertToWeight != value)
                {
                    convertToWeight = value;
                    OnPropertyChanged();
                }

            }
        }


        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
