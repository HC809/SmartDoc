using Azure;
using Azure.AI.FormRecognizer.DocumentAnalysis;
using Microsoft.AspNetCore.Mvc;
using SmartDoc.Api.Models;

namespace SmartDoc.Api.Controllers;

[ApiController]
[Route("api/file")]

public class FileController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> UploadFile([FromForm] IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("No se logro cargar ningun archivo. Intente nuevamente.");
        }

        FileUploadResponse data = new();

        string key = "b7265dd6172247ecb7b7af43a1fc2726";
        string endpoint = "https://file-analyze-service.cognitiveservices.azure.com/";

        try
        {
            AzureKeyCredential credential = new AzureKeyCredential(key);
            DocumentAnalysisClient client = new DocumentAnalysisClient(new Uri(endpoint), credential);

            using Stream stream = file.OpenReadStream();
            AnalyzeDocumentOperation operation = await client.AnalyzeDocumentAsync(WaitUntil.Completed, "prebuilt-invoice", stream);
            AnalyzeResult result = operation.Value;

            AnalyzedDocument document = result.Documents[0];

            var fields = document.Fields;

            if (fields.TryGetValue("CustomerName", out DocumentField? customerNameField) && customerNameField is not null)
            {
                if (customerNameField.FieldType == DocumentFieldType.String && customerNameField.Content is not null)
                {
                    data.CustomerName = customerNameField.Content;
                }
            }

            if (fields.TryGetValue("VendorName", out DocumentField? vendorNameField) && vendorNameField is not null)
            {
                if (vendorNameField.FieldType == DocumentFieldType.String && vendorNameField.Content is not null)
                {
                    data.VendorName = vendorNameField.Content;
                }
            }

            return Ok(data);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}
