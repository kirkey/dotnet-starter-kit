using FSH.Starter.WebApi.Store.Application.InventoryTransfers.Create.v1;
using Shared.Authorization;

namespace Store.Infrastructure.Endpoints.InventoryTransfers.v1;

public static class CreateInventoryTransferEndpoint
{
    internal static RouteHandlerBuilder MapCreateInventoryTransferEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/", async (CreateInventoryTransferCommand request, ISender mediator) =>
        {
            var response = await mediator.Send(request).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName(nameof(CreateInventoryTransferEndpoint))
        .WithSummary("Create a new inventory transfer")
        .WithDescription("Creates a new transfer between warehouses")
        .Produces<CreateInventoryTransferResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Warehouse))
        .MapToApiVersion(1);
    }
}
