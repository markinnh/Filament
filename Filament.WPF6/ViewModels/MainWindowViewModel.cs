using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataDefinitions;
using Microsoft.Toolkit.Mvvm.Messaging;

namespace Filament.WPF6.ViewModels
{
    public enum InventoryDisplayStyle { Intuitive, Simple, Form }
    public class MainWindowViewModel : Observable
    {
        // TODO: Add attribution to FlatIcon.com for all the special icons used in the UX
        // TODO: Add support for a third inventory page type more of a list view and a form for creating new measurements.
        #region Settings Exposed to UI
        //private bool useSwissArmyKnifeUI;
        public bool UseSwissArmyKnifeUI
        {
            get => Enum.Parse<InventoryDisplayStyle>(Properties.Settings.Default.CurrentInventoryDisplayStyle)== InventoryDisplayStyle.Intuitive;
            //set
            //{
            //    if(Properties.Settings.Default.UseSwissArmyKnifeUI != value)
            //    {
            //        Properties.Settings.Default.UseSwissArmyKnifeUI = value;
            //        Properties.Settings.Default.Save();
            //        OnPropertyChanged(nameof(UseSwissArmyKnifeUI));
            //    }
            //}
        }
        public bool UseSimpleUI => Enum.Parse<InventoryDisplayStyle>(Properties.Settings.Default.CurrentInventoryDisplayStyle)==InventoryDisplayStyle.Simple;
        public bool UseFormUI => Enum.Parse<InventoryDisplayStyle>(Properties.Settings.Default.CurrentInventoryDisplayStyle) == InventoryDisplayStyle.Form;
        public int SelectedTabIndex
        {
            get => Properties.Settings.Default.SelectedTabIndex;
            set => Properties.Settings.Default.SelectedTabIndex = value;
        }
        #endregion
        public MainWindowViewModel()
        {
            WeakReferenceMessenger.Default.Register<NotifyContainerEventArgs>(this, HandleNotifyContainer);
        }

        private void HandleNotifyContainer(object recipient, NotifyContainerEventArgs message)
        {
            System.Diagnostics.Debug.Assert(message != null, "Message object is required.");

            if (message != null)
            {
                foreach (var element in message.Elements)
                    OnPropertyChanged(element);
            }
        }
    }
}
