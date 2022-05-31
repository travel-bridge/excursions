namespace Excursions.Application.IntegrationEvents;

public record PaidExcursionBookingApprovedIntegrationEvent(int BookingId) : IIntegrationEvent
{
    public string GetTopic() => Topics.PaidExcursionBookingApproved;
}