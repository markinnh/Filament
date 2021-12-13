using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Filament_Data.JsonModel;

namespace Filament.WPF.ViewModel
{
    [ViewModelDescriptor(Category ="Definitions",Description ="Define a vendor's spool",Title ="Spool")]
    public class SpoolDefineViewModel : BaseViewModel<SpoolDefinition>
    {
        protected override void SaveNew()
        {
            throw new NotImplementedException();
        }
    }
}
