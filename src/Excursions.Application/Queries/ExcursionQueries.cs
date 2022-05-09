using Excursions.Application.Responses;
using Npgsql;
using SqlKata.Compilers;
using SqlKata.Execution;

namespace Excursions.Application.Queries;

public class ExcursionQueries : IExcursionQueries
{
    private readonly string _connectionString;

    public ExcursionQueries(string connectionString)
    {
        _connectionString = connectionString;
    }
    
    public async Task<ExcursionResponse?> GetByIdAsync(int id)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        var compiler = new PostgresCompiler();
        var queryFactory = new QueryFactory(connection, compiler);

        var placesBookedSubQuery = queryFactory
            .Query("excursion.Booking as b")
            .WhereColumns("b.ExcursionId", "=", "e.Id")
            .Where("b.Status", "!=", "Rejected")
            .AsCount();
        
        var excursionQuery = queryFactory
            .Query("excursion.Excursion as e")
            .Select(
                "e.Id",
                "e.Name",
                "e.Description",
                "e.DateTimeUtc",
                "e.PlacesCount",
                "e.PricePerPlace",
                "e.GuideId",
                "e.Status")
            .Select(placesBookedSubQuery, "PlacesBooked")
            .Where("e.Id", "=", id);

        var excursion = await excursionQuery.FirstOrDefaultAsync<ExcursionResponse>();
        return excursion;
    }

    public async Task<PageableResponse<ExcursionResponse>> GetAsync(
        int skip = 0,
        int take = 20)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        var compiler = new PostgresCompiler();
        var queryFactory = new QueryFactory(connection, compiler);

        var placesBookedSubQuery = queryFactory
            .Query("excursion.Booking as b")
            .WhereColumns("b.ExcursionId", "=", "e.Id")
            .Where("b.Status", "!=", "Rejected")
            .AsCount();

        var excursionQuery = queryFactory
            .Query("excursion.Excursion as e")
            .Select(
                "e.Id",
                "e.Name",
                "e.Description",
                "e.DateTimeUtc",
                "e.PlacesCount",
                "e.PricePerPlace",
                "e.GuideId",
                "e.Status")
            .Select(placesBookedSubQuery, "PlacesBooked")
            .Skip(skip)
            .Take(take);

        var totalQuery = queryFactory
            .Query("excursion.Excursion as e")
            .AsCount();

        var excursions = (await excursionQuery.GetAsync<ExcursionResponse>()).ToList().AsReadOnly();
        var total = await totalQuery.CountAsync<int>();
        var response = new PageableResponse<ExcursionResponse> { Collection = excursions, Total = total };
        return response;
    }

    public async Task<PageableResponse<ExcursionResponse>> GetByGuideIdAsync(
        string guideId,
        int skip = 0,
        int take = 20)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        var compiler = new PostgresCompiler();
        var queryFactory = new QueryFactory(connection, compiler);

        var placesBookedSubQuery = queryFactory
            .Query("excursion.Booking as b")
            .WhereColumns("b.ExcursionId", "=", "e.Id")
            .Where("b.Status", "!=", "Rejected")
            .AsCount();

        var excursionQuery = queryFactory
            .Query("excursion.Excursion as e")
            .Select(
                "e.Id",
                "e.Name",
                "e.Description",
                "e.DateTimeUtc",
                "e.PlacesCount",
                "e.PricePerPlace",
                "e.GuideId",
                "e.Status")
            .Select(placesBookedSubQuery, "PlacesBooked")
            .Where("e.GuideId", "=", guideId)
            .Skip(skip)
            .Take(take);

        var totalQuery = queryFactory
            .Query("excursion.Excursion as e")
            .Where("e.GuideId", "=", guideId)
            .AsCount();

        var excursions = (await excursionQuery.GetAsync<ExcursionResponse>()).ToList().AsReadOnly();
        var total = await totalQuery.CountAsync<int>();
        var response = new PageableResponse<ExcursionResponse> { Collection = excursions, Total = total };
        return response;
    }
}