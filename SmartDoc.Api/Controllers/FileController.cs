using Microsoft.AspNetCore.Mvc;
using SmartDoc.Api.Models;

namespace SmartDoc.Api.Controllers;

[ApiController]
[Route("api/file")]

public class FileController : ControllerBase
{
    [HttpPost]
    public IActionResult UploadFile([FromForm] IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("No se logro cargar ningun archivo. Intente nuevamente.");
        }

        var result = new FileUploadResponse(file.FileName.ToString(), file.Length.ToString());

        return Ok(result);
    }
}
