using Accounting.Application.DeferredRevenue.Commands;
using FSH.Framework.Infrastructure.Auth.Policy;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

// Endpoint for updating a deferred revenue
namespace Accounting.Infrastructure.Endpoints.DeferredRevenue.v1;

public static class DeferredRevenueUpdateEndpoint
{
    internal static RouteHandlerBuilder MapDeferredRevenueUpdateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id}/recognize", async (DefaultIdType id, RecognizeDeferredRevenueCommand command, ISender mediator) =>
            {
                command.Id = id;
                await mediator.Send(command).ConfigureAwait(false);
                return Results.NoContent();
            })
            .WithName(nameof(DeferredRevenueUpdateEndpoint))
            .WithSummary("Recognize deferred revenue")
            .WithDescription("Recognizes deferred revenue for a given entry")
            .RequirePermission("Permissions.Accounting.Update")
            .MapToApiVersion(1);
    }
}
