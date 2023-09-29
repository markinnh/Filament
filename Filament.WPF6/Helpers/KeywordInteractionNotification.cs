using CommunityToolkit.Mvvm.Messaging.Messages;

namespace Filament.WPF6.Helpers
{
    public class KeywordInteractionNotification : ValueChangedMessage<ContentInteraction>
    {
        public KeywordInteractionNotification(ContentInteraction value):base(value) { }
    }
}
