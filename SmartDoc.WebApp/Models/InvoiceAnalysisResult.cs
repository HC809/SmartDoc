namespace SmartDoc.WebApp.Models;

public sealed class InvoiceAnalysisResult
{
    public bool Success { get; set; } = false;
    public DocumentType DocumentType { get; init; }
    public InvoiceData? Data { get; init; }
}

public sealed record InvoiceData(
    string VendorName,
    string VendorAddress,
    string CustomerName,
    string CustomerAddress,
    string InvoiceId,
    string InvoiceDate,
    string Total,
    IEnumerable<Items> Items);

public sealed record Items(
    string Name,
    string UnitPrice,
    string Quantity,
    string Total);


