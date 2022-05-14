using Excursions.Api.Infrastructure;
using HotChocolate.AspNetCore.Authorization;

namespace Excursions.Api.Gql.Schema.Mutations;

public class RootMutation
{
    [GraphQLName("excursions"), Authorize(Policy = AuthorizePolicies.WriteExcursions)]
    public ExcursionMutation ExcursionMutation => new();
}