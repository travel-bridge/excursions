using Excursions.Api.Infrastructure;
using HotChocolate.AspNetCore.Authorization;

namespace Excursions.Api.Gql.Schema.Queries;

public class RootQuery
{
    [GraphQLName("excursions"), Authorize(Policy = AuthorizePolicies.ReadExcursions)]
    public ExcursionQuery ExcursionQuery => new();
    
    [GraphQLName("booking"), Authorize(Policy = AuthorizePolicies.ReadExcursions)]
    public BookingQuery BookingQuery => new();
}