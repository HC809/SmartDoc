using Microsoft.AspNetCore.Mvc;
using SmartDoc.BL.Services.InvoiceAnalyze;

namespace SmartDoc.Api.Controllers;

[ApiController]
[Route("api/file")]

public class FileController : ControllerBase
{
    private readonly IAzureInvoiceAnalisisService _azureInvoiceAnalisisService;

    public FileController(IAzureInvoiceAnalisisService azureInvoiceAnalisisService)
    {
        _azureInvoiceAnalisisService = azureInvoiceAnalisisService;
    }

    [HttpPost]
    public async Task<IActionResult> UploadFile([FromForm] IFormFile file)
    {
        if (file is null || file.Length == 0)
        {
            return BadRequest("No se logro cargar ningun archivo. Intente nuevamente.");
        }

        try
        {
            using Stream fileStream = file.OpenReadStream();
            var result = await _azureInvoiceAnalisisService.Analyze(fileStream);

            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}
