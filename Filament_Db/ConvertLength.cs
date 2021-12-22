using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Filament_Db
{
    public class ConvertLength : INotifyPropertyChanged
    {
        private double myMeasurement;

        public double Measurement => FilamentMath.ConvertTo(inputLength, supportedLength, convertToLength);
        
        private double inputLength=double.NaN;

        public double InputLength
        {
            get { return inputLength; }
            set
            {
                if (inputLength != value)
                {
                    inputLength = value;
                    OnPropertyChanged();
                }

            }
        }

        private SupportedLength supportedLength= SupportedLength.Millimeter;

        public SupportedLength SupportedLength
        {
            get { return supportedLength; }
            set
            {
                if (supportedLength != value)
                {
                    supportedLength = value;
                    OnPropertyChanged();
                }

            }
        }

        private ConvertToLength convertToLength= ConvertToLength.Millimeter;

        public ConvertToLength ConvertToLength
        {
            get { return convertToLength; }
            set
            {
                if (convertToLength != value)
                {
                    convertToLength = value;
                    OnPropertyChanged();
                }

            }
        }


        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
