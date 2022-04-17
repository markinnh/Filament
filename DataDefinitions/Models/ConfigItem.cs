using MyLibraryStandard.Attributes;
using System;
using System.ComponentModel.DataAnnotations.Schema;

/*
 * The configuration is now part of the vendor definition, it should be there, since the settings are specific to the vendor, and not a spool definition
 * or a inventory definition, usually, for the major filament providers, there is consistency from spool to spool, however, there might be differences
 * for individual colors as I have personally observed.
  */
namespace DataDefinitions.Models
{
    /// <summary>
    /// Individual settings for the configuration.
    /// </summary>
    public class ConfigItem : DatabaseObject
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
        private int configItemId;
        //[Affected(Names = new string[] { nameof(InDatabase) })]
        public int ConfigItemId
        {
            get => configItemId;
            set => Set(ref configItemId, value);
        }
        public int VendorSettingsConfigId { get; set; }
        public virtual VendorSettingsConfig VendorSettings { get; set; }

        public int PrintSettingDefnId { get; set; }
        private PrintSettingDefn settingDefn;
        public virtual PrintSettingDefn PrintSettingDefn
        {
            get => settingDefn; set
            {
                if (Set(ref settingDefn, value) && settingDefn != null)
                    PrintSettingDefnId = settingDefn.PrintSettingDefnId;
            }
        }

        private DateTime dateEntered = DateTime.Today;

        public DateTime DateEntered
        {
            get => dateEntered;
            set => Set<DateTime>(ref dateEntered, value);
        }

        private string myTextValue;

        public string TextValue
        {
            get => myTextValue;
            set
            {
                Set<string>(ref myTextValue, value);
            }
        }
        [NotMapped]
        public object Value
        {
            get
            {
                if (PrintSettingDefn != null)
                    switch (PrintSettingDefn.SettingValueType)
                    {
                        case SupportedSettingValueType.Integer:
                            return Convert.ToInt32(TextValue);
                            break;
                        case SupportedSettingValueType.YesNo:
                        case SupportedSettingValueType.Boolean:
                            return Convert.ToBoolean(TextValue);
                            break;
                        case SupportedSettingValueType.Float:
                            return Convert.ToSingle(TextValue);
                            break;

                        default:
                            throw new ArgumentOutOfRangeException($"Unexpected {nameof(SupportedSettingValueType)}, value - {PrintSettingDefn.SettingValueType}");
                            break;
                    }
                else
                    return TextValue;
            }
            set
            {
                TextValue = value.ToString();
                OnPropertyChanged(nameof(Value));
            }
        }
        public override bool InDatabase => ConfigItemId != default;
        public override bool IsValid => PrintSettingDefnId != default && !string.IsNullOrEmpty(myTextValue);
    }
}
