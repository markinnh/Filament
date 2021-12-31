
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Filament.WPF6.Resources
{
    public class SupportingItems:ResourceDictionary
    {
        public SupportingItems()
        {
            System.Diagnostics.Debug.WriteLine("Initializing SupportingItems");
            BeginInit();
            EndInit();
        }
    }
}
