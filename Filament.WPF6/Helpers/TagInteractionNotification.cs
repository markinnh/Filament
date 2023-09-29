using CommunityToolkit.Mvvm.Messaging.Messages;

namespace Filament.WPF6.Helpers
{
    public class TagInteractionNotification : ValueChangedMessage<ContentInteraction>
    {
        public TagInteractionNotification(ContentInteraction value) : base(value) { }
    }
}
