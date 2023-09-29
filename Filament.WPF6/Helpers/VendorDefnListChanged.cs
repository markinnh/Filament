using CommunityToolkit.Mvvm.Messaging.Messages;
using DataDefinitions.Models;
using System.Collections.ObjectModel;

namespace Filament.WPF6.Helpers
{
    public class VendorDefnListChanged : ValueChangedMessage<ObservableCollection<VendorDefn>>
    {
        public VendorDefnListChanged(ObservableCollection<VendorDefn> value) : base(value)
        {
        }
    }
}
