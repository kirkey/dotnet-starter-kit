using FSH.Starter.WebApi.Store.Application.WarehouseLocations.Update.v1;

namespace Store.Infrastructure.Endpoints.WarehouseLocations.v1;

public static class UpdateWarehouseLocationEndpoint
{
    internal static RouteHandlerBuilder MapUpdateWarehouseLocationEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPut("/{id:guid}", async (DefaultIdType id, UpdateWarehouseLocationCommand request, ISender sender) =>
        {
            var command = request with { Id = id };
            var result = await sender.Send(command).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName(nameof(UpdateWarehouseLocationEndpoint))
        .WithSummary("Update warehouse location")
        .WithDescription("Updates an existing warehouse location with the provided details")
        .Produces<UpdateWarehouseLocationResponse>()
        .MapToApiVersion(1);
    }
}
