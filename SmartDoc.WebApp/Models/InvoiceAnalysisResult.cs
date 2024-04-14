namespace SmartDoc.WebApp.Models;

public sealed class InvoiceAnalysisResult
{
    public DocumentType DocumentType { get; init; }
    public InvoiceData Data { get; init; }
}

public sealed record InvoiceData(
    string VendorName,
    string CustomerName);
