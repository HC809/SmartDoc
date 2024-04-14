namespace SmartDoc.WebApp.Models;

public sealed class InvoiceAnalysisResult
{
    public bool Success { get; set; }
    public DocumentType DocumentType { get; init; }
    public InvoiceData Data { get; init; }
}

public sealed record InvoiceData(
    string VendorName,
    string VendorAddress,
    string CustomerName,
    string CustomerAddress,
    string InvoiceId,
    string InvoiceDate,
    string Total
    );
