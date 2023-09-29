using DataDefinitions.Interfaces;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filament.WPF6.Helpers
{
    /// <summary>
    /// Probably can be eliminated, reduced the complexity of the WindowsFilter to a single class, all the complexity is in the IResolveFilter interface
    /// </summary>
    /// <seealso cref="IResolveFilter"/>
    public interface IWindowsFilter
    {
        //public enum Filters : short
        //{
        //    ShowAll=0x1000,
        //    Tag,
        //    Keyword
        //}
        /// <summary>
        /// Determines if the Filter is applied
        /// </summary>
        bool Applied { get; set; }
        /// <summary>
        /// performs defined checks specific to class to determine if the item is accepted
        /// </summary>
        /// <param name="item">item to examine</param>
        /// <returns>criteria satified</returns>
        //bool Accepted(object item);
        void Filter(object sender, System.Windows.Data.FilterEventArgs e);
        IResolveFilter Resolve { get; set; }
    }
}
