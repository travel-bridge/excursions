namespace Excursions.Application.Requests;

public class CreateExcursionRequest
{
    public string Name { get; init; } = null!;

    public string? Description { get; init; }

    public DateTime DateTimeUtc { get; init; }

    public int PlacesCount { get; init; }

    public decimal? PricePerPlace { get; init; }
}