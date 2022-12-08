using DataDefinitions;
using DataDefinitions.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Filament.WPF6.Helpers
{
    internal class ShowAllFilter :Observable, IFilter
    {
#if DEBUG
        public object Owner { get; set; }
#endif
        private bool _applied;
        public bool Applied { get => _applied; set =>Set(ref _applied,value); }

        public void Filter(object sender, FilterEventArgs e)
        {
            if (e.Item is ITrackUsable usable)
                e.Accepted = !usable.StopUsing;
            //throw new NotImplementedException();
        }
    }
}
