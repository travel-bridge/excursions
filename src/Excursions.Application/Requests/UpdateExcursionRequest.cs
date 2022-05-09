namespace Excursions.Application.Requests;

public class UpdateExcursionRequest
{
    public int Id { get; init; }
    
    public string? Name { get; init; }

    public string? Description { get; init; }

    public DateTime? DateTimeUtc { get; init; }

    public int? PlacesCount { get; init; }

    public decimal? PricePerPlace { get; init; }
}