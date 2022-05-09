using System.Reflection;
using Excursions.Application.Queries;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Excursions.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddMediatR(Assembly.GetExecutingAssembly());

        var connectionString = configuration.GetConnectionString("ExcursionsDatabase")
            ?? throw new InvalidOperationException("Connection string is not configured.");
        
        services.AddSingleton<IExcursionQueries>(_ => new ExcursionQueries(connectionString));
        services.AddSingleton<IBookingQueries>(_ => new BookingQueries(connectionString));

        services.AddLocalization();
        
        return services;
    }
}