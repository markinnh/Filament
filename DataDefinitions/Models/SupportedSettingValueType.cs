using System.ComponentModel;

namespace DataDefinitions.Models
{
    public enum SupportedSettingValueType
    {
        Integer = 0x100,
        @Boolean,
        Float,
        [Description("Yes or No")]
        YesNo,
        [Description("On/Off")]
        OnOff,
        OneOfItems
    }
}
