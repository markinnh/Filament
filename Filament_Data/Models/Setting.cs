using System;
using System.Collections.Generic;
using System.Text;

namespace Filament_Data.Models
{
    public class Setting
    {
        public int SettingId { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }

        public Setting() { }
        public Setting(string name, object value)
        {
            Name = name;
            Value = value.ToString();
        }

        public static implicit operator int(Setting setting)=>int.Parse(setting.Name);
        public static implicit operator bool(Setting setting)=>bool.Parse(setting.Value);
        public static implicit operator DateTime(Setting setting)=>DateTime.Parse(setting.Value);
        public static implicit operator double(Setting setting)=>double.Parse(setting.Value);
    }
}
