using CommunityToolkit.Mvvm.Input;
using DataDefinitions;
using DataDefinitions.LiteDBSupport;
using DataDefinitions.Models;
using FilamentWinUI3.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FilamentWinUI3.ViewModel;

public class VendorDefnViewModel : BaseBrowserViewModel<VendorDefn, VendorDefn>
{
    protected override IList Data => Singleton<LiteDBDal>.Instance.Vendors;
    internal override bool SupportsLiveFiltering => false;

    private ICommand? webNavigate;
    public ICommand WebNavigate => webNavigate ??= new RelayCommand<object>(HandleWebNavigate);

    private void HandleWebNavigate(object? obj)
    {
        if (obj is Uri uri)
        {

            //var processStartInfo = new ProcessStartInfo("msedge.exe",uri.AbsoluteUri);
            Process.Start(new ProcessStartInfo("cmd", $"/c start {uri.AbsoluteUri}") { CreateNoWindow = true });
        }
        //throw new NotImplementedException();
    }

    protected override void DerivedInitItems()
    {
        // This is where messaging should be established for consumer classes
        
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
