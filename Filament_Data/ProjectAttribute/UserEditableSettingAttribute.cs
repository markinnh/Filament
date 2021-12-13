using System;
using System.Collections.Generic;
using System.Text;

namespace Filament_Data.ProjectAttributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class UserEditableSettingAttribute:System.Attribute
    {
        public string Group { get; set; }
        public string SubGroup { get; set; }
        public string ReadableName { get; set; }
        public string SettingKey { get; set; }
        public string Tooltip { get; set; }
        public UserEditableSettingAttribute(string group,string subGroup,string readableName)
        {
            Group = group;
            SubGroup = subGroup;
            ReadableName = readableName;
        }
    }
}
