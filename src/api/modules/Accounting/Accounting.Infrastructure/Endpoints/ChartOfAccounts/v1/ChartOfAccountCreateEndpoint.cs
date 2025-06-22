using Accounting.Application.ChartOfAccounts.Create.v1;
using FSH.Framework.Infrastructure.Auth.Policy;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Accounting.Infrastructure.Endpoints.ChartOfAccounts.v1;

public static class ChartOfAccountCreateEndpoint
{
    internal static RouteHandlerBuilder MapChartOfAccountCreateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (ChartOfAccountCreateRequest request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(ChartOfAccountCreateEndpoint))
            .WithSummary("create a chart of account")
            .WithDescription("create a chart of account")
            .Produces<DefaultIdType>()
            .RequirePermission("Permissions.Accounting.Create")
            .MapToApiVersion(1);
    }
}
