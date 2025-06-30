using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataDefinitions.Interfaces;
/// <summary>
/// Identify items consume events, so they can be 'released' and allow garbage collection
/// </summary>
public interface IEventListener
{
    void ReleaseEventHandlers();
}
