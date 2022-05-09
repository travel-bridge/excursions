using Excursions.Domain.Exceptions;

namespace Excursions.Domain.Aggregates.ExcursionAggregate;

public class Booking : EntityBase<int>
{
    private static readonly BookingValidator Validator = new();
    
    protected Booking(string touristId)
    {
        TouristId = touristId;
        Status = BookingStatus.Booked;
        CreateDateTimeUtc = DateTime.UtcNow;
    }

    public string TouristId { get; private set; }
    
    public BookingStatus Status { get; private set; }
    
    public DateTime CreateDateTimeUtc { get; private set; }

    public DateTime? UpdateDateTimeUtc { get; private set; }
    
    public int ExcursionId { get; private set; }

    public static Booking Create(string touristId)
    {
        var booking = new Booking(touristId);
        Validator.ValidateEntityAndThrow(booking);
        return booking;
    }
    
    public void Approve()
    {
        if (Status != BookingStatus.Booked)
            throw new DomainException("Domain:BookingApproveWhenNotBookedError");

        Status = BookingStatus.Approved;
        UpdateDateTimeUtc = DateTime.UtcNow;
    }

    public void Reject()
    {
        if (Status != BookingStatus.Booked)
            throw new DomainException("Domain:BookingRejectWhenNotBookedError");
        
        Status = BookingStatus.Rejected;
        UpdateDateTimeUtc = DateTime.UtcNow;
    }
}