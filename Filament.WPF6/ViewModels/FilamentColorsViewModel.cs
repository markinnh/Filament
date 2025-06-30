using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using DataDefinitions;
using DataDefinitions.LiteDBSupport;
using DataDefinitions.Models;

namespace Filament.WPF6.ViewModels;
public partial class FilamentColorsViewModel : BaseBrowserViewModel<FilamentColor, FilamentColor>
{
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
    

    protected override void DerivedInitItems()
    {
        currentSortDescription = GetSortDirectionDescription();
        ViewSource.Source = Singleton<LiteDBDal>.Instance.FilamentColors;
        ViewSource.SortDescriptions.Add(currentSortDescription);
    }
    [RelayCommand]
    protected void DoSwitchSortDirection(object p)
    {
        ViewSource.SortDescriptions.Remove(currentSortDescription);

        sortDirection = SwitchSortDirection();

        currentSortDescription = GetSortDirectionDescription();

        ViewSource.SortDescriptions.Add(currentSortDescription);
        if (p is bool b)
            SortAscending = b;
    }
    [RelayCommand]
    protected void DoLookup(object l)
    {
        if (l is string lookup && !string.IsNullOrEmpty(lookup) && Regex.IsMatch(lookup, "#[a-fA-F0-9]{2,8}"))
            SelectedItem = Singleton<LiteDBDal>.Instance.FilamentColors.Where(fc => fc.ColorRGB == lookup).FirstOrDefault();
    }
    protected ListSortDirection SwitchSortDirection() => sortDirection == ListSortDirection.Ascending ? ListSortDirection.Descending : ListSortDirection.Ascending;
    protected SortDescription GetSortDirectionDescription() => new("ColorRGB", sortDirection);


}
