using Filament.WPF6.Helpers;
using DataDefinitions.Models;

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
using DataDefinitions.JsonSupport;
using CommunityToolkit.Mvvm.Messaging;
using Filament.WPF6.Properties;
using System.Diagnostics;
using Filament.WPF6.Pages;
using System.Collections.ObjectModel;
using Filament.WPF6.ViewModels;
using DataDefinitions.Interfaces;

namespace Filament.WPF6
{
    public enum TagInteraction
    {
        TagUpdated
    }
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MenuItem? lastShowStateChecked;
        private MenuItem? lastPageSelected;
        public MainWindow()
        {
            InitializeComponent();
            // TODO : Figure out a way to initialize the JsonFilamentDocument
            //var showFlagSetting = Singleton<>.Instance.GetSingleSetting(s => s.Name == nameof(SelectShowFlag));

            //if (showFlagSetting != null)
            //    SelectShowFlag.SelectedItem = Enum.Parse<ShowAllFlag>(showFlagSetting.Value);
            //else
            //    SelectShowFlag.SelectedItem = ShowAllFlag.ShowAll;
            //SelectShowFlag.SelectedItem = Enum.Parse<ShowAllFlag>(Settings.Default.ShowStateFlag);
            if (FindName(Settings.Default.ShowStateFlag) is MenuItem menuItem)
            {
                menuItem.IsChecked = true;
                lastShowStateChecked = menuItem;
                SetFilter(Settings.Default.ShowStateFlag);
            }
            if (FindName(Settings.Default.LastPageVisited) is MenuItem menuItem1)
            {

                menuItem1.IsChecked = true;
                lastPageSelected = menuItem1;
                if (menuItem1.Tag is string str)
                    ViewFrame.Navigate(new Uri(str, UriKind.RelativeOrAbsolute));
            }
            WeakReferenceMessenger.Default.Register<TagInteractionNotification>(this, HandleTagMessage);
        }

        private void HandleTagMessage(object recipient, TagInteractionNotification message)
        {
            Debug.WriteLine($"Tag interaction received, notification - {message.Value}");
            if (message.Value== TagInteraction.TagUpdated && ViewFrame.Content is Page page && page.DataContext is ITagCollate tagCollate && DataContext is MainWindowViewModel main)
                main.UpdateTagList(tagCollate);
            //throw new NotImplementedException();
        }

        

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox cmb)
            {
                if (cmb.SelectedItem is ShowAllFlag flag)
                {

                    WeakReferenceMessenger.Default.Send(new ShowAllFlagChanged(flag));
                    if (Settings.Default.ShowStateFlag != flag.ToString())
                        Settings.Default.ShowStateFlag = flag.ToString();

                    //if (Singleton<JsonDAL>.Instance.Document.Settings.FirstOrDefault(s => s.Name == nameof(SelectShowFlag)) is Setting setting)
                    //{
                    //    setting.SetValue(flag);
                    //    setting.UpdateItem();
                    //}
                    //else
                    //{
                    //    var createSetting = new Setting(nameof(SelectShowFlag), flag);
                    //    //createSetting.EstablishLink(Singleton<JsonDAL>.Instance.Document);
                    //    createSetting.UpdateItem();
                    //    //DAL.Abstraction.UpdateItem(createSetting);
                    //    //Singleton<DAL.DataLayer>.Instance.Add(createSetting);
                    //}
                }
            }
        }

        private void ShowState_Click(object sender, RoutedEventArgs e)
        {
            if (lastShowStateChecked is MenuItem item)
                item.IsChecked = false;
            if (sender is MenuItem sentMenu)
            {
                sentMenu.IsChecked = true;
                lastShowStateChecked = sentMenu;
                SetFilter(sentMenu.Name);
            }

        }
        private void SetFilter(string state)
        {
            var flag = Enum.Parse<ShowAllFlag>(state);
            WeakReferenceMessenger.Default.Send(new ShowAllFlagChanged(flag));
            if (Settings.Default.ShowStateFlag != state)
                Settings.Default.ShowStateFlag = state;
        }

        private void SelectPage_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menu)
            {
                Debug.WriteLine($"Menu selected - {menu.Name}");
                if (lastPageSelected != null)
                    lastPageSelected.IsChecked = false;

                lastPageSelected = menu;
                menu.IsChecked = true;
                if (menu.Tag is string str)
                {
                    Debug.WriteLine($"Navigate to : {str}");
                    ViewFrame.Navigate(new Uri(str, UriKind.RelativeOrAbsolute));
                    Settings.Default.LastPageVisited = menu.Name;
                }
            }
        }

        private void ViewFrame_Navigated(object sender, NavigationEventArgs e)
        {
            Debug.WriteLine($"Navigated to {e.Content.GetType().Name}");
            //this all has to go to the MainWindowViewModel
            // if DataContext is MainWindowViewModel then set child page member, handle all the other issues in the MainWindowViewModel
            if (e.Content is Page page && page.DataContext is ITagCollate tagCollation && DataContext is MainWindowViewModel main)
                main.SetTagList(tagCollation);
            //{
            //    Debug.WriteLine($"tag collation count {tagCollation.TagStats.Count()}");
            //    if (tagCollation.TagStats != null)
            //    {
            //        CheckableTags.Clear();
            //        foreach (var ele in from t in tagCollation.TagStats select new CheckableTagStat(false, t))
            //            CheckableTags.Add(ele);
            //    }

            //}
            //else
            //    CheckableTags.Clear();
        }

        private void ViewFrame_Navigating(object sender, NavigatingCancelEventArgs e)
        {

        }

    }
}
