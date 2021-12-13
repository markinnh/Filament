using Filament_Db.DataContext;
using System;
using System.Collections.Generic;
using System.Text;

namespace Filament_Db.Models
{
    public class Setting
    {
        public int SettingId { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }

        public Setting() { }
        public Setting(string name, object value)
        {
            System.Diagnostics.Debug.Assert(value != null);
            Name = name;
            Value = value?.ToString() ?? "undefined";
        }
        public override string ToString()
        {
            return $@"{{
    Name = {("\"" + Name + "\"")},
    Value = {Value}
}}";
        }
        public void SetValue(object value)
        {
            if (value != null)
            {
#pragma warning disable CS8601 // Possible null reference assignment.
                Value = value.ToString();
#pragma warning restore CS8601 // Possible null reference assignment.
            }
        }
        public static void UpdateOrAdd(string settingName, object value)
        {
            if (value != null)
            {
                if (FilamentContext.GetSetting(s => s.Name == settingName) is Setting setting)
                {
#pragma warning disable CS8601 // Possible null reference assignment.
                    setting.Value = value.ToString();
#pragma warning restore CS8601 // Possible null reference assignment.
                    FilamentContext.UpdateItems(setting);
                }
                else
                {
#pragma warning disable CS8604 // Possible null reference argument.
                    FilamentContext.AddAll(1,new Setting(settingName, value.ToString()));
#pragma warning restore CS8604 // Possible null reference argument.
                }
            }
        }
        public static Setting? GetSetting(string settingName) => FilamentContext.GetSetting(s => s.Name == settingName);

        public static implicit operator int(Setting setting)
        {
            if (int.TryParse(setting.Value, out int result))
                return result;
            return 0;
        }
        public static implicit operator bool(Setting setting)
        {
            if (bool.TryParse(setting.Value, out bool result))
                return result;
            else
                return false;
        }
        public static implicit operator DateTime(Setting setting)
        {
            if (DateTime.TryParse(setting.Value, out DateTime result))
                return result;
            else
                return DateTime.MinValue;
        }
        public static implicit operator double(Setting setting)
        {
            if (double.TryParse(setting.Value, out double result))
                return result;
            else
                return double.NaN;
        }
    }
}
