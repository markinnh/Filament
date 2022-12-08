namespace DataDefinitions.Interfaces
{
    public interface ITrackModified
    {
        bool InDataOperations { get; }
        bool IsModified { get; set; }
    }
}
