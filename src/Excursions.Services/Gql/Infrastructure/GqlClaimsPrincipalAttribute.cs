using System.Security.Claims;

namespace Excursions.Api.Gql.Infrastructure;

public class GqlClaimsPrincipalAttribute : GlobalStateAttribute
{
    public GqlClaimsPrincipalAttribute() : base(nameof(ClaimsPrincipal))
    {
    }
}