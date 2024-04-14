using Microsoft.Extensions.DependencyInjection;
using SmartDoc.BL.Services.DocumentClassifier;
using SmartDoc.BL.Services.InvoiceAnalyze;

namespace SmartDoc.BL;
public static class DIContainer
{
    public static IServiceCollection AddBusinessLogic(this IServiceCollection services)
    {
        services.AddScoped<IDocumentClassifierService, DocumentClassifierService>();
        services.AddScoped<IInvoiceAnalysisService, InvoiceAnalysisService>();

        return services;
    }
}
