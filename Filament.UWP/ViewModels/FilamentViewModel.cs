using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

using Filament.UWP.Core.Models;
using Filament.UWP.Core.Services;
using Filament.UWP.Helpers;

using WinUI = Microsoft.UI.Xaml.Controls;

namespace Filament.UWP.ViewModels
{
    public class FilamentViewModel : Observable
    {
        private ICommand _itemInvokedCommand;
        private object _selectedItem;

        public object SelectedItem
        {
            get => _selectedItem;
            set => Set(ref _selectedItem, value);
        }

        public ObservableCollection<SampleCompany> SampleItems { get; } = new ObservableCollection<SampleCompany>();

        public ICommand ItemInvokedCommand => _itemInvokedCommand ?? (_itemInvokedCommand = new RelayCommand<WinUI.TreeViewItemInvokedEventArgs>(OnItemInvoked));

        public FilamentViewModel()
        {
        }

        public async Task LoadDataAsync()
        {
            var data = await SampleDataService.GetTreeViewDataAsync();
            foreach (var item in data)
            {
                SampleItems.Add(item);
            }
        }

        private void OnItemInvoked(WinUI.TreeViewItemInvokedEventArgs args)
            => SelectedItem = args.InvokedItem;
    }
}
