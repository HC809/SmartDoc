using SmartDoc.Data.Abstractions;
using System.Linq.Expressions;

namespace SmartDoc.Data.Entites.DocumentLogEntries;
public interface IFileLogEntryRepository
{
    void Add(FileLogEntry documentLog);
    Task<IEnumerable<FileLogEntry>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<IEnumerable<FileLogEntry>> GetFilteredAsync(string? actionType = null, string? description = null, DateTime? startDate = null, DateTime? endDate = null, CancellationToken cancellationToken = default);
}
