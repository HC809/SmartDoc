
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

    /// <summary>
    /// The function `GetDocumentType` analyzes a document using Azure Document Analysis service and
    /// classifies it as an invoice, information, or other based on the content.
    /// </summary>
    /// <param name="Stream">The `Stream fileStream` parameter in the `GetDocumentType` method
    /// represents a stream of data that contains the content of the document to be analyzed. This
    /// stream allows you to read the document content without loading the entire file into memory at
    /// once, which can be useful for processing large documents efficiently.</param>
    /// <returns>
    /// The method `GetDocumentType` returns a `DocumentClassifierResponse` object. The
    /// `DocumentClassifierResponse` object contains information about the type of document being
    /// analyzed. The type of document can be one of the following:
    /// 1. If the document is identified as an invoice, the method returns a
    /// `DocumentClassifierResponse` object with the document type set to `DocumentType.Invoice`.
    /// 2. If
    /// </returns>
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

    /// <summary>
    /// The IsInvoice function checks if the content of an AnalyzeResult object contains specific
    /// keywords related to invoices in various languages.
    /// </summary>
    /// <param name="AnalyzeResult">AnalyzeResult is a class or structure that contains information
    /// about the result of an analysis process. In the provided code snippet, the IsInvoice method
    /// takes an instance of AnalyzeResult as a parameter and checks if the content of the AnalyzeResult
    /// object contains certain keywords related to invoices.</param>
    /// <returns>
    /// The method `IsInvoice` returns a boolean value indicating whether the `analyzeResult` contains
    /// any of the specified keywords related to an invoice, such as "Invoice", "Factura", "Cliente",
    /// "Customer", "SubTotal", "Rechnungsbetrag", or "Total".
    /// </returns>
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

/// <summary>
/// The function `GetDocumentText` takes an `AnalyzeResult` object, extracts the content of each
/// paragraph, and returns a single string containing all paragraph contents separated by spaces.
/// </summary>
/// <param name="AnalyzeResult">AnalyzeResult is a class that contains information about the result of
/// analyzing a document. It likely has a property named Paragraphs which is a collection of paragraphs
/// with content.</param>
/// <returns>
/// The `GetDocumentText` method returns a single string that contains the content of all paragraphs in
/// the `AnalyzeResult` object, with each paragraph separated by a space.
/// </returns>
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
