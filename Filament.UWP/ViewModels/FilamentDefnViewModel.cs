using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

using Filament.UWP.Core.Models;
using Filament.UWP.Core.Services;

using Microsoft.Toolkit.Uwp.UI.Controls;
using DataDefinitions.Models;
using Filament.UWP.Core.Helpers;

namespace Filament.UWP.ViewModels
{
    public class FilamentDefnViewModel : DataDefinitions.Observable
    {
        private FilamentDefn _selected;

        public FilamentDefn Selected
        {
            get { return _selected; }
            set { Set(ref _selected, value); }
        }

        private ObservableCollection<FilamentDefn> items=new ObservableCollection<FilamentDefn>();
            
        public ObservableCollection<FilamentDefn> Items
        {
            get => items;
            set => Set<ObservableCollection<FilamentDefn>>(ref items, value);
        }

        //public ObservableCollection<FilamentDefn> SampleItems { get; private set; } = new ObservableCollection<FilamentDefn>();

        public FilamentDefnViewModel()
        {
        }

        public void LoadData(ListDetailsViewState viewState)
        {
            Items.Clear();

            var data = Singleton<DataContext.DataLayer>.Instance.FilamentList;
            if (data is ObservableCollection<FilamentDefn> odata)
                Items = odata;
            else
                foreach (var item in data)
                {
                    Items.Add(item);
                }

            if (viewState == ListDetailsViewState.Both)
            {
                Selected = Items.First();
            }
        }
    }
}
