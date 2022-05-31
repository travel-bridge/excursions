namespace Excursions.Application.IntegrationEvents;

public record PaidExcursionBookedIntegrationEvent(int BookingId, string TouristId) : IIntegrationEvent
{
    public string GetTopic() => Topics.PaidExcursionBooked;
}