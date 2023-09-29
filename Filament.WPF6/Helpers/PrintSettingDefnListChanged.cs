using CommunityToolkit.Mvvm.Messaging.Messages;

namespace Filament.WPF6.Helpers
{
    public class PrintSettingDefnListChanged : ValueChangedMessage<DataDefinitions.DatabaseObjectActions>
    {
        public PrintSettingDefnListChanged(DataDefinitions.DatabaseObjectActions objectActions) : base(objectActions) { }
    }
}
