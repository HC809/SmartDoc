using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SmartDoc.Data.Entites.DocumentLogEntries;

namespace SmartDoc.DataAccess.Repositories;
public sealed class FileLogEntryRepository : Repository<FileLogEntry>, IFileLogEntryRepository
{
    public FileLogEntryRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    /// <summary>
    /// This C# async method retrieves filtered FileLogEntry records based on specified criteria using
    /// raw SQL query and parameters.
    /// </summary>
    /// <param name="actionType">The `actionType` parameter is used to filter the log entries based on
    /// the type of action performed. If a value is provided for `actionType`, only log entries with a
    /// matching `ActionType` will be included in the result. If `actionType` is `null`, all log
    /// entries</param>
    /// <param name="description">The `description` parameter in the `GetFilteredAsync` method is used
    /// to filter the log entries based on the description of the action. If a value is provided for the
    /// `description` parameter, the method will only return log entries where the description contains
    /// the specified value (case-insensitive search using</param>
    /// <param name="startDate">The `startDate` parameter in the `GetFilteredAsync` method is used to
    /// filter log entries based on the date they were created. If a `startDate` value is provided, only
    /// log entries created on or after that date will be included in the result. If `startDate` is not
    /// provided (</param>
    /// <param name="endDate">The `endDate` parameter in the `GetFilteredAsync` method is used to filter
    /// the log entries based on the date they were created. If a value is provided for `endDate`, only
    /// log entries created on or before that date will be included in the result. If `endDate` is not
    /// provided</param>
    /// <param name="CancellationToken">The CancellationToken parameter in the GetFilteredAsync method
    /// allows you to pass a token that can be used to request cancellation of the asynchronous
    /// operation. This token can be used to notify the operation that it should be canceled, allowing
    /// for graceful termination of the operation if needed. It is a way to provide a means</param>
    /// <returns>
    /// This method returns a collection of `FileLogEntry` objects that match the specified filter
    /// criteria. The filter criteria include `actionType`, `description`, `startDate`, and `endDate`.
    /// The method executes a raw SQL query against the `FileLogEntries` table in the database using the
    /// provided filter parameters. The query filters the results based on the provided criteria and
    /// returns the filtered logs as a collection of
    /// </returns>
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
