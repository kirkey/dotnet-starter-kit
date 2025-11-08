using Accounting.Application.DeferredRevenues.Update.v1;

namespace Accounting.Infrastructure.Endpoints.DeferredRevenue.v1;

public static class DeferredRevenueUpdateEndpoint
{
    internal static RouteHandlerBuilder MapDeferredRevenueUpdateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id:guid}", async (DefaultIdType id, UpdateDeferredRevenueCommand command, ISender mediator) =>
            {
                if (id != command.Id) return Results.BadRequest("ID in URL does not match ID in request body.");
                var revenueId = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(new { Id = revenueId });
            })
            .WithName(nameof(DeferredRevenueUpdateEndpoint))
            .WithSummary("Update deferred revenue")
            .WithDescription("Updates a deferred revenue entry details")
            .Produces<object>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission("Permissions.Accounting.Update")
            .MapToApiVersion(1);
    }
}

