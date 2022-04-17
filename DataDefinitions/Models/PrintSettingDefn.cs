using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataDefinitions.Models
{
    public enum SupportedSettingValueType
    {
        Integer=0x100,
        @Boolean,
        Float,
        YesNo
    }
    /// <summary>
    /// Print setting definitions that can affect the print and change based on filament type which the user might want to record
    /// </summary>
    public class PrintSettingDefn : DatabaseObject
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

        public int PrintSettingDefnId { get; set; }

        private string myDefinition;
        [MaxLength(128)]
        public string Definition
        {
            get => myDefinition;
            set => Set<string>(ref myDefinition, value);
        }
        private SupportedSettingValueType settingValueType;

        public SupportedSettingValueType SettingValueType
        {
            get => settingValueType;
            set => Set<SupportedSettingValueType>(ref settingValueType, value);
        }

        public static void SetDataOperationsState(bool state)
        {
            InDataOps = state;
        }
        public override void SetContainedModifiedState(bool state)
        {
            IsModified = state;
        }
        public override bool InDatabase => PrintSettingDefnId != default;
        public override bool IsValid => !string.IsNullOrEmpty(myDefinition);
    }
}
