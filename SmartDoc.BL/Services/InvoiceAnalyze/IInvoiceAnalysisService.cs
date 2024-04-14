namespace SmartDoc.BL.Services.InvoiceAnalyze;
public interface IInvoiceAnalysisService
{
    Task<InvoiceData> Analyze(Stream fileStream);
}
