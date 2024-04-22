namespace SmartDoc.BL.Services.InvoiceAnalyze;
public interface IInvoiceAnalysisService
{
    Task<InvoiceData> GetInvoiceAnalysisData(Stream fileStream);
}
