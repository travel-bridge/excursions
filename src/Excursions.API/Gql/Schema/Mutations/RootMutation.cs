namespace Excursions.Api.Gql.Schema.Mutations;

public class RootMutation
{
    [GraphQLName("excursions")]
    public ExcursionMutation ExcursionMutation => new();
}