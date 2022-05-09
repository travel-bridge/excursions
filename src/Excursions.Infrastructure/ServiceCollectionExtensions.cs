using Excursions.Domain.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Excursions.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("ExcursionsDatabase")
            ?? throw new InvalidOperationException("Connection string is not configured.");

        services.AddDbContext<DataContext>(
            builder => builder.UseNpgsql(
                connectionString,
                options => options.EnableRetryOnFailure(
                    3,
                    TimeSpan.FromSeconds(10),
                    null)));

        services.AddScoped<IRepositoryRegistry, RepositoryRegistry>();
        services.AddScoped<IDataExecutionContext, DataExecutionContext>();
        
        return services;
    }
}