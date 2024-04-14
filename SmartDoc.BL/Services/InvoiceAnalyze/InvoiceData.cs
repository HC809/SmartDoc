namespace SmartDoc.BL.Services.InvoiceAnalyze;
public sealed record InvoiceData(
    string VendorName,
    string VendorAddress,
    string CustomerName,
    string CustomerAddress,
    string InvoiceId,
    string InvoiceDate,
    string Total
    );
