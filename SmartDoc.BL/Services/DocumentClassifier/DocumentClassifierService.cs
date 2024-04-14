
using Azure.AI.FormRecognizer.DocumentAnalysis;
using Azure;
using Azure.Core;

namespace SmartDoc.BL.Services.DocumentClassifier;
internal sealed class DocumentClassifierService : IDocumentClassifierService
{
    private readonly string key = "b7265dd6172247ecb7b7af43a1fc2726";
    private readonly string endpoint = "https://file-analyze-service.cognitiveservices.azure.com/";

    public async Task<DocumentType> GetDocumentType(Stream fileStream)
    {
        AzureKeyCredential credential = new AzureKeyCredential(key);
        DocumentAnalysisClient client = new DocumentAnalysisClient(new Uri(endpoint), credential);

        AnalyzeDocumentOperation operation = await client.AnalyzeDocumentAsync(WaitUntil.Completed, "prebuilt-layout", fileStream);
        AnalyzeResult analyzeResult = operation.Value;

        return IsInvoice(analyzeResult) ? DocumentType.Invoice : DocumentType.Other;
    }

    private bool IsInvoice(AnalyzeResult analyzeResult)
    {
        return
            analyzeResult.Content.Contains("Invoice", StringComparison.OrdinalIgnoreCase) ||
            analyzeResult.Content.Contains("Factura", StringComparison.OrdinalIgnoreCase) ||
            analyzeResult.Content.Contains("Cliente", StringComparison.OrdinalIgnoreCase) ||
            analyzeResult.Content.Contains("Customer", StringComparison.OrdinalIgnoreCase) ||
            analyzeResult.Content.Contains("SubTotal", StringComparison.OrdinalIgnoreCase) ||
            analyzeResult.Content.Contains("Rechnungsbetrag", StringComparison.OrdinalIgnoreCase) ||
            analyzeResult.Content.Contains("Total", StringComparison.OrdinalIgnoreCase);
    }
}
