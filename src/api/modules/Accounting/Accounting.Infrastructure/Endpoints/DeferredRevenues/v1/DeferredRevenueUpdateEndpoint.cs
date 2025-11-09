using Accounting.Application.DeferredRevenues.Update;

namespace Accounting.Infrastructure.Endpoints.DeferredRevenues.v1;

public static class DeferredRevenueUpdateEndpoint
{
    internal static RouteHandlerBuilder MapDeferredRevenueUpdateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id:guid}", async (DefaultIdType id, UpdateDeferredRevenueCommand command, ISender mediator) =>
            {
                if (id != command.Id)
                    return Results.BadRequest("ID mismatch");

                var result = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(new { Id = result });
            })
            .WithName(nameof(DeferredRevenueUpdateEndpoint))
            .WithSummary("Update deferred revenue")
            .WithDescription("Updates an existing deferred revenue entry (cannot update recognized revenue)")
            .Produces<object>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission("Permissions.Accounting.Update")
            .MapToApiVersion(1);
    }
}

