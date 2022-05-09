namespace Excursions.Application.Responses;

public class BookingToRejectResponse
{
    public int ExcursionId { get; init; }

    public string TouristId { get; init; } = null!;
}