using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using DataDefinitions;

namespace Filament.WPF6.ViewModels
{
    public class SettingsViewModel : Observable
    {
        private InventoryDisplayStyle inventoryDisplayStyle;

        public InventoryDisplayStyle InventoryDisplayStyle
        {
            get => Enum.Parse<InventoryDisplayStyle>(Properties.Settings.Default.CurrentInventoryDisplayStyle);
            set
            {
                if(Enum.TryParse<InventoryDisplayStyle>(Properties.Settings.Default.CurrentInventoryDisplayStyle,out InventoryDisplayStyle inventoryDisplayStyle))
                {
                    if (inventoryDisplayStyle != value)
                    {
                        Properties.Settings.Default.CurrentInventoryDisplayStyle = value.ToString();
                        OnPropertyChanged(nameof(InventoryDisplayStyle));
                        //TODO: Determine UI Members on MainWindow that are affected probably as below
                        WeakReferenceMessenger.Default.Send<NotifyContainerEventArgs>(new NotifyContainerEventArgs(new string[]
                        {
                            nameof(MainWindowViewModel.UseSwissArmyKnifeUI),
                            nameof(MainWindowViewModel.UseSimpleUI),
                            nameof(MainWindowViewModel.UseFormUI)  
                        }));
                    }
                }
            }
        }

        public bool UseSwissArmyKnifeUI
        {
            get => Properties.Settings.Default.UseSwissArmyKnifeUI;
            set
            {
                if (Properties.Settings.Default.UseSwissArmyKnifeUI != value)
                {
                    Properties.Settings.Default.UseSwissArmyKnifeUI = value;
                    //Properties.Settings.Default.Save();
                    OnPropertyChanged(nameof(UseSwissArmyKnifeUI));
                    WeakReferenceMessenger.Default.Send<NotifyContainerEventArgs>(new NotifyContainerEventArgs(new string[] { nameof(UseSwissArmyKnifeUI) }));
                }
            }
        }
        private ICommand? handleFlatIconsClick;
        public ICommand? HandleFlatIconsClick => handleFlatIconsClick ??= new RelayCommand(() =>
        {

            if (Uri.TryCreate("https://www.flaticon.com", UriKind.Absolute, out Uri? result))
                Process.Start(new ProcessStartInfo("cmd", $"/c start {result.AbsoluteUri}") { CreateNoWindow = true });

        });
    }
}
