using Filament.WPF6.Helpers;
using DataDefinitions;

using DataDefinitions.Models;
//using DataContext;
//using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using DataDefinitions.JsonSupport;
using System.Collections.ObjectModel;
using System.Diagnostics;
using DataDefinitions.LiteDBSupport;
using CommunityToolkit.Mvvm.Input;
using System.Windows;
using CommunityToolkit.Mvvm.Messaging;
//using DAL;

namespace Filament.WPF6.ViewModels;

public partial class FilamentDefnViewModel : BaseBrowserViewModel<FilamentDefn, FilamentDefn>,IExportUI
{
    public ObservableCollection<Material> Materials
    {
        get;
    } = Singleton<LiteDBDal>.Instance.Materials;

    public ImportExportUIState ImportExportUIState => new (true,false);
    public ObservableCollection<VendorDefn> Vendors { get; set; }=Singleton<LiteDBDal>.Instance.Vendors;
    public FilamentDefnViewModel() : base()
    {
        WeakReferenceMessenger.Default.Register<ExchangeActionEventArgs>(this, (r, m) =>
        {
            if(m.Action == ExchangeActions.ExportSelected)
                ExportFilament();
        });
    }
    protected override void DerivedInitItems()
    {
        //PrepareForDataOperations();
        ViewSource.Source = Singleton<LiteDBDal>.Instance.Filaments;
        //if (Singleton<JsonDAL>.Instance.Document.Filaments is ObservableCollection<FilamentDefn> filaments)
        //{
        //    Items = filaments;
        //    //InitItems(filaments);
        //}
        //FinishedDataOperations();
    }
    [RelayCommand]
    private void ExportFilament()
    {
        /*
        if (SelectedItem != null)
        {
            var sfd = new Microsoft.Win32.SaveFileDialog
            {
                Title = "Export Filament Definition",
                Filter = "JSON Files|*.json|All Files|*.*",
                FileName = $"{SelectedItem.Name}.json",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
            };
            if (sfd.ShowDialog() == true)
            {
                try
                {
                    JsonFileSupport.WriteToJsonFile(sfd.FileName, SelectedItem);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error exporting filament definition to {sfd.FileName}\n{ex.Message}", "Export Filament Definition", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }*/
        var exportFilename = string.Empty;
        var suggestedFilename = SelectedItem != null ? $"{SelectedItem.Material.ShortName}{SelectedItem.Enhancement}{SelectedItem.Vendor?.Name} - {DateTime.Now:yyyyMMddHHmmss}.json" : $"Filament - {DateTime.Now:yyyyMMddHHmmss}.json";
        try
        {
            exportFilename = JsonFileSupport.GetExportJsonFilename(suggestedName: suggestedFilename,
            startingPath: System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), Properties.Settings.Default.DataPath, Properties.Settings.Default.ExchangePath, "Filaments"));
            if (SelectedItem != null)
                JsonFileSupport.ExportToGJsonPackage(exportFilename, SelectedItem);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error exporting filament definition to {exportFilename}\n{ex.Message}", "Export Filament Definition", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        Debug.WriteLine($"FilamentDefnViewModel ExportFilamentDefn called" );
        Debug.WriteLine(SelectedItem?.SerializeForSharing());
    }
    //protected override void FinishedDataOperations() => FilamentDefn.InDataOps = false;

    //protected override void PrepareForDataOperations() => FilamentDefn.InDataOps = true;

    //protected override IEnumerable<FilamentDefn>? GetAllItems() => throw new NotImplementedException();
    //protected override IEnumerable<FilamentDefn>? GetInUseItems() => throw new NotImplementedException();

    //protected override IEnumerable<FilamentDefn>? GetFilteredItems(Func<FilamentDefn, bool> predicate) =>
    //    throw new NotImplementedException();
    //protected override void ShowAllItems()
    //{
    //    if (FilamentContext.GetAllFilaments() is IEnumerable<FilamentDefn> filaments)
    //    {
    //        InitItems(filaments);
    //    }
    //}
    protected override void PostItemsInitialization()
    {
        //if (Items != null)
        //    SelectedItem = Items.First();
        // Watch the SelectedItem to update the Material.Enhancements list
        WeakEventManager<FilamentDefnViewModel, SelectedItemChangedEventArgs>.AddHandler(this, "SelectedItemChanged", handleSelectedItemChanged);
        if (ViewSource.View.CurrentItem is FilamentDefn filamentDefn)
            SelectedItem = filamentDefn;
            
    }

    private void handleSelectedItemChanged(object? sender, SelectedItemChangedEventArgs e) => OnPropertyChanged(nameof(Material.Enhancements));
    

    //protected void InitItems(IEnumerable<FilamentDefn> filaments)
    //{
    //    if (Items != null)
    //    {
    //        RemoveEventLinks();
    //        Items?.Clear();
    //        foreach (var filament in filaments)
    //        {
    //            Items?.Add(filament);
    //            filament?.InitNotificationHandler();
    //        }
    //    }
    //    else
    //    {
    //        Items = new System.Collections.ObjectModel.ObservableCollection<FilamentDefn>(filaments);
    //        foreach (var filament in filaments)
    //            filament.InitNotificationHandler();
    //    }
    //    UpdateEventLinks();
    //    SelectedItem = Items.First();
    //}
    //protected override void ShowInUseItems()
    //{
    //    if (FilamentContext.GetFilaments(f => !f.StopUsing) is IEnumerable<FilamentDefn> filaments)
    //    {
    //        InitItems(filaments);
    //    }

    //}
    /*
    protected override void UpdateSelectedItemHander()
    {
        if (SelectedItem != null)
        {
            //PrepareForDataOperations();
            SelectedItem.UpdateItem<FilamentContext>();
            //FilamentContext.UpdateSpec(SelectedItem);
            SelectedItem.SetContainedModifiedState(false);
            //FinishedDataOperations();
        }
    }*/
//#if DEBUG
//    public bool IsDebug => true;
//    private ICommand? prepopulate;

//    public ICommand? Prepopulate => prepopulate ??= new RelayCommand(HandlePrepopulateCommand);

//    private void HandlePrepopulateCommand()
//    {
//        //if (SelectedItem?.DensityAlias?.MeasuredDensity.Count == 0 && SelectedItem.MaterialType == MaterialType.PLA)
//        //{
//        //    Random random = Singleton<Random>.Instance;
//        //    const int minRandom = 990;
//        //    const int maxRandom = 1030;
//        //    SelectedItem?.DensityAlias?.MeasuredDensity.Add(new MeasuredDensity(2.98, random.Next(minRandom, maxRandom)));
//        //    SelectedItem?.DensityAlias?.MeasuredDensity.Add(new MeasuredDensity(2.99, random.Next(minRandom, maxRandom)));
//        //    SelectedItem?.DensityAlias?.MeasuredDensity.Add(new MeasuredDensity(3, random.Next(minRandom, maxRandom)));
//        //}
//    }
//#else
//    public bool IsDebug=> false;
//#endif    
}
