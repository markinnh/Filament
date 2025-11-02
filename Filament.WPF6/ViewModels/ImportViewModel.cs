using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using DataDefinitions;
using DataDefinitions.Interfaces;
using DataDefinitions.LiteDBSupport;
using Filament.WPF6.Helpers;

namespace Filament.WPF6.ViewModels;

public partial class ImportViewModel:IViewModel
{
    public Guid ViewModelGuid
    {
        get; } = Guid.NewGuid();
    public void ReleaseEventHandlers()  // TODO: implement if required, currently, no filters so no handlers
    {
    }
    public virtual void InitFilterUI()
    {
        UpdateFilterUI();// overridden where filters are used
        //throw new NotImplementedException();
    }
    protected void UpdateFilterUI(bool TagFilterEnabled = false, bool DateFilterEnabled = false, bool KeywordFilterEnabled = false)
    {
        var args = new FilterUIChangedEventArgs(TagFilterEnabled, DateFilterEnabled, KeywordFilterEnabled);
        WeakReferenceMessenger.Default.Send(args);
    }
    public CollectionViewSource FilesViewSource
    {
        get; set;
    }

    ObservableCollection<object> _files;
    public ImportViewModel()
    {
        _files = new ObservableCollection<object>();
        FilesViewSource = new CollectionViewSource { Source = _files };
    }
    internal void ProcessFiles(IEnumerable<string> files)
    {
        foreach (var file in files)
        {
            Trace.WriteLine($"Opening {file}");
            try
            {
                JsonFileSupport.ImportFromGJsonPackage<object>(file, out var result);
                if (result != null && result is not IEnumerable<object>)
                {
                    if (result is ISharingSupport sharing)
                        Singleton<LiteDBDal>.Instance.UpdateInDatabaseFlag(sharing);

                    _files.Add(result);
                }
                else if (result is IEnumerable<object> list)
                    foreach (var item in list)
                    {
                        if (item is ISharingSupport sharing)
                            Singleton<LiteDBDal>.Instance.UpdateInDatabaseFlag(sharing);

                        _files.Add(item);
                    }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error processing {file}\n Message {ex.Message}\n {ex.InnerException}", "Error during drag drop operations", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
    [RelayCommand]
    private void ImportObject(object obj)
    {
        if (obj is DatabaseObject dbObject)
        {
            Debug.WriteLine($"Simulating importing {dbObject.GetType().Name}");
        }
    }
}
public partial class DroppedFile:ObservableObject
{
    [ObservableProperty]
    private string _fullfilename;
    [ObservableProperty]
    private string _filename;
    public DroppedFile(string fullfilename)
    {
        Fullfilename = fullfilename;
        Filename = System.IO.Path.GetFileName(fullfilename);
    }
}
public class DroppedListTemplateSelector:DataTemplateSelector
{
    public DataTemplate? VendorDefnTemplate
    {
        get; set;
    }
    public DataTemplate? FilamentDefnTemplate
    {
        get; set;
    }
    public DataTemplate? DefaultTemplate
    {
        get; set;
    }
    public DataTemplate? FilamentColorTemplate
    {
        get; set;
    }
    public override DataTemplate? SelectTemplate(object item, DependencyObject container)
    {
        if (item is DataDefinitions.Models.VendorDefn)
            return VendorDefnTemplate;
        else if (item is DataDefinitions.Models.FilamentDefn)
            return FilamentDefnTemplate;
        else if (item is DataDefinitions.Models.FilamentColor)
            return FilamentColorTemplate;
        return DefaultTemplate;
    }
}
