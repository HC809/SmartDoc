﻿using Azure;
using Azure.AI.FormRecognizer.DocumentAnalysis;

namespace SmartDoc.BL.Services.InvoiceAnalyze;

internal sealed class InvoiceAnalysisService : IInvoiceAnalysisService
{
    private readonly string key = "b7265dd6172247ecb7b7af43a1fc2726";
    private readonly string endpoint = "https://file-analyze-service.cognitiveservices.azure.com/";

    public async Task<InvoiceData> Analyze(Stream fileStream)
    {
        AzureKeyCredential credential = new AzureKeyCredential(key);
        DocumentAnalysisClient client = new DocumentAnalysisClient(new Uri(endpoint), credential);

        AnalyzeDocumentOperation operation = await client.AnalyzeDocumentAsync(WaitUntil.Completed, "prebuilt-invoice", fileStream);
        AnalyzeResult result = operation.Value;

        AnalyzedDocument document = result.Documents[0];
        var docFields = document.Fields;

        InvoiceData response = new(
            TryGetFieldContent(docFields, "VendorName", DocumentFieldType.String) ?? "N/D",
            TryGetFieldContent(docFields, "VendorAddress", DocumentFieldType.Address) ?? "N/D",
            TryGetFieldContent(docFields, "CustomerName", DocumentFieldType.String) ?? "N/D",
            TryGetFieldContent(docFields, "CustomerAddress", DocumentFieldType.Address) ?? "N/D",
            TryGetFieldContent(docFields, "InvoiceId", DocumentFieldType.String) ?? "N/D",
            TryGetFieldContent(docFields, "InvoiceDate", DocumentFieldType.Date) ?? "N/D",
            TryGetFieldContent(docFields, "InvoiceTotal", DocumentFieldType.Currency) ?? "0.00"
            );

        return response;
    }

    private string? TryGetFieldContent(IReadOnlyDictionary<string, DocumentField> fields, string fieldName, DocumentFieldType fieldType)
    {
        if (fields.TryGetValue(fieldName, out DocumentField? field) && field is not null)
        {
            if (field.FieldType == fieldType && field.Content is not null)
                return field.Content;
        }

        return null;
    }
}