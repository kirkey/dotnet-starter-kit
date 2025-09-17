using Accounting.Application.Inventory.Commands;

namespace Accounting.Infrastructure.Endpoints.Inventory.v1;

public static class CreateInventoryItemEndpoint
{
    internal static RouteHandlerBuilder MapCreateInventoryItemEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/items", async (CreateInventoryItemCommand request, ISender mediator) =>
            {
                var id = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(id);
            })
            .WithName(nameof(CreateInventoryItemEndpoint))
            .WithSummary("Create inventory item")
            .WithDescription("Creates a new inventory item")
            .Produces<DefaultIdType>()
            .RequirePermission("Permissions.Accounting.Create")
            .MapToApiVersion(1);
    }
}

