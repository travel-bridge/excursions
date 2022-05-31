using Excursions.Application;
using Excursions.Infrastructure;
using Excursions.Worker.Workers;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddApplication(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddHostedService<RejectExpiredBookingWorker>();
builder.Services.AddHostedService<ApprovePaidBookingWorker>();
builder.Services.AddHealthChecks()
    .AddNpgSql(builder.Configuration.GetConnectionString("ExcursionsDatabase")
        ?? throw new InvalidOperationException("Connection string is not configured."));

var app = builder.Build();
app.UseRouting();
app.UseApplication();
app.MapHealthChecks("/health");

await app.RunAsync();