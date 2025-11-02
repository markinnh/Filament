using Filament.WPF6.Helpers;
using DataDefinitions;
//using DataContext;
using DataDefinitions.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
//using DAL;
using System.Windows.Controls;
using DataDefinitions.JsonSupport;
using System.Collections.ObjectModel;
using DataDefinitions.LiteDBSupport;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using DataDefinitions.Filters;
using DataDefinitions.Interfaces;

namespace Filament.WPF6.ViewModels;

public partial class TVendorDefnViewModel : BaseTagFilterViewModel<VendorDefn, VendorDefn>, IExportUI //BaseBrowserViewModel<VendorDefn, VendorDefn>
{
    public override Guid Signature => Singleton<LiteDBDal>.Instance.Vendors.Signature;
    readonly DataGridLengthConverter dglConvert = new();
    #region ExportUI State, Feedback and Operations
    public ImportExportUIState ImportExportUIState { get; } = new ImportExportUIState(supportsSingleExport: true, supportsMultiExport: true, supportsImport: false);
    public void ExportChecked()
    {
        var items = ViewSource.View.Cast<VendorDefn>().Where(v => v.IsSelectedForSharing).ToList();
        var suggestedExportFileName = $"{items.Count} Selected_Vendor_Definitions - {DateTime.Now:yyyyMMddHHmmss}.json";
        var initDir = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), Properties.Settings.Default.DataPath, Properties.Settings.Default.ExchangePath, "Vendors");
        var exportFileName = string.Empty;
        if (items.Count > 1)
        {
            try
            {
                exportFileName = JsonFileSupport.GetExportJsonFilename(suggestedExportFileName, initDir);
                if (!string.IsNullOrEmpty(exportFileName))
                    JsonFileSupport.ExportManyToGJsonPackage(exportFileName, items);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error exporting all vendor definitions to {exportFileName}\n{ex.Message}", "Export Vendor Definitions", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }
    }
    public void ExportSelected()
    {
        if (SelectedItem != null && SelectedItem is VendorDefn defn)
        {
            var safefileName = string.Concat(defn.Name.Split(System.IO.Path.GetInvalidFileNameChars()));
            var initDir = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), Properties.Settings.Default.DataPath, Properties.Settings.Default.ExchangePath, "Vendors");
            var exportFileName = string.Empty;
            try
            {
                exportFileName = JsonFileSupport.GetExportJsonFilename(safefileName, initDir);
                Debug.WriteLine(defn.SerializeForSharing());
                if (!string.IsNullOrEmpty(exportFileName))
                    JsonFileSupport.ExportToGJsonPackage(exportFileName, defn);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error exporting vendor definition {defn.Name} to \n{exportFileName}\n{ex.Message}", "Export Vendor Definition", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }
    }
    /*
    public void ImportObject()
    {
        var ofd = new Microsoft.Win32.OpenFileDialog()
        {
            Title = "Import A Vendor Definition",
            Filter = "JSON Files|*.json|All Files|*.*",
            InitialDirectory = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), Properties.Settings.Default.DataPath, Properties.Settings.Default.ExchangePath),
            FilterIndex = 0
        };
        if (ofd.ShowDialog() == true)
        {
            JsonFileSupport.ImportFromGJsonPackage<VendorDefn>(ofd.FileName, out var result);
#if DEBUG
            if (result is VendorDefn vdefn)
                Debug.WriteLine(vdefn?.GenerateCode());
            if (result is IEnumerable<VendorDefn> vdefns)
            {
                foreach (var v in vdefns)
                    Debug.WriteLine(v.GenerateCode());
            }
#endif
        }
    }*/
    #endregion
    private ICommand? webNavigate;
    public ICommand WebNavigate => webNavigate ??= new RelayCommand<object>(HandleWebNavigate);

    private void HandleWebNavigate(object? obj)
    {
        if (obj is Uri uri)
        {

            //var processStartInfo = new ProcessStartInfo("msedge.exe",uri.AbsoluteUri);
            Process.Start(new ProcessStartInfo("cmd", $"/c start {uri.AbsoluteUri}") { CreateNoWindow = true });
        }
        //throw new NotImplementedException();
    }
    private ICommand? updateExportUICommand;
    public ICommand UpdateExportUICommand => updateExportUICommand ??= new RelayCommand(HandleUpdateExportUI);
    private void HandleUpdateExportUI()
    {
        ImportExportUIState.UpdateCanExportState(SelectedItem != null, ViewSource.View.Cast<VendorDefn>().Count(v => v.IsSelectedForSharing) > 0);
    }
    public bool ListHasModifiedItems => ((IEnumerable<VendorDefn>)ViewSource.Source).Count(i => i.IsModified) > 0;
    //protected override void PrepareForDataOperations()
    //{
    //    VendorDefn.InDataOps = true;
    //    SpoolDefn.InDataOps = true;
    //}

    //protected override void FinishedDataOperations()
    //{
    //    VendorDefn.InDataOps = false;
    //    SpoolDefn.InDataOps = false;
    //}
    [RelayCommand]
    private void ContextExportChecked()
    {
        ExportChecked();
    }
    [RelayCommand]
    private void ContextExportSelected()
    {
        ExportSelected();
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

    protected override void DerivedInitItems()
    {

        //PrepareForDataOperations();
#pragma warning disable CS8604 // Possible null reference argument.
        ViewSource.Source = Singleton<LiteDBDal>.Instance.Vendors;
        //InitItems(GetAllItems());
#pragma warning restore CS8604
        // Possible null reference argument.
        //var defns = FilamentContext.GetAllVendors();
        //if (defns != null)
        //{
        //    InitItems(defns);
        //}
        //FinishedDataOperations();
    }
    //protected override IEnumerable<VendorDefn>? GetAllItems() => throw new NotSupportedException();
    //protected override IEnumerable<VendorDefn>? GetInUseItems() => throw new NotSupportedException();

    //protected override IEnumerable<VendorDefn>? GetFilteredItems(Func<VendorDefn, bool> predicate) => throw new NotSupportedException();
    public IEnumerable<FilamentDefn> Filaments => Singleton<LiteDBDal>.Instance.Filaments;

    public string DescriptionColumnWidth
    {
        get => Properties.Settings.Default.DescriptionColumnWidth;
        set => Properties.Settings.Default.DescriptionColumnWidth = dglConvert.ConvertToString(value);
    }
    //protected override void UpdateSelectedItemHander()
    //{
    //    if (SelectedItem != null)
    //    {
    //        //var needAdd = !SelectedItem.InDatabase;

    //        //DAL.Abstraction.UpdateItem(SelectedItem);
    //        SelectedItem.UpdateItem(Singleton<LiteDBDal>.Instance);
    //        //if (Items?.Count(i => i.VendorDefnId == SelectedItem.VendorDefnId) == 0 && needAdd)
    //        //    Items.Add(SelectedItem);

    //        //if (needAdd)
    //        //    Singleton<LiteDBDal>.Instance.Add(SelectedItem);

    //    }
    //}
    protected override void PostItemsInitialization()
    {
        //if (Items != null)
        //    SelectedItem = Items.First();
        if (ViewSource.View.CurrentItem is VendorDefn defn)
            SelectedItem = defn;
    }
    public TVendorDefnViewModel()
    {
        WeakReferenceMessenger.Default.Register<ExchangeActionEventArgs>(this, (r, m) =>
        {
            if (m.Action == ExchangeActions.ExportAllChecked)
                ExportChecked();
            else if (m.Action == ExchangeActions.ExportSelected)
                ExportSelected();
            else if (m.Action == ExchangeActions.CheckAllInView)
                ContextCheckAll();
            else if (m.Action == ExchangeActions.UnCheckAllInView)
                ContextUncheckAll();
        });
        HandleUpdateExportUI();
        //WeakReferenceMessenger.Default.Register<Helpers.VendorDefnListChanged>(this, HandleVendorDefnCollectionChanged);
    }
    public override void ReleaseEventHandlers()
    {
        base.ReleaseEventHandlers();
        WeakReferenceMessenger.Default.Unregister<ExchangeActionEventArgs>(this);
    }
    ~TVendorDefnViewModel()
    {
        //WeakReferenceMessenger.Default.Unregister<Helpers.VendorDefnListChanged>(this);
    }
    private void HandleVendorDefnCollectionChanged(object recipient, VendorDefnListChanged message)
    {
        //if (message.Value != null)
        //{
        //    Items = message.Value;
        //    SelectedItem = Items.FirstOrDefault();
        //}
        //throw new NotImplementedException();
    }

    //        protected override void InitFilterViewModel()
    //        {
    //            Signature = Singleton<LiteDBDal>.Instance.Vendors.Signature;
    //#if DEBUG
    //            filterSupported[(int)IResolveFilter.Filters.Tag] = filters.TryAdd(IResolveFilter.Filters.Tag, new WindowsFilter(new TagResolve() { Signature = Signature }));
    //#else
    //            filterSupported[(int)IResolveFilter.Filters.Tag] =filters.TryAdd(IResolveFilter.Filters.Tag, new WindowsFilter( new TagResolve() { Signature = Signature }));
    //#endif
    //            WeakReferenceMessenger.Default.Register<TagFilterChangedEventArgs>(this, HandleTagFilterMessages);
    //        }
}
