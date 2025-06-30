using DataDefinitions.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Filament.WPF6.Helpers;

public class WindowsFilter : IWindowsFilter
{
    private bool _applied;
    public bool Applied { get => _applied; set => _applied = value; }

    public IResolveFilter Resolve { get; set; }

    public WindowsFilter(IResolveFilter resolve) => Resolve = resolve;
    public void Filter(object sender, FilterEventArgs e)
    {
        if (Resolve != null)
        {
            e.Accepted = Resolve?.SatisfiedCriteria(e.Item) ?? true;
        }
        else
            e.Accepted = true;
    }
}
