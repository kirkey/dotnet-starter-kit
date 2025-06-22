using Accounting.Application.ChartOfAccounts.Delete.v1;
using FSH.Framework.Infrastructure.Auth.Policy;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Accounting.Infrastructure.Endpoints.ChartOfAccount.v1;

public static class ChartOfAccountDeleteEndpoint
{
    internal static RouteHandlerBuilder MapChartOfAccountDeleteEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapDelete("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                await mediator.Send(new ChartOfAccountDeleteRequest(id)).ConfigureAwait(false);
                return Results.NoContent();
            })
            .WithName(nameof(ChartOfAccountDeleteEndpoint))
            .WithSummary("delete chart of account by id")
            .WithDescription("delete chart of account by id")
            .Produces(StatusCodes.Status204NoContent)
            .RequirePermission("Permissions.Accounting.Delete")
            .MapToApiVersion(1);
    }
}
