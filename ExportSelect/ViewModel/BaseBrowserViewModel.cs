using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ExportSelect.ViewModel;
public enum InitializationResult
{
    Success,
    Failure,
    InitializedWithErrors,
    NotImplemented
}
public abstract class BaseBrowserViewModel:ObservableObject
{
    public abstract Guid Signature { get;
    }
    public CollectionViewSource ViewSource { get; } = new CollectionViewSource();
    public abstract InitializationResult InitViewSource();
    public BaseBrowserViewModel()=> InitViewSource();
}
