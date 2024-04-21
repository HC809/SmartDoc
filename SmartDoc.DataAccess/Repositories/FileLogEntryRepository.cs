using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SmartDoc.Data.Entites.DocumentLogEntries;

namespace SmartDoc.DataAccess.Repositories;
public sealed class FileLogEntryRepository : Repository<FileLogEntry>, IFileLogEntryRepository
{
    public FileLogEntryRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<IEnumerable<FileLogEntry>> GetFilteredAsync(
        string? actionType = null,
        string? description = null,
        DateTime? startDate = null,
        DateTime? endDate = null,
        CancellationToken cancellationToken = default)
    {
        var actionTypeParam = string.IsNullOrEmpty(actionType) ? (object)DBNull.Value : actionType;
        var descriptionParam = string.IsNullOrEmpty(description) ? (object)DBNull.Value : description;
        var startDateParam = startDate ?? (object)DBNull.Value;
        var endDateParam = endDate ?? (object)DBNull.Value;

        var filteredLogs = await DbContext.Set<FileLogEntry>()
            .FromSqlRaw(
                $@"SELECT
                    Id, ActionType, Description, CreatedOn
                FROM FileLogEntries
                WHERE 
                    (@actionType IS NULL OR ActionType LIKE '%' + @actionType + '%') AND
                    (@description IS NULL OR Description LIKE '%' + @description + '%') AND
                    (@startDate IS NULL OR CONVERT(date, CreatedOn) >= CONVERT(date, @startDate)) AND
                    (@endDate IS NULL OR CONVERT(date, CreatedOn) <= CONVERT(date, @endDate))",
                new SqlParameter("@actionType", actionTypeParam),
                new SqlParameter("@description", descriptionParam),
                new SqlParameter("@startDate", startDateParam),
                new SqlParameter("@endDate", endDateParam))
            .ToListAsync(cancellationToken);

        return filteredLogs;
    }
}
