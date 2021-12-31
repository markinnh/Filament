using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using DataDefinitions.Models;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Filament.UWP.Views
{
    public sealed partial class SpoolDetailControl : UserControl
    {
        public List<string> SupportedColors { get; set; } = new List<string>(new string[] {"Black","White","Red","Green","Blue","Silver","Grey","Orange" });
        public SpoolDetailControl()
        {
            this.InitializeComponent();
        }


        public SpoolDefn EditSpoolDefn
        {
            get { return (SpoolDefn)GetValue(EditSpoolDefnProperty); }
            set { SetValue(EditSpoolDefnProperty, value); }
        }

        // Using a DependencyProperty as the backing store for EditSpoolDefn.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EditSpoolDefnProperty =
            DependencyProperty.Register("EditSpoolDefn", typeof(SpoolDefn), typeof(SpoolDetailControl), new PropertyMetadata(null,OnEditSpoolChanged));

        private static void OnEditSpoolChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //throw new NotImplementedException();
        }
    }
}
