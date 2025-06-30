using DataDefinitions;
using DataDefinitions.Interfaces;
using FilamentWinUI3.View;
using FilamentWinUI3.ViewModel;
using FilamentWinUI3.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Syncfusion.UI.Xaml.Data;
using System;
using System.Linq;
using System.Reflection;

namespace FilamentWinUI3;

public sealed partial class MainWindow : Window
{

    public MainWindowViewModel ViewModel { get; set; } = Singleton<MainWindowViewModel>.Instance;
    public MainWindow()
    {
        this.InitializeComponent();
    }

    private void NavigationView_ItemInvoked(Microsoft.UI.Xaml.Controls.NavigationView sender, Microsoft.UI.Xaml.Controls.NavigationViewItemInvokedEventArgs args)
    {
        if (args.InvokedItemContainer is NavigationViewItem item) { 
            if (item.Content is string str)
            {
                switch (str)
                {
                    case "Vendor":
                        ContentFrame.Navigate(typeof(VendorDefnPage));
                        break;
                    case "Filament":
                        ContentFrame.Navigate(typeof(FilamentDefnPage));
                        break;
                    case "Notes":
                        break;
                    case "Print Settings":
                        break;
                }
            }
            else
                System.Diagnostics.Debug.WriteLine(item.Content);
        }
    }

    private void ContentFrame_Navigated(object sender, Microsoft.UI.Xaml.Navigation.NavigationEventArgs e)
    {
        if (e.Content.GetType().GetProperty("ViewModel") is PropertyInfo property)
        {
            if(property.GetValue(e.Content, null) is IFilterStatus filterStatus) 
                ViewModel.UpdateFilterStatus(filterStatus);
            // TODO: Add code to update the tag list if the ViewModel is based on ITagCollate
            if(property.GetValue(e.Content,null) is ITagCollate tagCollate)
                ViewModel.InitializeTagCollate(tagCollate);
            System.Diagnostics.Debug.WriteLine(e.Content.GetType().FullName);
        }

    }
}
