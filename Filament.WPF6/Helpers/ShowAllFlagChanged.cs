using CommunityToolkit.Mvvm.Messaging.Messages;

using System;
using System.Collections.Generic;
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
    public class ItemAdded<TAdded> : ValueChangedMessage<TAdded>
    {
        public ItemAdded(TAdded added) : base(added) { }
    }
    public class ItemRemoved<TRemoved> : ValueChangedMessage<TRemoved>
    {
        public ItemRemoved(TRemoved removed) : base(removed) { }
    }
}
