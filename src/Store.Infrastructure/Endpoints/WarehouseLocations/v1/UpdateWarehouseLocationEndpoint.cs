using Store.Application.WarehouseLocations.Update.v1;
using MediatR;

namespace Store.Infrastructure.Endpoints.WarehouseLocations.v1;

public static class UpdateWarehouseLocationEndpoint
{
    internal static RouteHandlerBuilder MapUpdateWarehouseLocationEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPut("/{id:guid}", async (DefaultIdType id, UpdateWarehouseLocationCommand command, ISender sender) =>
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
