namespace Excursions.Application.Responses;

public class ExcursionResponse
{
    public int Id { get; init; }

    public string Name { get; init; } = null!;

    public string? Description { get; init; }

    public DateTime DateTimeUtc { get; init; }

    public int PlacesCount { get; init; }

    public int PlacesBooked { get; init; }

    public decimal? PricePerPlace { get; init; }

    public string GuideId { get; init; } = null!;

    public string Status { get; init; } = null!;
}