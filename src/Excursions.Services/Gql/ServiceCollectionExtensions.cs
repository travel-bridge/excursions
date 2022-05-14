using Excursions.Api.Gql.Infrastructure;
using Excursions.Api.Gql.Schema.Mutations;
using Excursions.Api.Gql.Schema.Queries;

namespace Excursions.Api.Gql;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddGql(this IServiceCollection services)
    {
        services
            .AddGraphQLServer()
            .AddAuthorization()
            .AddQueryType<RootQuery>()
            .AddMutationType<RootMutation>();

        services.AddErrorFilter<GqlErrorFilter>();
        
        return services;
    }
}