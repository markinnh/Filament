using Filament_Db.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filament.WPF6.ViewModels
{
    internal class TreeData:List<Tree>
    {

    }

    internal class Tree
    {
        public ObservableCollection<VendorDefn> Items { get; set; }

        public string Name { get; set; }

        internal Tree(ObservableCollection<VendorDefn> items,string name)
        {
            Items = items;
            Name = name;
        }

    }
}
