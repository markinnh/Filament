using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace HintLib
{
    public class BaseObject : INotifyPropertyChanged
    {
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        protected void Set<TSet>(ref TSet target, TSet value, [CallerMemberName] string propertyName = null)
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
        {
            if (!EqualityComparer<TSet>.Default.Equals(target, value))
            {
                target = value;
                OnPropertyChanged(propertyName);
                if (!InDataOps && propertyName != nameof(IsModified))
                    IsModified = true;
            }
        }
        [JsonIgnore,NotMapped]
        internal static bool InDataOps { get; set; }

        private bool isModified;
        [JsonIgnore,NotMapped]
        public virtual bool IsModified
        {
            get => isModified;
            set => Set<bool>(ref isModified, value);
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    }
}