using System.Security.Claims;

namespace Excursions.Api.Gql.Infrastructure;

public static class ClaimsPrincipalExtensions
{
    public static string GetId(this ClaimsPrincipal claimsPrincipal)
    {
        // TODO: Implemet get user id extension
        return "TODO: Fake id";
    }
}