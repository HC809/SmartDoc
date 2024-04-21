using Microsoft.EntityFrameworkCore;
using SmartDoc.Data.Entites.DocumentLogEntries;

namespace SmartDoc.DataAccess.Repositories;
public sealed class FileLogEntryRepository : Repository<FileLogEntry>, IFileLogEntryRepository
{
    public FileLogEntryRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<IEnumerable<FileLogEntry>> GetFilteredAsync(
        FileActionType? actionType = null,
        string? description = null,
        DateTime? startDate = null,
        DateTime? endDate = null,
        CancellationToken cancellationToken = default)
    {

        //USAR QUERY DE EF 
        var logs = await DbContext.Set<FileLogEntry>().AsNoTracking()
         .Where(log =>
            (!actionType.HasValue || log.ActionType == actionType) &&
            (string.IsNullOrEmpty(description) || log.Description.Value == (description)) &&
            (!startDate.HasValue || log.CreatedOn.Date >= startDate.Value) &&
            (!endDate.HasValue || log.CreatedOn.Date <= endDate.Value)
        ).ToListAsync(cancellationToken);

        return logs;
    }
}
