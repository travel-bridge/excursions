using Excursions.Application.Responses;

namespace Excursions.Application.Queries;

public interface IExcursionQueries
{
    Task<ExcursionResponse?> GetByIdAsync(int id);

    Task<PageableResponse<ExcursionResponse>> GetAsync(int skip = 0, int take = 20);
    
    Task<PageableResponse<ExcursionResponse>> GetByGuideIdAsync(
        string guideId,
        int skip = 0,
        int take = 20);
}