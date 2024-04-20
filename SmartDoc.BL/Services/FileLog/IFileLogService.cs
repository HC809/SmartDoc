namespace SmartDoc.BL.Services.Log;
public interface IFileLogService
{
    Task<IEnumerable<FileLogDTO>> GetAllLogs();
}
