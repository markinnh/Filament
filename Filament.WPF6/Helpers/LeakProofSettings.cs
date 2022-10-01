using DataDefinitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filament.WPF6.Helpers
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// All this class does is provide a memory leak proof mechanism to tie Settings to the UI
    /// </remarks>
    internal class LeakProofSettings : Observable
    {
        public string SimpleInvColumn1
        {
            get => Filament.WPF6.Properties.Settings.Default.SimpleInvColumn1;
            set
            {
                if (value != Properties.Settings.Default.SimpleInvColumn1)
                {
                    Properties.Settings.Default.SimpleInvColumn1 = value;
                    OnPropertyChanged(nameof(SimpleInvColumn1));
                }
            }
        }
        public string ByVendorColumn1
        {
            get => Properties.Settings.Default.ByVendorColumn1;
            set
            {
                if (value != Properties.Settings.Default.ByVendorColumn1)
                {
                    Properties.Settings.Default.ByVendorColumn1 = value;
                    OnPropertyChanged(nameof(ByVendorColumn1));
                }
            }
        }

        public string VendorDefnColumn1
        {
            get => Properties.Settings.Default.VendorDefnColumn1;
            set
            {
                if (value != Properties.Settings.Default.VendorDefnColumn1)
                {
                    Properties.Settings.Default.VendorDefnColumn1 = value;
                    OnPropertyChanged(nameof(VendorDefnColumn1));
                }
            }
        }

        public string FilamentDefnColumn1
        {
            get => Properties.Settings.Default.FilamentDefnColumn1;
            set
            {
                if (value != Properties.Settings.Default.FilamentDefnColumn1)
                {
                    Properties.Settings.Default.FilamentDefnColumn1 = value;
                    OnPropertyChanged(nameof(FilamentDefnColumn1));
                }
            }
        }
        #region Individual DataGrid Columns
        public string DescriptionColumnWidth
        {
            get => Properties.Settings.Default.DescriptionColumnWidth;
            set
            {
                if (value != Properties.Settings.Default.DescriptionColumnWidth)
                {
                    Properties.Settings.Default.DescriptionColumnWidth = value;
                    OnPropertyChanged(nameof(DescriptionColumnWidth));
                }
            }
        }
        public string DiameterColumnWidth
        {
            get => Properties.Settings.Default.DiameterColumnWidth;
            set
            {
                if (value != Properties.Settings.Default.DiameterColumnWidth)
                {
                    Properties.Settings.Default.DiameterColumnWidth = value;
                    OnPropertyChanged(nameof(DiameterColumnWidth));
                }
            }
        }


        public string MeasuredOnColumnWidth
        {
            get => Properties.Settings.Default.MeasuredOnColumnWidth;
            set
            {
                if (value != Properties.Settings.Default.MeasuredOnColumnWidth)
                {
                    Properties.Settings.Default.MeasuredOnColumnWidth = value;
                    OnPropertyChanged(nameof(MeasuredOnColumnWidth));
                }
            }
        }

        public string DepthLeftColumnWidth
        {
            get => Properties.Settings.Default.DepthLeftColumnWidth;
            set
            {
                if (value != Properties.Settings.Default.DepthLeftColumnWidth)
                {
                    Properties.Settings.Default.DepthLeftColumnWidth = value;
                    OnPropertyChanged(nameof(DepthLeftColumnWidth));
                }
            }
        }

        public string DepthRightColumnWidth
        {
            get => Properties.Settings.Default.DepthRightColumnWidth;
            set
            {
                if (value != Properties.Settings.Default.DepthRightColumnWidth)
                {
                    Properties.Settings.Default.DepthRightColumnWidth = value;
                    OnPropertyChanged(nameof(DepthRightColumnWidth));
                }
            }
        }

        public string RemainingGColumnWidth
        {
            get => Properties.Settings.Default.RemainingGColumnWidth;   
            set
            {
                if (value != Properties.Settings.Default.RemainingGColumnWidth)
                {
                    Properties.Settings.Default.RemainingGColumnWidth = value;
                    OnPropertyChanged(nameof(RemainingGColumnWidth));
                }
            }
        }


        public string RemainingMColumnWidth
        {
            get => Properties.Settings.Default.RemainingMColumnWidth;
            set
            {
                if (value != Properties.Settings.Default.RemainingMColumnWidth)
                {
                    Properties.Settings.Default.RemainingMColumnWidth = value;
                    OnPropertyChanged(nameof(RemainingMColumnWidth));
                }
            }
        }


        public string SpoolWidthColumnWidth
        {
            get => Properties.Settings.Default.SpoolWidthColumnWidth;
            set
            {
                if (value != Properties.Settings.Default.SpoolWidthColumnWidth)
                {
                    Properties.Settings.Default.SpoolWidthColumnWidth = value;
                    OnPropertyChanged(nameof(SpoolWidthColumnWidth));
                }
            }
        }


        public string DrumDiameterColumnWidth
        {
            get => Properties.Settings.Default.DrumDiameterColumnWidth;
            set
            {
                if (value != Properties.Settings.Default.DrumDiameterColumnWidth)
                {
                    Properties.Settings.Default.DrumDiameterColumnWidth = value;
                    OnPropertyChanged(nameof(DrumDiameterColumnWidth));
                }
            }
        }


        public string WeightColumnWidth
        {
            get => Properties.Settings.Default.WeightColumnWidth;
            set
            {
                if (value != Properties.Settings.Default.WeightColumnWidth)
                {
                    Properties.Settings.Default.WeightColumnWidth = value;
                    OnPropertyChanged(nameof(WeightColumnWidth));
                }
            }
        }


        public string StopUsingColumnWidth
        {
            get => Properties.Settings.Default.StopUsingColumnWidth;
            set
            {
                if (value != Properties.Settings.Default.StopUsingColumnWidth)
                {
                    Properties.Settings.Default.StopUsingColumnWidth = value;
                    OnPropertyChanged(nameof(StopUsingColumnWidth));
                }
            }
        }


        public string VerifiedColumnWidth
        {
            get => Properties.Settings.Default.VerifiedColumnWidth;
            set
            {
                if (value != Properties.Settings.Default.VerifiedColumnWidth)
                {
                    Properties.Settings.Default.VerifiedColumnWidth = value;
                    OnPropertyChanged(nameof(VerifiedColumnWidth));
                }
            }
        }


        public string FilamentColumnWidth
        {
            get => Properties.Settings.Default.FilamentColumnWidth;
            set
            {
                if (value != Properties.Settings.Default.FilamentColumnWidth)
                {
                    Properties.Settings.Default.FilamentColumnWidth = value;
                    OnPropertyChanged(nameof(FilamentColumnWidth));
                }
            }
        }


        public string DefinitionColumnWidth
        {
            get => Properties.Settings.Default.DefinitionColumnWidth;
            set
            {
                if (value != Properties.Settings.Default.DefinitionColumnWidth)
                {
                    Properties.Settings.Default.DefinitionColumnWidth = value;
                    OnPropertyChanged(nameof(DefinitionColumnWidth));
                }
            }
        }


        public string DateOpenedColumnWidth
        {
            get => Properties.Settings.Default.DateOpenedColumnWidth;
            set
            {
                if (value != Properties.Settings.Default.DateOpenedColumnWidth)
                {
                    Properties.Settings.Default.DateOpenedColumnWidth = value;
                    OnPropertyChanged(nameof(DateOpenedColumnWidth));
                }
            }
        }


        public string ColorColumnWidth
        {
            get => Properties.Settings.Default.ColorColumnWidth;
            set
            {
                if (value != Properties.Settings.Default.ColorColumnWidth)
                {
                    Properties.Settings.Default.ColorColumnWidth = value;
                    OnPropertyChanged(nameof(ColorColumnWidth));
                }
            }
        }


        #endregion
    }
}
