namespace Store.Infrastructure.Endpoints.v1;

public static class GetWarehouseEndpoint
{
    internal static RouteHandlerBuilder MapGetWarehouseEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/warehouses/{id:guid}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new FSH.Starter.WebApi.Store.Application.Warehouses.Get.v1.GetWarehouseQuery(id)).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName("GetWarehouse")
        .WithSummary("Get warehouse by ID")
        .WithDescription("Retrieves a warehouse by its unique identifier")
        .MapToApiVersion(1);
    }
}

