using Excursions.Application.Responses;
using Excursions.Domain.Aggregates.ExcursionAggregate;
using Npgsql;
using SqlKata.Compilers;
using SqlKata.Execution;

namespace Excursions.Application.Queries;

public class BookingQueries : IBookingQueries
{
    private readonly string _connectionString;

    public BookingQueries(string connectionString)
    {
        _connectionString = connectionString;
    }
    
    public async Task<PageableResponse<BookingResponse>> GetByTouristAsync(
        string touristId,
        int skip = 0,
        int take = 20)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        var compiler = new PostgresCompiler();
        var queryFactory = new QueryFactory(connection, compiler);

        var bookingQuery = queryFactory
            .Query("excursion.Booking as b")
            .Select(
                "b.Id",
                "b.Status",
                "e.Id as ExcursionId",
                "e.Name as ExcursionName",
                "e.DateTimeUtc as ExcursionDateTimeUtc")
            .Join("excursion.Excursion as e", "e.Id", "b.ExcursionId")
            .Where("b.TouristId", "=", touristId)
            .Skip(skip)
            .Take(take);
        
        var totalQuery = queryFactory
            .Query("excursion.Booking as b")
            .Where("b.TouristId", "=", touristId)
            .AsCount();

        var booking = (await bookingQuery.GetAsync<BookingResponse>()).ToList().AsReadOnly();
        var total = await totalQuery.CountAsync<int>();
        var response = new PageableResponse<BookingResponse>{ Collection = booking, Total = total };
        return response;
    }

    public async Task<BookingToRejectResponse?> GetFirstToRejectAsync(
        BookingStatus status,
        DateTime dateTimeUtc)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        var compiler = new PostgresCompiler();
        var queryFactory = new QueryFactory(connection, compiler);

        var bookingQuery = queryFactory
            .Query("excursion.Booking as b")
            .Select(
                "b.ExcursionId",
                "b.TouristId")
            .Where("b.CreateDateTimeUtc", "<=", dateTimeUtc)
            .Where("b.Status", "=", status.ToString());

        var booking = await bookingQuery.FirstOrDefaultAsync<BookingToRejectResponse>();
        return booking;
    }
}