using Excursions.Domain.Aggregates.ExcursionAggregate;
using Microsoft.EntityFrameworkCore;

namespace Excursions.Infrastructure.Database.Repositories;

public class ExcursionRepository : RepositoryBase<DataContext, Excursion, int>, IExcursionRepository
{
    public ExcursionRepository(DataContext context) : base(context)
    {
        QuerySet = base.QuerySet.Include(x => x.Booking);
    }

    protected override IQueryable<Excursion> QuerySet { get; }
}