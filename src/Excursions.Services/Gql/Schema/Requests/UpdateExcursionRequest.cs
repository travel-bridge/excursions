namespace Excursions.Api.Gql.Schema.Requests;

public class UpdateExcursionRequest
{
    public int Id { get; init; }
    
    public string? Name { get; init; }

    public Optional<string> Description { get; init; }

    public DateTime? DateTimeUtc { get; init; }

    public int? PlacesCount { get; init; }

    public Optional<decimal> PricePerPlace { get; init; }
}