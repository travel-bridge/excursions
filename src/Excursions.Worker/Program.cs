using Excursions.Application;
using Excursions.Infrastructure;
using Excursions.Worker.Workers;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddApplication(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddHostedService<BookingRejectionWorker>();
// TODO: Add Health Checks

var app = builder.Build();
app.UseRouting();
app.UseApplication();
// TODO: Map Health Checks

await app.RunAsync();