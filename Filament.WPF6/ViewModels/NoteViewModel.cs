using CommunityToolkit.Mvvm.Messaging;
using DataDefinitions;
using DataDefinitions.LiteDBSupport;
using DataDefinitions.Models;
using Filament.WPF6.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filament.WPF6.ViewModels
{
    public class NoteViewModel : BaseTagFilterViewModel<NoteDefn, NoteDefn>
    {
        protected override void DerivedInitItems()
        {
            ViewSource.Source = Singleton<LiteDBDal>.Instance.Notes;
            //throw new NotImplementedException();
        }

        protected override void FinishedDataOperations()
        {
            //throw new NotImplementedException();
        }

        protected override void InitFilterViewModel()
        {
            DistinctTagStats = Singleton<LiteDBDal>.Instance.Notes.DistinctTagStats;
            Signature = Singleton<LiteDBDal>.Instance.Notes.Signature;
#if DEBUG
            filters.Add(tagFilterKey, new TagFilter() { Signature = Signature, Owner = this });
#else
            filters.Add(tagFilterKey, new TagFilter() { Signature = Signature });
#endif

            WeakReferenceMessenger.Default.Register<TagFilterChangedEventArgs>(this, HandleTagFilterMessages);
        }

        protected override void PrepareForDataOperations()
        {
            //throw new NotImplementedException();
        }
    }
}
