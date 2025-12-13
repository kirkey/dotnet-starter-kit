using FSH.Starter.WebApi.Store.Application.Warehouses.AssignManager.v1;

namespace Store.Infrastructure.Endpoints.Warehouses.v1;

/// <summary>
/// Endpoint for assigning a manager to a warehouse.
/// </summary>
public static class AssignWarehouseManagerEndpoint
{
    /// <summary>
    /// Maps the assign warehouse manager endpoint.
    /// </summary>
    internal static RouteHandlerBuilder MapAssignWarehouseManagerEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPut("/{id}/assign-manager", async (DefaultIdType id, AssignWarehouseManagerCommand request, ISender sender) =>
        {
            var command = request with { Id = id };
            var result = await sender.Send(command).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName(nameof(AssignWarehouseManagerEndpoint))
        .WithSummary("Assign manager to warehouse")
        .WithDescription("Assigns a new manager to an existing warehouse")
        .Produces<AssignWarehouseManagerResponse>()
        .MapToApiVersion(1);
    }
}

