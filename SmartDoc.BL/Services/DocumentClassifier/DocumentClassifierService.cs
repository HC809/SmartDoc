
using Azure.AI.FormRecognizer.DocumentAnalysis;
using Azure;
using SmartDoc.BL.Services.InvoiceAnalyze;
using Microsoft.Extensions.Options;

namespace SmartDoc.BL.Services.DocumentClassifier;
internal sealed class DocumentClassifierService : IDocumentClassifierService
{
    private readonly DocumentAnalysisSettings _settings;

    public DocumentClassifierService(IOptions<DocumentAnalysisSettings> options)
    {
        _settings = options.Value ?? throw new ArgumentNullException(nameof(options));
    }

    public async Task<DocumentClassifierResponse> GetDocumentType(Stream fileStream)
    {
        AzureKeyCredential credential = new AzureKeyCredential(_settings.DocumentAnalysisApiKey);
        DocumentAnalysisClient client = new DocumentAnalysisClient(new Uri(_settings.DocumentAnalysisEndpoint), credential);

        AnalyzeDocumentOperation operation = await client.AnalyzeDocumentAsync(WaitUntil.Completed, "prebuilt-layout", fileStream);
        AnalyzeResult analyzeResult = operation.Value;

        if (IsInvoice(analyzeResult))
            return new DocumentClassifierResponse(DocumentType.Invoice);

        string documentText = GetDocumentText(analyzeResult);
        if (!string.IsNullOrEmpty(documentText))
            return new DocumentClassifierResponse(DocumentType.Information, documentText);

        return new DocumentClassifierResponse(DocumentType.Other);
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

    private string GetDocumentText(AnalyzeResult analyzeResult)
    {
        var paragraphs = new List<string>();

        foreach (var paragraph in analyzeResult.Paragraphs)
        {
            paragraphs.Add(paragraph.Content);
        }

        return string.Join(" ", paragraphs);
    }

}
