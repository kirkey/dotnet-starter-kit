using FSH.Starter.WebApi.Store.Application.Warehouses.Delete.v1;

namespace Store.Infrastructure.Endpoints.Warehouses.v1;

public static class DeleteWarehouseEndpoint
{
    internal static RouteHandlerBuilder MapDeleteWarehouseEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/{id:guid}", async (DefaultIdType id, ISender sender) =>
        {
            await sender.Send(new DeleteWarehouseCommand(id)).ConfigureAwait(false);
            return Results.NoContent();
        })
        .WithName("DeleteWarehouse")
        .WithSummary("Delete warehouse")
        .WithDescription("Deletes a warehouse by its unique identifier")
        .MapToApiVersion(1);
    }
}
