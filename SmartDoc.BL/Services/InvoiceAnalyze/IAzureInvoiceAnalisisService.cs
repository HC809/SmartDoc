using SmartDoc.BL.Models;

namespace SmartDoc.BL.Services.InvoiceAnalyze;
public interface IAzureInvoiceAnalisisService
{
    Task<InvoiceResultResponse> Analyze(Stream fileStream);
}
