﻿
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace DataDefinitions.Models
{
    public class Setting : DatabaseObject
    {
        public static event InDataOpsChangedHandler InDataOpsChanged;

        private static bool inDataOps;
        public static bool InDataOps
        {
            get => inDataOps;
            set
            {
                inDataOps = value;

                // Notify all the FilamentDefn objects of change to InDataOps state, allowing them to update the UI.
                InDataOpsChanged?.Invoke(EventArgs.Empty);
            }
        }
        public override bool InDataOperations => InDataOps;
        public override bool InDatabase => SettingId != default;
        public int SettingId { get; set; }
        private string settingName;
        public string Name { get => settingName; set => Set(ref settingName, value); }

        private string settingValue;
        public string Value { get => settingValue; set => Set(ref settingValue, value); }

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
