﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using SmartDoc.Api.Models;
using SmartDoc.BL.Services.DocumentClassifier;
using SmartDoc.BL.Services.InvoiceAnalyze;
using SmartDoc.BL.Services.FileNotificationLogFactory;
using SmartDoc.BL.Services.SentimentAnalysis;

namespace SmartDoc.Api.Controllers;

[ApiController]
[Route("api/file")]

public class FileController : ControllerBase
{
    private readonly IDocumentClassifierService _documentClassifierService;
    private readonly IInvoiceAnalysisService _invoiceAnalisisService;
    private readonly ISentimentAnalysisService _sentimentAnalysisService;
    private readonly IFileNotificationLogFactory _logFactory;
    private readonly IMediator _mediator;

    public FileController(
        IDocumentClassifierService documentClassifierService,
        IInvoiceAnalysisService invoiceAnalisisService,
        ISentimentAnalysisService sentimentAnalysisService,
        IFileNotificationLogFactory logFactory,
        IMediator mediator)
    {
        _documentClassifierService = documentClassifierService;
        _invoiceAnalisisService = invoiceAnalisisService;
        _sentimentAnalysisService = sentimentAnalysisService;
        _logFactory = logFactory;
        _mediator = mediator;
    }

    /// <summary>
    /// This C# function analyzes a file uploaded through a form, determines its type (invoice or
    /// information), and performs specific analysis based on the type.
    /// </summary>
    /// <param name="IFormFile">The code snippet you provided is a C# method that analyzes a file
    /// uploaded through a form. The method `AnalyzeFile` is an HTTP POST endpoint that takes an
    /// `IFormFile` parameter named `file`. Here's a breakdown of the code:</param>
    /// <returns>
    /// The AnalyzeFile method returns different types of responses based on the type of document being
    /// analyzed:
    /// </returns>
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
            var documentTypeResponse = await _documentClassifierService.GetDocumentType(classificationStream);
            memoryStream.Seek(0, SeekOrigin.Begin);

            switch (documentTypeResponse.DocumentType)
            {
                case DocumentType.Invoice:
                    var invoiceData = await _invoiceAnalisisService.GetInvoiceAnalysisData(memoryStream);
                    return Ok(new FileAnalysisResponse(DocumentType.Invoice.ToString(), invoiceData));

                case DocumentType.Information:
                    var fileSentiment =  await _sentimentAnalysisService.GetSentimentAnalysisResponse(documentTypeResponse.Document!);
                    return Ok(new FileAnalysisResponse(DocumentType.Information.ToString(), fileSentiment));

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
