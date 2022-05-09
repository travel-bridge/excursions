namespace Excursions.Api.Gql.Schema.Queries;

public class RootQuery
{
    [GraphQLName("excursions")]
    public ExcursionQuery ExcursionQuery => new();
    
    [GraphQLName("booking")]
    public BookingQuery BookingQuery => new();
}