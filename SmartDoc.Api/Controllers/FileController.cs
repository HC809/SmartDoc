using MediatR;
using Microsoft.AspNetCore.Mvc;
using SmartDoc.Api.Models;
using SmartDoc.BL.Services.DocumentClassifier;
using SmartDoc.BL.Services.InvoiceAnalyze;
using SmartDoc.BL.Services.FileNotificationLogFactory;

namespace SmartDoc.Api.Controllers;

[ApiController]
[Route("api/file")]

public class FileController : ControllerBase
{
    private readonly IDocumentClassifierService _documentClassifierService;
    private readonly IInvoiceAnalysisService _invoiceAnalisisService;
    private readonly IFileNotificationLogFactory _logFactory;
    private readonly IMediator _mediator;

    public FileController(
        IDocumentClassifierService documentClassifierService, 
        IInvoiceAnalysisService invoiceAnalisisService,
        IFileNotificationLogFactory logFactory,
        IMediator mediator)
    {
        _documentClassifierService = documentClassifierService;
        _invoiceAnalisisService = invoiceAnalisisService;
        _logFactory = logFactory;
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> AnalyzeFile([FromForm] IFormFile file)
    {
        if (file is null || file.Length == 0)
        {
            return BadRequest("No se logro cargar ningun archivo. Intente nuevamente.");
        }

        var logEntry = _logFactory.Create("Azure FormRecognizer", file.FileName, file.Length);
        await _mediator.Publish(logEntry);

        MemoryStream memoryStream = new MemoryStream();

        try
        {
            Stream fileStream = file.OpenReadStream();
            await fileStream.CopyToAsync(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin); // Restablece el puntero al inicio después de copiar.

            using var classificationStream = new MemoryStream(memoryStream.ToArray());
            var documentType = await _documentClassifierService.GetDocumentType(classificationStream);
            memoryStream.Seek(0, SeekOrigin.Begin);

            switch (documentType)
            {
                case DocumentType.Invoice:
                    var invoiceData = await _invoiceAnalisisService.Analyze(memoryStream);
                    return Ok(new InvoiceAnalysisResponse(DocumentType.Invoice, invoiceData));

                case DocumentType.Information:
                    var generalTextAnalisisResult = "El documento es de información.";
                    return Ok(new { DocumentType.Invoice, generalTextAnalisisResult });

                default:
                    string message = "El documento seleccionado no es de tipo 'factura' ni 'información'. Por favor seleccione otro documento.";
                    return BadRequest(new FileErrorResponse(DocumentType.Invoice, message));
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error al procesar el documento: {ex.Message}");
        }
        finally { memoryStream.Dispose(); }
    }
}
