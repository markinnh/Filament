using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataDefinitions.Interfaces;

public interface ISharingSupport
{
    bool IsSelectedForSharing
    {
        get; set;
    }
    Guid SharingID
    {
        get;
    }
    bool NotAlreadyInDatabase { get;set;
    }
    string SerializeForSharing();
    string SuggestedFileNameForSharing() => $"{GetType().Name}-{DateTime.Now:yyyyMMddHHmmss}.json";
}
