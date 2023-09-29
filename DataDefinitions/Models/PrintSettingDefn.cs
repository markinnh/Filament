using LiteDB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Text;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace DataDefinitions.Models
{
    [Flags]
    public enum SettingAppliesTo
    {
        Vendor,
        [Description("Vendor specific filament")]
        VendorFilament,
        [Description("Vendor filament color")]
        VendorFilamentColor,
        [Description("Vendor filament and color")]
        VendorFilamentAndColor = VendorFilament | VendorFilamentColor
    }
    /// <summary>
    /// Print setting definitions that can affect the print and change based on filament type which the user might want to record
    /// </summary>
    public class PrintSettingDefn : DataDefinitions.DatabaseObject
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
        public override bool InDataOperations => InDataOps;
        [XmlAttribute("ID"),JsonPropertyName("ID")]
        public int PrintSettingDefnId { get; set; }

        private string myDefinition;
        [XmlAttribute("definition")]
        public string Definition
        {
            get => myDefinition;
            set => Set<string>(ref myDefinition, value);
        }
        private SupportedSettingValueType settingValueType;
        [XmlAttribute("valueType"),JsonPropertyName("ValueType")]
        public SupportedSettingValueType SettingValueType
        {
            get => settingValueType;
            set => Set<SupportedSettingValueType>(ref settingValueType, value);
        }
        private string? items;

        public string? Items
        {
            get => items;
            set => Set(ref items, value);
        }

        public static void SetDataOperationsState(bool state)
        {
            InDataOps = state;
        }
        public override void SetContainedModifiedState(bool state)
        {
            IsModified = state;
        }
        //public override bool InDatabase => PrintSettingDefnId != default;
        [JsonIgnore,BsonIgnore]
        public override bool IsValid => !string.IsNullOrEmpty(myDefinition);
        protected override void SaveToJsonDatabase()
        {
            //Document.Add(this);
        }
        
        protected override bool NeedsKey => true;
        internal override int KeyID { get => PrintSettingDefnId; set => PrintSettingDefnId = value; }
        //protected override void AssignKey(int myId)
        //{
        //    if (PrintSettingDefnId == default)
        //        PrintSettingDefnId = myId;
        //    else
        //        ReportKeyAlreadyInitialized();
        //}
    }
}
