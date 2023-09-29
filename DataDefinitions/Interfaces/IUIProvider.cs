using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataDefinitions.Interfaces
{
    
    public interface IUIProvider
    {
        IEnumerable GetMenuElements(string Key);
        bool TryGetElements<T>(string Key, out T value);
    }
}
