using SmartDoc.BL.Models;
using Azure;
using Azure.AI.FormRecognizer.DocumentAnalysis;

namespace SmartDoc.BL.Services.InvoiceAnalyze;

internal sealed class AzureInvoiceAnalisisService : IAzureInvoiceAnalisisService
{
    private readonly string key = "b7265dd6172247ecb7b7af43a1fc2726";
    private readonly string endpoint = "https://file-analyze-service.cognitiveservices.azure.com/";

    public async Task<InvoiceResultResponse> Analyze(Stream fileStream)
    {
        AzureKeyCredential credential = new AzureKeyCredential(key);
        DocumentAnalysisClient client = new DocumentAnalysisClient(new Uri(endpoint), credential);

        AnalyzeDocumentOperation operation = await client.AnalyzeDocumentAsync(WaitUntil.Completed, "prebuilt-invoice", fileStream);
        AnalyzeResult result = operation.Value;

        AnalyzedDocument document = result.Documents[0];

        InvoiceResultResponse response = new();

        if (document.Fields.TryGetValue("CustomerName", out DocumentField? customerNameField) && customerNameField is not null)
        {
            if (customerNameField.FieldType == DocumentFieldType.String && customerNameField.Content is not null)
                response.CustomerName = customerNameField.Content;
        }

        if (document.Fields.TryGetValue("VendorName", out DocumentField? vendorNameField) && vendorNameField is not null)
        {
            if (vendorNameField.FieldType == DocumentFieldType.String && vendorNameField.Content is not null)
                response.VendorName = vendorNameField.Content;
        }

        return response;
    }
}
