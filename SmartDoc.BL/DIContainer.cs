using Microsoft.Extensions.DependencyInjection;
using SmartDoc.BL.Services.InvoiceAnalyze;

namespace SmartDoc.BL;
public static class DIContainer
{
    public static IServiceCollection AddBusinessLogic(this IServiceCollection services)
    {
        services.AddScoped<IAzureInvoiceAnalisisService, AzureInvoiceAnalisisService>();

        return services;
    }
}
