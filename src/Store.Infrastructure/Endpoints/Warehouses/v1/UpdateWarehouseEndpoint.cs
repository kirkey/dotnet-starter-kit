using FSH.Starter.WebApi.Store.Application.Warehouses.Update.v1;

namespace Store.Infrastructure.Endpoints.Warehouses.v1;

public static class UpdateWarehouseEndpoint
{
    internal static RouteHandlerBuilder MapUpdateWarehouseEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPut("/{id:guid}", async (DefaultIdType id, UpdateWarehouseCommand command, ISender sender) =>
        {
            if (id != command.Id) return Results.BadRequest("ID mismatch");
            var result = await sender.Send(command).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName("UpdateWarehouse")
        .WithSummary("Update warehouse")
        .WithDescription("Updates an existing warehouse with the provided details")
        .MapToApiVersion(1);
    }
}
