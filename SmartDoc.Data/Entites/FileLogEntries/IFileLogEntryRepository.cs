namespace SmartDoc.Data.Entites.DocumentLogEntries;
public interface IFileLogEntryRepository
{
    void Add(FileLogEntry documentLog);
    Task<IEnumerable<FileLogEntry>> GetAllAsync(CancellationToken cancellationToken = default);

}
