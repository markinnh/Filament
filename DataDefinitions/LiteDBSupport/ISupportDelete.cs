using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataDefinitions.LiteDBSupport;

public interface IDeleteSupport
{
    void Delete(ILiteDBDAL lite);
}
