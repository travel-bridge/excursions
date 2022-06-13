namespace Excursions.Application.Events;

public record PaidExcursionBookingApprovedEvent(int BookingId) : IEvent
{
    public string GetTopic() => Topics.PaidExcursionBookingApproved;
}