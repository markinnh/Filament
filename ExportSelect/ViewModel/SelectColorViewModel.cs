using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using DataDefinitions;
using DataDefinitions.LiteDBSupport;
using DataDefinitions.Models;
using ExportSelect.Helpers;

namespace ExportSelect.ViewModel;
public partial class SelectColorViewModel : BaseBrowserViewModel
{
    private readonly Guid signature = Guid.NewGuid();
    public override Guid Signature => signature;
    
    [ObservableProperty]
    private int itemsChecked;
    public override InitializationResult InitViewSource()
    {
        // Initialize the view source with all filament colors
        ViewSource.Source = new ObservableCollection<SelectFilamentColorWrapper>(
            Singleton<LiteDBDal>.Instance.FilamentColors
                .Select(fc => new SelectFilamentColorWrapper(fc,true))
        );
        ItemChecked();
        WeakReferenceMessenger.Default.Register<InternalMessageEventArgs>(this, (r, m) =>
        {
            if (m.MessageType == InternalMessageType.ResetInternalState)
            {
                if (ViewSource.Source is ObservableCollection<SelectFilamentColorWrapper> items)
                {
                    foreach (var item in items)
                    {
                        item.IsSelected = true;
                    }
                    ItemChecked();
                }
            }
            else if (m.MessageType == InternalMessageType.ClearInternalState)
            {
                if (ViewSource.Source is ObservableCollection<SelectFilamentColorWrapper> items)
                {
                    foreach (var item in items)
                    {
                        item.IsSelected = false;
                    }
                    ItemChecked();
                }
            }
            else if (m.MessageType == InternalMessageType.GenerateCode)
            {
                // Generate the code for the selected colors
                if (ViewSource.Source is ObservableCollection<SelectFilamentColorWrapper> items)
                {
                    var selectedColors = items.Where(i => i.IsSelected).Select(i => i.Item).ToList();
                    var code = CodeGenerator.GenerateColorCode(selectedColors);
                    WeakReferenceMessenger.Default.Send(new InternalMessageEventArgs(InternalMessageType.GenerateCodeResults, code));
                }
            }
        });
        return InitializationResult.Success;
    }
    [RelayCommand]
    void ItemChecked()
    {

        if (ViewSource.Source is ObservableCollection<SelectFilamentColorWrapper> items)
            ItemsChecked = items.Count(i => i.IsSelected);
    }
}
