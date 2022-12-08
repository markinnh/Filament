using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filament.WPF6.Helpers
{
    public interface IFilter
    {
#if DEBUG
        object Owner { get; }
#endif
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

    }
}
