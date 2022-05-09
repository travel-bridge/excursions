using Excursions.Domain.Exceptions;

namespace Excursions.Domain.Aggregates.ExcursionAggregate;

public class Excursion : EntityBase<int>, IAggregateRoot
{
    private static readonly ExcursionValidator Validator = new();
    
    protected Excursion(
        string name,
        string? description,
        DateTime dateTimeUtc,
        int placesCount,
        decimal pricePerPlace,
        string guideId)
    {
        Name = name;
        Description = description;
        DateTimeUtc = dateTimeUtc;
        PlacesCount = placesCount;
        PricePerPlace = pricePerPlace;
        GuideId = guideId;
        Status = ExcursionStatus.Draft;
        CreateDateTimeUtc = DateTime.UtcNow;
    }
    
    public string Name { get; private set; }

    public string? Description { get; private set; }

    public DateTime DateTimeUtc { get; private set; }

    public int PlacesCount { get; private set; }

    public decimal PricePerPlace { get; private set; }

    public string GuideId { get; private set; }
    
    public ExcursionStatus Status { get; private set; }
    
    public DateTime CreateDateTimeUtc { get; private set; }

    public DateTime? UpdateDateTimeUtc { get; private set; }
    
    private readonly List<Booking> _booking = new();
    public IReadOnlyCollection<Booking> Booking => _booking.AsReadOnly();

    public static Excursion Create(
        string name,
        string? description,
        DateTime dateTimeUtc,
        int placesCount,
        decimal pricePerPlace,
        string guideId)
    {
        var excursion = new Excursion(name, description, dateTimeUtc, placesCount, pricePerPlace, guideId);
        Validator.ValidateEntityAndThrow(excursion);
        return excursion;
    }
    
    public void Update(
        string? name,
        string? description,
        DateTime? dateTimeUtc,
        int? placesCount,
        decimal? pricePerPlace)
    {
        if (Status != ExcursionStatus.Draft)
            throw new DomainException("Domain:ExcursionUpdateWhenNotDraftError");
        
        if (name is not null)
            Name = name;

        if (description is not null)
            Description = description;

        if (dateTimeUtc.HasValue)
            DateTimeUtc = dateTimeUtc.Value;

        if (placesCount.HasValue)
            PlacesCount = placesCount.Value;

        if (pricePerPlace.HasValue)
            PricePerPlace = pricePerPlace.Value;

        UpdateDateTimeUtc = DateTime.UtcNow;
        
        Validator.ValidateEntityAndThrow(this);
    }

    public void Publish()
    {
        if (Status != ExcursionStatus.Draft)
            return;

        Status = ExcursionStatus.Published;
    }
    
    public void Book(string touristId)
    {
        if (Status != ExcursionStatus.Published)
            throw new DomainException("Domain:ExcursionBookWhenNotPublishedError");
        
        if (_booking.Count >= PlacesCount)
            throw new DomainException("Domain:ExcursionBookPlacesCountLimitError");
        
        var booking = ExcursionAggregate.Booking.Create(touristId);
        _booking.Add(booking);
    }

    public void ApproveBooking(string touristId)
    {
        if (Status != ExcursionStatus.Published)
            throw new DomainException("Domain:ExcursionApproveBookingWhenNotPublishedError");
        
        var booking = _booking.FirstOrDefault(x => x.TouristId == touristId);
        
        if (booking is null)
            throw new DomainException("Domain:ExcursionApproveBookingNotFoundError", touristId);
        
        booking.Approve();
    }
    
    public void RejectBooking(string touristId)
    {
        if (Status != ExcursionStatus.Published)
            throw new DomainException("Domain:ExcursionRejectBookingWhenNotPublishedError");
        
        var booking = _booking.FirstOrDefault(x => x.TouristId == touristId);
        
        if (booking is null)
            throw new DomainException("Domain:ExcursionRejectBookingNotFoundError", touristId);
        
        booking.Reject();
    }
}