using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataDefinitions;
using DataDefinitions.LiteDBSupport;
using DataDefinitions.Models;

namespace Filament.WPF6.ViewModels;
public class MaterialsViewModel : BaseBrowserViewModel<Material, Material>
{
    protected override bool SupportsShowAllFiltering => false;
    protected override void DerivedInitItems()
    {
        ViewSource.Source = Singleton<LiteDBDal>.Instance.Materials;
    }
}
