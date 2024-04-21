using SmartDoc.Data.Entites.DocumentLogEntries;

namespace SmartDoc.BL.Services.Log;
public interface IFileLogService
{
    Task<IEnumerable<FileLogDTO>> GetAllLogs();

    Task<IEnumerable<FileLogDTO>> GetFilteredLogs(string? actionType, string? description, DateTime? startDate, DateTime? endDate);

    byte[] GetFilteredLogsExcelFile(List<FileLogDTO> logs);
}
