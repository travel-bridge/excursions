using Excursions.Domain.Aggregates.ExcursionAggregate;
using Microsoft.EntityFrameworkCore;

namespace Excursions.Infrastructure;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("excursion");

        modelBuilder.Entity<Excursion>().ToTable("Excursion");
        modelBuilder.Entity<Booking>().ToTable("Booking");

        modelBuilder.Entity<Excursion>().Property(x => x.Status).HasConversion(
            x => x.ToString(),
            x => Enum.Parse<ExcursionStatus>(x));

        modelBuilder.Entity<Booking>().Property(x => x.Status).HasConversion(
            x => x.ToString(),
            x => Enum.Parse<BookingStatus>(x));
        
        modelBuilder.Entity<Excursion>().Property(x => x.DateTimeUtc).HasConversion(
            x => DateTime.SpecifyKind(x, DateTimeKind.Utc),
            x => DateTime.SpecifyKind(x, DateTimeKind.Utc));
        
        modelBuilder.Entity<Excursion>().Property(x => x.CreateDateTimeUtc).HasConversion(
            x => DateTime.SpecifyKind(x, DateTimeKind.Utc),
            x => DateTime.SpecifyKind(x, DateTimeKind.Utc));
        
        modelBuilder.Entity<Excursion>().Property(x => x.UpdateDateTimeUtc).HasConversion(
            x => x.HasValue ? DateTime.SpecifyKind(x.Value, DateTimeKind.Utc) : (DateTime?)null,
            x => x.HasValue ? DateTime.SpecifyKind(x.Value, DateTimeKind.Utc) : null);
        
        modelBuilder.Entity<Booking>().Property(x => x.CreateDateTimeUtc).HasConversion(
            x => DateTime.SpecifyKind(x, DateTimeKind.Utc),
            x => DateTime.SpecifyKind(x, DateTimeKind.Utc));
        
        modelBuilder.Entity<Booking>().Property(x => x.UpdateDateTimeUtc).HasConversion(
            x => x.HasValue ? DateTime.SpecifyKind(x.Value, DateTimeKind.Utc) : (DateTime?)null,
            x => x.HasValue ? DateTime.SpecifyKind(x.Value, DateTimeKind.Utc) : null);
    }
}