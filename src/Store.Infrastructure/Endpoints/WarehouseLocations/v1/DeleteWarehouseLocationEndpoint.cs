using FSH.Starter.WebApi.Store.Application.WarehouseLocations.Delete.v1;

namespace Store.Infrastructure.Endpoints.WarehouseLocations.v1;

/// <summary>
/// Endpoint for deleting a warehouse location.
/// </summary>
public static class DeleteWarehouseLocationEndpoint
{
    /// <summary>
    /// Maps the delete warehouse location endpoint to the route builder.
    /// </summary>
    /// <param name="endpoints">The endpoint route builder</param>
    /// <returns>Route handler builder for delete warehouse location endpoint</returns>
    internal static RouteHandlerBuilder MapDeleteWarehouseLocationEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/{id:guid}", async (DefaultIdType id, ISender sender) =>
        {
            await sender.Send(new DeleteWarehouseLocationCommand(id)).ConfigureAwait(false);
            return Results.NoContent();
        })
        .WithName("DeleteWarehouseLocation")
        .WithSummary("Delete a warehouse location")
        .WithDescription("Deletes a warehouse location by ID")
        .Produces(204)
        .MapToApiVersion(1);
    }
}
