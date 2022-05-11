using Excursions.Domain.Exceptions;
using FluentValidation;

namespace Excursions.Domain.Aggregates.BookingAggregate;

internal class BookingValidator : AbstractValidator<Booking>
{
    public BookingValidator()
    {
        RuleFor(x => x.TouristId)
            .NotEmpty()
            .WithState(_ => new InvalidRequestMessage("TouristId should not be empty."))
            .MaximumLength(64)
            .WithState(_ => new InvalidRequestMessage("TouristId length should be less then or equal to 64."));
    }
}