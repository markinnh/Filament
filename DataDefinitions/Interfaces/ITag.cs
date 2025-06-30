using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataDefinitions.Interfaces;

public interface ITag
{
    string Tags { get; set; }
    IEnumerable<string> GetTags();
}
