namespace Store.Infrastructure.Endpoints.v1;

public static class UpdateWarehouseEndpoint
{
    internal static RouteHandlerBuilder MapUpdateWarehouseEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPut("/warehouses/{id:guid}", async (Guid id, FSH.Starter.WebApi.Store.Application.Warehouses.Update.v1.UpdateWarehouseCommand command, ISender sender) =>
        {
            if (id != command.Id) return Results.BadRequest("ID mismatch");
            await sender.Send(command).ConfigureAwait(false);
            return Results.NoContent();
        })
        .WithName("UpdateWarehouse")
        .WithSummary("Update warehouse")
        .WithDescription("Updates an existing warehouse with the provided details")
        .MapToApiVersion(1);
    }
}

