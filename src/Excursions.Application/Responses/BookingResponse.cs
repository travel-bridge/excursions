namespace Excursions.Application.Responses;

public class BookingResponse
{
    public int Id { get; init; }

    public string Status { get; init; } = null!;

    public int ExcursionId { get; init; }

    public string ExcursionName { get; set; } = null!;

    public DateTime ExcursionDateTimeUtc { get; init; }
}