using ClosedXML.Excel;
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
        var logDTOs = logs.Select(log => new FileLogDTO(log.Id, log.ActionType.ToString(), log.Description, log.CreatedOn)).ToList();

        return logDTOs;
    }

    public async Task<IEnumerable<FileLogDTO>> GetFilteredLogs(string? actionType = null, string? description = null, DateTime? startDate = null, DateTime? endDate = null)
    {
        var filteredLogs = (!string.IsNullOrEmpty(actionType) || !string.IsNullOrEmpty(description) || startDate != null || endDate != null)
            ? await _fileLogRepository.GetFilteredAsync(actionType, description, startDate, endDate)
            : await _fileLogRepository.GetAllAsync();

        var filteredLogDTOs = filteredLogs.Select(log => new FileLogDTO(log.Id, log.ActionType.ToString(), log.Description, log.CreatedOn)).ToList();

        return filteredLogDTOs;
    }

    public byte[] GetFilteredLogsExcelFile(List<FileLogDTO> logs)
    {
        using (var workbook = new XLWorkbook())
        {
            var worksheet = workbook.AddWorksheet("Logs");
            worksheet.Cell(1, 1).Value = "ID";
            worksheet.Cell(1, 2).Value = "Action Type";
            worksheet.Cell(1, 3).Value = "Description";
            worksheet.Cell(1, 4).Value = "Created On";

            for (int index = 0; index < logs.Count; index++)
            {
                var log = logs[index];
                int row = index + 2;
                worksheet.Cell(row, 1).Value = log.Id;
                worksheet.Cell(row, 2).Value = log.ActionType;
                worksheet.Cell(row, 3).Value = log.Description;
                worksheet.Cell(row, 4).Value = log.CreatedOn;
            }

            using (var stream = new MemoryStream())
            {
                workbook.SaveAs(stream);
                return stream.ToArray();
            }
        }
    }
}

