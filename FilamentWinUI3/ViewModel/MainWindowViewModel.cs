using DataDefinitions;
using DataDefinitions.Interfaces;
using DataDefinitions.LiteDBSupport;
using DataDefinitions.Models;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilamentWinUI3.ViewModel;

public class MainWindowViewModel:Observable
{
    private Guid tagCollateSignature= Guid.Empty;
    private bool supportTagFilter;

    public bool SupportTagFilter
    {
        get => supportTagFilter;
        set => Set(ref supportTagFilter, value);
    }

    internal void UpdateFilterStatus(IFilterStatus filterStatus)
    {
        SupportTagFilter = filterStatus.IsFilterSupported(IResolveFilter.Filters.Tag);
    }
    internal void InitializeTagCollate(ITagCollate tagCollate)
    {
        if(tagCollateSignature != tagCollate.Signature)
        {
            tagCollateSignature = tagCollate.Signature;
            
        }
    }
}
