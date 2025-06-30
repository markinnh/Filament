using CommunityToolkit.Mvvm.Input;
using DataDefinitions;
using DataDefinitions.LiteDBSupport;
using DataDefinitions.Models;
using FilamentWinUI3.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FilamentWinUI3.ViewModel;

public class FilamentDefnViewModel : BaseBrowserViewModel<FilamentDefn, FilamentDefn>
{
    protected override IList Data => Singleton<LiteDBDal>.Instance.Filaments;
    internal override bool SupportsLiveFiltering =>false;
    public ObservableCollection<MaterialType> MaterialTypes { get; set; }= new ObservableCollection<MaterialType>(Enum.GetValues<MaterialType>());
    public ObservableCollection<double> SupportedDiameters { get; set; } = new ObservableCollection<double>(new double[] { 1.75, 3.00 });
    protected override void DerivedInitItems()
    {

        //throw new NotImplementedException();
    }

    protected override void FinishedDataOperations()
    {
        //throw new NotImplementedException();
    }

    protected override void PrepareForDataOperations()
    {
        //throw new NotImplementedException();
    }
}
