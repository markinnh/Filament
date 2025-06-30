using CommunityToolkit.Mvvm.ComponentModel;
using DataDefinitions;
using DataDefinitions.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filament.WPF6;

/// <summary>
/// Checkable Item, for UI consumption
/// </summary>
/// <typeparam name="T">Observable type</typeparam>
/// <remarks>
/// requires an Observable type to prevent memory leaks
/// </remarks>
public partial class CheckableItem<T> : ObservableObject where T : Observable
{
    [ObservableProperty]
    private bool isChecked;

    [ObservableProperty]
    private T? item;

    public CheckableItem(bool isChecked, T item)
    {
        IsChecked = isChecked;
        Item = item;
    }
}
public class CheckableWordStat : CheckableItem<WordWithOccuranceCount>
{
    public CheckableWordStat(bool isChecked, WordWithOccuranceCount tagStat) : base(isChecked, tagStat) { }
}
