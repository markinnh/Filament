using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filament.WPF6.Helpers;

internal class FilterUIChangedEventArgs:EventArgs
{
    public bool TagFilterEnabled
    {
        get;
        set;
    }
    public bool DateFilterEnabled
    {
        get;
        set;
    }
    public bool KeywordFilterEnabled
    {
        get;
        set;
    }
    public FilterUIChangedEventArgs(bool tagFilterEnabled, bool dateFilterEnabled=false, bool keywordFilterEnabled=false)
    {
        TagFilterEnabled = tagFilterEnabled;
        DateFilterEnabled = dateFilterEnabled;
        KeywordFilterEnabled = keywordFilterEnabled;
    }
}
