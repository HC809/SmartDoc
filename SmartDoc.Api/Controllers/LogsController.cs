using Microsoft.AspNetCore.Mvc;
using SmartDoc.BL.Services.Log;

namespace SmartDoc.Api.Controllers;

[ApiController]
[Route("api/logs")]
public class LogsController : ControllerBase
{
    private readonly IFileLogService _fileLogService;

    public LogsController(IFileLogService fileLogService)
    {
        _fileLogService = fileLogService;
    }

    [HttpGet]
    public async Task<IActionResult> GetLogs()
    {
        return Ok(await _fileLogService.GetAllLogs());
    }

    [HttpGet("download")]
    public async Task<IActionResult> DownloadLogsAsExcel(string? actionType = null, string? description = null, DateTime? startDate = null, DateTime? endDate = null)
    {
        var filteredLogs = (!string.IsNullOrEmpty(actionType) || !string.IsNullOrEmpty(description) || startDate != null || endDate != null)
            ? await _fileLogService.GetFilteredLogs(actionType, description, startDate, endDate)
            : await _fileLogService.GetAllLogs();

        var excelFile = _fileLogService.GetFilteredLogsExcelFile(filteredLogs.ToList());

        return File(excelFile, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "FilteredLogs.xlsx");
    }

}
