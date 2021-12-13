using Filament_Db.Models;
using Microsoft.Toolkit.Mvvm.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filament.WPF6.Helpers
{
    public class ShowAllFlagChanged : ValueChangedMessage<ShowAllFlag>
    {
        public ShowAllFlagChanged(ShowAllFlag value) : base(value)
        {
        }
    }
    public class VendorDefnListChanged : ValueChangedMessage<ObservableCollection<VendorDefn>>
    {
        public VendorDefnListChanged(ObservableCollection<VendorDefn> value) : base(value)
        {
        }
    }
}
