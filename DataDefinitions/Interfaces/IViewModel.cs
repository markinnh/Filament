using System;

namespace DataDefinitions.Interfaces;

/// <summary>
/// Support ViewModels having a Guid Id which ensures messages are received by the correct recipient
/// </summary>
/// <remarks>
/// since GC does not happen at any measurable interval
/// </remarks>
public interface IViewModel:IEventListener
{
    Guid ViewModelGuid { get; }
}
