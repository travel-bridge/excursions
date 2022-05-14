using System.Security.Claims;

namespace Excursions.Api.Infrastructure;

public static class ClaimsPrincipalExtensions
{
    public static string GetUserId(this ClaimsPrincipal claimsPrincipal) =>
        claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);
}