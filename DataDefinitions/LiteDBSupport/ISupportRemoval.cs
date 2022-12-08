namespace DataDefinitions.LiteDBSupport
{
    public enum RemovalResult { RecordRemoved,HasReferences};
    public interface ISupportRemoval
    {
        /// <summary>
        /// Removes item from a database, normally a 'root' object
        /// </summary>
        /// <param name="dBDal">Data access layer</param>
        /// <remarks>
        /// used sparingly
        /// </remarks>
        (RemovalResult,object) Remove(LiteDBDal dBDal);

    }
}
