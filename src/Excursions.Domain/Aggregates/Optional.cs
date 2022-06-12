namespace Excursions.Domain.Aggregates;

public record struct Optional<TValue>(TValue? Value, bool HasValue);