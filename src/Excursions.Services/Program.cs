using Excursions.Api.Gql;
using Excursions.Application;
using Excursions.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
// TODO: Add Authentication/Authorization
builder.Services.AddApplication(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddCors();
builder.Services.AddGql();
// TODO: Add Health Checks

var app = builder.Build();
app.UseRouting();
app.UseCors(x=> x.SetIsOriginAllowed(_ => true).AllowCredentials().AllowAnyHeader().AllowAnyMethod());
app.UseApplication();
app.MapGraphQL().AllowAnonymous();
// TODO: Map Health Checks

await app.RunAsync();
