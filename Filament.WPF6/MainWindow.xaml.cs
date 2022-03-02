using Filament.WPF6.Helpers;
using DataDefinitions.Models;
using DataContext;
using Microsoft.Toolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Filament.WPF6
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var showFlagSetting = Singleton<DAL.DataLayer>.Instance.GetSingleSetting(s => s.Name == nameof(SelectShowFlag));

            if (showFlagSetting != null)
                SelectShowFlag.SelectedItem = Enum.Parse<ShowAllFlag>(showFlagSetting.Value);
            else
                SelectShowFlag.SelectedItem = ShowAllFlag.ShowAll;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox cmb)
            {
                if (cmb.SelectedItem is ShowAllFlag flag)
                {
                    WeakReferenceMessenger.Default.Send(new ShowAllFlagChanged(flag));
                    if (Singleton<DAL.DataLayer>.Instance.GetSingleSetting(s => s.Name == nameof(SelectShowFlag)) is Setting setting)
                    {
                        setting.SetValue(flag);
                        DAL.Abstraction.UpdateItem(setting);
                    }
                    else
                    {
                        var createSetting = new Setting(nameof(SelectShowFlag), flag);
                        //createSetting.UpdateItem<FilamentContext>();
                        DAL.Abstraction.UpdateItem(createSetting);
                        Singleton<DAL.DataLayer>.Instance.Add(createSetting);
                    }
                }
            }
        }
    }
}
