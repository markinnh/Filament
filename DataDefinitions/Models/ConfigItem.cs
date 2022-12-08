using DataDefinitions.Interfaces;
using LiteDB;
using MyLibraryStandard.Attributes;
using System;
using System.Text.Json.Serialization;


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
    public class ConfigItem : DataObject,ITrackModified
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
        
        
        
        

        [JsonIgnore,BsonIgnore]
        public override  bool InDataOperations => InDataOps;
        private int configItemId;
        //[Affected(Names = new string[] { nameof(InDatabase) })]
        [BsonIgnore]
        public int ConfigItemId
        {
            get => configItemId;
            set => Set(ref configItemId, value);
        }
        public int VendorSettingsConfigId { get; set; }
        [JsonIgnore, BsonIgnore]
        public virtual VendorPrintSettingsConfig VendorSettings { get; set; }

        public int PrintSettingDefnId { get; set; }
        private PrintSettingDefn settingDefn;
        [JsonIgnore, BsonIgnore]
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
        [System.Text.Json.Serialization.JsonIgnore,BsonIgnore]
        public object Value
        {
            get
            {
                if (PrintSettingDefn != null)
                    switch (PrintSettingDefn.SettingValueType)
                    {
                        case SupportedSettingValueType.Integer:
                            if (int.TryParse(myTextValue, out int value))
                                return value;
                            else
                                throw new ArgumentOutOfRangeException($"Unable to convert {myTextValue} to an integer");
                            break;
                        case SupportedSettingValueType.YesNo:
                            var ynResult =myTextValue.ToLower()=="yes";
                            return ynResult;
                            break;

                        case SupportedSettingValueType.Boolean:
                            if (bool.TryParse(myTextValue, out bool result))
                                return result;
                            else
                                throw new ArgumentOutOfRangeException($"unable to convert {myTextValue} to a boolean(true/false)");
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
        //public override bool InDatabase => ConfigItemId != default;
        //internal override int KeyID 
        //{ 
        //    get => ConfigItemId; 
        //    set => ConfigItemId = value; 
        //}
        //protected override void AssignKey(int myId)
        //{
        //    if (ConfigItemId == default)
        //        ConfigItemId = myId;
        //    else
        //        ReportKeyAlreadyInitialized();
        //}
        [JsonIgnore,BsonIgnore]
        public override bool IsValid => PrintSettingDefnId != default && !string.IsNullOrEmpty(myTextValue);
    }
}
