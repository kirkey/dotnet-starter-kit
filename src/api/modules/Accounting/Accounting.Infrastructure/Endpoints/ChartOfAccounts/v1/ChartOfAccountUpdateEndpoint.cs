using Accounting.Application.ChartOfAccounts.Update.v1;
using FSH.Framework.Infrastructure.Auth.Policy;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Accounting.Infrastructure.Endpoints.ChartOfAccounts.v1;

public static class ChartOfAccountUpdateEndpoint
{
    internal static RouteHandlerBuilder MapChartOfAccountUpdateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id:guid}", async (DefaultIdType id, ChartOfAccountUpdateRequest request, ISender mediator) =>
            {
                if (id != request.Id) return Results.BadRequest();
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(ChartOfAccountUpdateEndpoint))
            .WithSummary("update a chart of account")
            .WithDescription("update a chart of account")
            .Produces<DefaultIdType>()
            .RequirePermission("Permissions.Accounting.Update")
            .MapToApiVersion(1);
    }
}
