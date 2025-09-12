using MediatR;
using Store.Application.WarehouseLocations.Update.v1;

namespace Store.Infrastructure.Endpoints.v1;

public static class UpdateWarehouseLocationEndpoint
{
    internal static RouteHandlerBuilder MapUpdateWarehouseLocationEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPut("/warehouse-locations/{id:guid}", async (DefaultIdType id, UpdateWarehouseLocationCommand command, ISender sender) =>
        {
            if (id != command.Id) return Results.BadRequest("ID mismatch");
            var result = await sender.Send(command).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName("UpdateWarehouseLocation")
        .WithSummary("Update warehouse location")
        .WithDescription("Updates an existing warehouse location with the provided details")
        .MapToApiVersion(1);
    }
}

namespace FSH.Starter.WebApi.Store.Infrastructure.Endpoints.v1;

public static class CreateWarehouseLocationEndpoint
{
    internal static RouteHandlerBuilder MapCreateWarehouseLocationEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/warehouse-locations", async (FSH.Starter.WebApi.Store.Application.WarehouseLocations.Create.v1.CreateWarehouseLocationCommand command, ISender sender) =>
        {
            var result = await sender.Send(command).ConfigureAwait(false);
            return Results.Created($"/warehouse-locations/{result.Id}", result);
        })
        .WithName("CreateWarehouseLocation")
        .WithSummary("Create a new warehouse location")
        .WithDescription("Creates a new warehouse location for storing items")
        .MapToApiVersion(1);
    }
}
