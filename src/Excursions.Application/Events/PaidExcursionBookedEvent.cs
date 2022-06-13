namespace Excursions.Application.Events;

public record PaidExcursionBookedEvent(int BookingId, string TouristId) : IEvent
{
    public string GetTopic() => Topics.PaidExcursionBooked;
}