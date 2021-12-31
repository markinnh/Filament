using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Filament.UWP.Helpers
{
    public class RoamingAppSettings : INotifyPropertyChanged
    {

        //const string defaultOriginalOnlineAccounts = "[\"Not Assigned\",\"Hotmail\",\"Yahoo\",\"Gmail\",\"Olive Garden\"]";
        //const string defaultUpdatedOnlineAccounts = "[{\"Content\":\"Not Assigned\",\"IsDeletable\":false,\"IsEditable\":false},{\"Content\":\"Hotmail\",\"IsDeletable\":false,\"IsEditable\":false},{\"Content\":\"Yahoo\",\"IsDeletable\":false,\"IsEditable\":false},{\"Content\":\"Gmail\",\"IsDeletable\":false,\"IsEditable\":false},{\"Content\":\"Olive Garden\",\"IsDeletable\":true,\"IsEditable\":true}]";
        //const string defaultPasswordsCopied = "[{\"Deletable\":false,\"Password\":\"Pcb^2kuFN1\",\"CopiedAt\":\"2021-07-11T18:19:42.9438587-04:00\",\"CopyCount\":0,\"AssignedTo\":\"Gmail\"},{\"Deletable\":false,\"Password\":\"RWy6^wo05nUqjI\",\"CopiedAt\":\"2021-07-11T18:19:49.593037-04:00\",\"CopyCount\":0,\"AssignedTo\":\"Not Assigned\"}]";

        public bool UpdateMigration
        {
            get => GetSetting(true);
            set
            {
                SaveSetting(value);
                NotifyPropertyChanged();
            }
        }
                
        private bool RemoveSetting(string settingName)
        {

            return Roaming.Values.Remove(settingName);
        }

        internal ApplicationDataContainer Roaming => ApplicationData.Current.RoamingSettings;

        public event PropertyChangedEventHandler PropertyChanged;

        private void SaveSetting(object setting, [CallerMemberName] string settingName = null)
        {
            if (!string.IsNullOrEmpty(settingName))
                Roaming.Values[settingName] = setting;
        }

        private T GetSetting<T>(T defaultValue, [CallerMemberName] string settingName = null)
        {
            if (!string.IsNullOrEmpty(settingName))
            {
                if (Roaming.Values.ContainsKey(settingName))
                {
                    var result = (T)Roaming.Values[settingName];
                    if (null == result)
                    {
                        return defaultValue;
                    }
                    else
                        return result;
                }
            }
            if (null != defaultValue)
                return defaultValue;
            else
                return default;

        }
        public RoamingAppSettings()
        {
        }

        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
