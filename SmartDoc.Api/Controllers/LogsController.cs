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
}
