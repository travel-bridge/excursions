namespace Excursions.Application.Responses;

public class PageableResponse<TResponse>
{
    public IReadOnlyCollection<TResponse> Collection { get; init; } = null!;

    public int Total { get; init; }
}