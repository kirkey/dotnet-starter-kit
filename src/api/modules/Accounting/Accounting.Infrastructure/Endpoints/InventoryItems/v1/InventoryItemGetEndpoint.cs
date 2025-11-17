using Accounting.Application.InventoryItems.Get;
using Accounting.Application.InventoryItems.Responses;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.InventoryItems.v1;

public static class InventoryItemGetEndpoint
{
    internal static RouteHandlerBuilder MapInventoryItemGetEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetInventoryItemRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(InventoryItemGetEndpoint))
            .WithSummary("Get inventory item by ID")
            .WithDescription("Gets the details of an inventory item by its ID")
            .Produces<InventoryItemResponse>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}

