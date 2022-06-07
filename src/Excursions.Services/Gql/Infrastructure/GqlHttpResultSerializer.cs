using System.Net;
using Excursions.Domain.Exceptions;
using HotChocolate.AspNetCore.Serialization;
using HotChocolate.Execution;

namespace Excursions.Api.Gql.Infrastructure;

public class GqlHttpResultSerializer : DefaultHttpResultSerializer
{
    public override HttpStatusCode GetStatusCode(IExecutionResult result)
    {
        if (result.Errors is null || result.Errors.Count == 0)
            return base.GetStatusCode(result);

        return result.Errors[0].Exception switch
        {
            ExceptionBase exceptionBase => (HttpStatusCode)exceptionBase.StatusCode,
            _ => base.GetStatusCode(result)
        };
    }
}