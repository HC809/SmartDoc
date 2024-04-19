using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmartDoc.Data.Abstractions;
using SmartDoc.Data.Entites.DocumentLogEntries;
using SmartDoc.DataAccess.Repositories;

namespace SmartDoc.DataAccess;
public static class DIContainer
{
    public static IServiceCollection AddDataAccess(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("OneCoreDb") ?? throw new ArgumentNullException(nameof(configuration));

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(connectionString);
        });

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ApplicationDbContext>());
        services.AddScoped<IFileLogEntryRepository, FileLogEntryRepository>();

        return services;
    }
}
