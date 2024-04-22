using Microsoft.Extensions.DependencyInjection;
using SmartDoc.BL.Services.DocumentClassifier;
using SmartDoc.BL.Services.FileNotificationLogFactory;
using SmartDoc.BL.Services.InvoiceAnalyze;
using SmartDoc.BL.Services.Log;
using SmartDoc.BL.Services.SentimentAnalysis;
using SmartDoc.Data.Entites.DocumentLogEntries;
using SmartDoc.DataAccess.Repositories;

namespace SmartDoc.BL;
public static class DIContainer
{
    public static IServiceCollection AddBusinessLogic(this IServiceCollection services)
    {
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(typeof(DIContainer).Assembly);
        });

        services.AddScoped<IDocumentClassifierService, DocumentClassifierService>();
        services.AddScoped<IInvoiceAnalysisService, InvoiceAnalysisService>();
        services.AddScoped<ISentimentAnalysisService, SentimentAnalysisService>();
        services.AddScoped<IFileLogService, FileLogService>();
        services.AddSingleton<IFileNotificationLogFactory, FileNotificationLogFactory>();

        return services;
    }
}
