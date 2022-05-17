using Excursions.Domain.Exceptions;
using FluentValidation;

namespace Excursions.Domain.Aggregates.ExcursionAggregate;

internal class ExcursionValidator : AbstractValidator<Excursion>
{
    private static readonly string[] Location = { "excursion" };
    
    public ExcursionValidator()
    {
        var nameLocation = new List<string>(Location) { "name" };
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithState(_ => new ValidationMessage(
                nameLocation,
                "Validation:ExcursionNameNotEmptyError"))
            .MaximumLength(64)
            .WithState(_ => new ValidationMessage(
                nameLocation,
                "Validation:ExcursionNameMaximumLengthError"));

        var descriptionLocation = new List<string>(Location) { "description" };
        RuleFor(x => x.Description)
            .MaximumLength(512)
            .WithState(_ => new ValidationMessage(
                descriptionLocation,
                "Validation:ExcursionDescriptionMaximumLengthError"));

        var dateTimeUtcLocation = new List<string>(Location) { "dateTimeUtc" };
        RuleFor(x => x.DateTimeUtc)
            .Must(x => x != default)
            .WithState(_ => new ValidationMessage(
                dateTimeUtcLocation,
                "Validation:ExcursionDateTimeUtcNotDefaultError"));
        
        var placesCountLocation = new List<string>(Location) { "placesCount" };
        RuleFor(x => x.PlacesCount)
            .GreaterThan(0)
            .WithState(_ => new ValidationMessage(
                placesCountLocation,
                "Validation:ExcursionPlacesCountGreaterThanError"));

        var pricePerPlaceLocation = new List<string>(Location) { "pricePerPlace" };
        RuleFor(x => x.PricePerPlace)
            .GreaterThan(0)
            .WithState(_ => new ValidationMessage(
                pricePerPlaceLocation,
                "Validation:ExcursionPricePerPlaceGreaterThanError"));

        RuleFor(x => x.GuideId)
            .NotEmpty()
            .WithState(_ => new InvalidRequestMessage("GuideId should not be empty."))
            .MaximumLength(64)
            .WithState(_ => new InvalidRequestMessage("GuideId length should be less then or equal to 64."));

        RuleFor(x => x.CreateDateTimeUtc)
            .Must(x => x != default)
            .WithState(_ => new InvalidRequestMessage("CreateDateTimeUtc should not be default."));

        When(x => x.UpdateDateTimeUtc is not null, () =>
        {
            RuleFor(x => x.UpdateDateTimeUtc)
                .Must(x => x != default)
                .WithState(_ => new InvalidRequestMessage("CreateDateTimeUtc should not be default."));
        });
    }
}