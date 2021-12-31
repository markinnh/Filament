using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HintLib
{
    public class DataLayer
    {
        public ObservableCollection<HintProject>? Projects { get; set; }
        public DataLayer()
        {
            BaseObject.InDataOps = true;
            //var iEnumerable = Context.HintContext.GetHintProjects();
            if(Context.HintContext.GetHintProjects() is IEnumerable<HintProject> iEnumerable)
                Projects=new ObservableCollection<HintProject>(iEnumerable);
            else
                Projects=new ObservableCollection<HintProject>();

            Projects.CollectionChanged += Projects_CollectionChanged;
            BaseObject.InDataOps = false;
        }
        ~DataLayer()
        {
            Projects.CollectionChanged-= Projects_CollectionChanged;
        }
        private void Projects_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            //if(e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add && e.NewItems != null)
            //    foreach(var item in e.NewItems)
            //        if(item is HintProject project)

            //throw new NotImplementedException();
        }
    }
}
