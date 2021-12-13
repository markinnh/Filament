using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filament.WPF.ViewModel
{
    interface IViewModel
    {
        string Title { get; }
        string Description { get; }
        string Category { get; }
        ViewModelDescriptorAttribute ModelAttribute { get; }
    }
}
