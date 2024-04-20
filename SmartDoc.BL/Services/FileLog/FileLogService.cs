using SmartDoc.Data.Entites.DocumentLogEntries;

namespace SmartDoc.BL.Services.Log;
public sealed class FileLogService : IFileLogService
{
    private readonly IFileLogEntryRepository _fileLogRepository;

    public FileLogService(IFileLogEntryRepository fileLogRepository)
    {
        _fileLogRepository = fileLogRepository;
    }

    public async Task<IEnumerable<FileLogDTO>> GetAllLogs()
    {
        var logs = await _fileLogRepository.GetAllAsync();

        var logDTOs = logs.Select(log => new FileLogDTO(log.Id, log.ActionType.ToString(), log.Description.Value, log.CreatedOn)).ToList();

        return logDTOs;
    }
}
