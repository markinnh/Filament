using CommunityToolkit.Mvvm.Messaging.Messages;
using DataDefinitions.Models;

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
    public class TagInteractionNotification : ValueChangedMessage<TagInteraction>
    {
        public TagInteractionNotification(TagInteraction value) : base(value) { }
    }
    public class VendorDefnListChanged : ValueChangedMessage<ObservableCollection<VendorDefn>>
    {
        public VendorDefnListChanged(ObservableCollection<VendorDefn> value) : base(value)
        {
        }
    }
    public class PrintSettingDefnListChanged : ValueChangedMessage<DataDefinitions.DatabaseObjectActions>
    {
        public PrintSettingDefnListChanged(DataDefinitions.DatabaseObjectActions objectActions) : base(objectActions) { }
    }
    public class ItemAdded<TAdded> : ValueChangedMessage<TAdded>
    {
        public ItemAdded(TAdded added) : base(added) { }
    }
    public class ItemRemoved<TRemoved> : ValueChangedMessage<TRemoved>
    {
        public ItemRemoved(TRemoved removed) : base(removed) { }
    }
}
