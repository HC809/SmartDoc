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

/// <summary>
/// This C# function uses an HTTP GET request to retrieve all logs from a file log service and returns
/// them as an IActionResult.
/// </summary>
/// <returns>
/// The GetLogs method is returning an HTTP response with the status code 200 (OK) along with the result
/// of calling the GetAllLogs method from the _fileLogService.
/// </returns>
    [HttpGet]
    public async Task<IActionResult> GetLogs()
    {
        return Ok(await _fileLogService.GetAllLogs());
    }

   /// <summary>
   /// This C# function downloads filtered logs as an Excel file based on specified parameters.
   /// </summary>
   /// <param name="actionType">The `actionType` parameter in the `DownloadLogsAsExcel` method is used
   /// to filter logs based on a specific action type. It allows users to download logs in Excel format
   /// that match the specified action type.</param>
   /// <param name="description">The `description` parameter in the `DownloadLogsAsExcel` method is used
   /// to filter logs based on a specific description. When calling this method, you can provide a
   /// description value to filter the logs accordingly.</param>
   /// <param name="startDate">The `startDate` parameter in the `DownloadLogsAsExcel` method is used to
   /// specify the starting date for filtering logs. It is a DateTime type parameter that allows you to
   /// filter logs based on a specific start date.</param>
   /// <param name="endDate">The `endDate` parameter in the `DownloadLogsAsExcel` method is a DateTime
   /// type parameter that represents the end date for filtering logs. It is used to filter logs based
   /// on their timestamp to retrieve logs that fall within a specific date range.</param>
   /// <returns>
   /// The method `DownloadLogsAsExcel` is returning an Excel file containing filtered logs based on the
   /// parameters provided (actionType, description, startDate, endDate). The filtered logs are
   /// retrieved using the `_fileLogService.GetFilteredLogs` method and then converted into an Excel
   /// file using the `_fileLogService.GetFilteredLogsExcelFile` method. Finally, the method returns the
   /// Excel file as a downloadable file
   /// </returns>
 [HttpGet("download")]
    public async Task<IActionResult> DownloadLogsAsExcel(string? actionType = null, string? description = null, DateTime? startDate = null, DateTime? endDate = null)
    {
        var filteredLogs = await _fileLogService.GetFilteredLogs(actionType, description, startDate, endDate);

        var excelFile = _fileLogService.GetFilteredLogsExcelFile(filteredLogs.ToList());

        return File(excelFile, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "FilteredLogs.xlsx");
    }

}
