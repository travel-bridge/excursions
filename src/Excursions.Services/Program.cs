using Excursions.Api.Gql;
using Excursions.Api.Infrastructure;
using Excursions.Application;
using Excursions.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
    {
        options.Authority = builder.Configuration["Security:Authority"]
            ?? throw new InvalidOperationException("Security authority are not configured.");
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(AuthorizePolicies.ReadExcursions, policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim("scope", "excursions.read");
    });
    options.AddPolicy(AuthorizePolicies.WriteExcursions, policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim("scope", "excursions.write");
    });
});

builder.Services.AddApplication(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddCors();
builder.Services.AddGql();
// TODO: Add Health Checks

var app = builder.Build();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseApplication();
app.UseCors(x=> x.SetIsOriginAllowed(_ => true).AllowCredentials().AllowAnyHeader().AllowAnyMethod());
app.MapGraphQL().AllowAnonymous();
// TODO: Map Health Checks

await app.RunAsync();
