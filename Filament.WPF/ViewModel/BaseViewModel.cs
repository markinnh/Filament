using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Filament.WPF.ViewModel
{
    [AttributeUsage(AttributeTargets.Class,AllowMultiple =false)]
    public class ViewModelDescriptorAttribute : Attribute
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
    }
    
    public abstract class BaseViewModel<T> :IViewModel, INotifyPropertyChanged where T : new()
    {
        public ViewModelDescriptorAttribute ModelAttribute => GetType().GetCustomAttribute<ViewModelDescriptorAttribute>();
        public string Title => ModelAttribute?.Title ?? "undefined";
        public string Description => ModelAttribute?.Description ?? "undefined";
        public string Category => ModelAttribute?.Category ?? "None";

        public static Filament_Data.JsonModel.DataFile DataFile { get; protected set; }

        protected T editItem;

        public virtual T EditItem { get => editItem; set => Set(ref editItem, value); }

        public string EditType { get => typeof(T).Name; }

        private bool inAddNew = false;
        public bool InAddNew { get => inAddNew; protected set => Set(ref inAddNew, value); }
        public bool CanAddNew => !inAddNew;
        protected abstract void SaveNew();
        protected virtual void DoNew()
        {
            if (InAddNew)
                return;

            EditItem = new T();
            InAddNew = true;
            NotifyPropertyChanged(nameof(CanAddNew));
        }

        public ICommand NewCommand { get; set; }

        public ICommand SaveCommand { get; set; }
        static BaseViewModel()
        {
            InitDataFile();
        }
        protected static void InitDataFile()
        {
            DataFile = new Filament_Data.JsonModel.DataFile(Properties.Settings.Default.RecreateDocument);
        }
        protected BaseViewModel()
        {
            NewCommand = new Helpers.RelayCommand(DoNew);
            SaveCommand = new Helpers.RelayCommand(SaveNew);
            if (GetType().GetCustomAttribute<ViewModelDescriptorAttribute>() == null)
                throw new NotImplementedException($"{GetType().Name} requires the {nameof(ViewModelDescriptorAttribute)} to be defined.");
        }



        public event PropertyChangedEventHandler PropertyChanged;

        protected bool Set<PType>(ref PType target, PType changeValue, [CallerMemberName] string propertyName = null)
        {
            if (!EqualityComparer<PType>.Default.Equals(target, changeValue))
            {
                target = changeValue;
                NotifyPropertyChanged(propertyName);
                return true;
            }
            return false;
        }

        protected void NotifyPropertyChanged(string Property) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(Property));

        protected void SaveDataFile()
        {
            DataFile.SaveFile();
        }
    }
}
