using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using DataDefinitions;
using DataDefinitions.LiteDBSupport;
using DataDefinitions.Models;
using Filament.WPF6.Helpers;
using Filament.WPF6.Properties;

namespace Filament.WPF6.ViewModels;
public partial class FilamentColorsViewModel : BaseTagFilterViewModel<FilamentColor, FilamentColor>,IExportUI
{
    private readonly Guid signature = Guid.NewGuid();
    public override Guid Signature => signature;
    //public override bool CanDelete => base.CanDelete;

    public ImportExportUIState ImportExportUIState { get; } = new ImportExportUIState(supportsSingleExport: false, supportsMultiExport: true);
    protected override void Update()
    {
        // if we are adding a new item, check if the color already exists in the database
        if (InAddNew &&
            SelectedItem is FilamentColor fc &&
            Singleton<LiteDBDal>.Instance.FilamentColors.Where(z => z.ColorRGB == fc.ColorRGB && z.VendorColorName==fc.VendorColorName).FirstOrDefault() is FilamentColor filamentColor)
        {
            MessageBox.Show($"An entry with the vendor color name: {fc.VendorColorName} and RGB value: {fc.ColorRGB} already exists in the database. Please enter a different color name or RGB value.", "Duplicate Color Name And Value", MessageBoxButton.OK, MessageBoxImage.Warning);
            // TODO: revist this logic
            SelectedItem = filamentColor;
            InAddNew = false;
        }
        else
            base.Update();
    }
    protected override void DerivedInitItems()
    {
        sortDirection=Enum.Parse<ListSortDirection>(Settings.Default.FilamentColorPageSortDirection);
        currentSortDescription = GetSortDirectionDescription();
        ViewSource.Source = Singleton<LiteDBDal>.Instance.FilamentColors;
        ViewSource.SortDescriptions.Add(currentSortDescription);
        WeakReferenceMessenger.Default.Register<ExchangeActionEventArgs>(this, (r, m) =>
        {
                switch (m.Action)
                {
                    case ExchangeActions.ExportAllChecked:
                        ExportAllChecked();
                        break;
                    case ExchangeActions.CheckAllInView:
                        foreach (var item in ViewSource.View.Cast<FilamentColor>())
                            item.IsSelectedForSharing = true;
                        break;
                    case ExchangeActions.UnCheckAllInView:
                        foreach (var item in ViewSource.View.Cast<FilamentColor>())
                            item.IsSelectedForSharing = false;
                        break;
            }
        });
    }
    public void ExportAllChecked()
    {
        var items = ViewSource.View.Cast<FilamentColor>().Where(fc => fc.IsSelectedForSharing).ToList();
        var exportFilename=string.Empty;
        if (items.Count > 0)
        {
            //var sfd = new Microsoft.Win32.SaveFileDialog
            //{
            //    Title = "Export Selected Filament Colors",
            //    Filter = "JSON Files|*.json|All Files|*.*",
            //    FileName = $"{items.Count} Selected_Filament_Colors - {DateTime.Now:yyyyMMddHHmmss}.json",
            //    InitialDirectory = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), Properties.Settings.Default.DataPath, Properties.Settings.Default.ExchangePath)
            //};
            //if (sfd.ShowDialog() == true)
            //{
                try
                {
                    exportFilename = JsonFileSupport.GetExportJsonFilename(suggestedName: $"{items.Count} Selected_Filament_Colors - {DateTime.Now:yyyyMMddHHmmss}.json",
                    startingPath: System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), Properties.Settings.Default.DataPath, Properties.Settings.Default.ExchangePath,"Colors"));
                    JsonFileSupport.ExportManyToGJsonPackage(exportFilename, items);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error exporting all filament colors to {exportFilename} \n{ex.Message}", "Export Filament Colors", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            //}
        }
    }
    #region Sorting
    private bool sortAscending = true;

    public bool SortAscending
    {
        get => sortAscending;
        set => Set(ref sortAscending, value);
    }
    protected ListSortDirection sortDirection = ListSortDirection.Ascending;
    protected SortDescription currentSortDescription
    {
        get; private set;
    }
    [RelayCommand]
    protected void ToggleSortDirection(object p)
    {
        ViewSource.SortDescriptions.Remove(currentSortDescription);

        sortDirection = SwitchSortDirection();
        Settings.Default.FilamentColorPageSortDirection = sortDirection.ToString();
        currentSortDescription = GetSortDirectionDescription();

        ViewSource.SortDescriptions.Add(currentSortDescription);
        if (p is bool b)
            SortAscending = b;
    }
    protected ListSortDirection SwitchSortDirection() => sortDirection == ListSortDirection.Ascending ? ListSortDirection.Descending : ListSortDirection.Ascending;
    protected SortDescription GetSortDirectionDescription() => new("ColorRGB", sortDirection);
    #endregion

    [RelayCommand]
    protected void DoLookup(object l)
    {
        if (l is string lookup && !string.IsNullOrEmpty(lookup) && Regex.IsMatch(lookup, "#FF[a-fA-F0-9]{6}"))
            SelectedItem = Singleton<LiteDBDal>.Instance.FilamentColors.Where(fc => fc.ColorRGB == lookup).FirstOrDefault();
    }
    private void HandleUpdateExportUI()
    {
        ImportExportUIState.UpdateCanExportState(false, ViewSource.View.Cast<FilamentColor>().Count(v => v.IsSelectedForSharing) > 0);
    }
    [RelayCommand]
    protected void UpdateUIForChecked()
    {
    }
    [RelayCommand]
    private void ContextExportChecked()
    {
        ExportAllChecked();
    }
    [RelayCommand]
    private void ContextCheckAll()
    {
        foreach (var item in ViewSource.View.Cast<VendorDefn>())
            item.IsSelectedForSharing = true;
        ViewSource.View.Refresh();
        HandleUpdateExportUI();
    }
    [RelayCommand]
    private void ContextUncheckAll()
    {
        foreach (var item in ViewSource.View.Cast<VendorDefn>())
            item.IsSelectedForSharing = false;
        ViewSource.View.Refresh();
        HandleUpdateExportUI();
    }
}
