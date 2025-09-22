using FSH.Starter.WebApi.Store.Application.Suppliers.Deactivate.v1;

namespace Store.Infrastructure.Endpoints.Suppliers.v1;

public static class DeactivateSupplierEndpoint
{
    internal static RouteHandlerBuilder MapDeactivateSupplierEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/{id:guid}/deactivate", async (DefaultIdType id, ISender sender) =>
        {
            var response = await sender.Send(new DeactivateSupplierCommand(id)).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName(nameof(DeactivateSupplierEndpoint))
        .WithSummary("Deactivate a supplier")
        .WithDescription("Deactivates a supplier to block transactions")
        .Produces<DeactivateSupplierResponse>()
        .RequirePermission("Permissions.Store.Update")
        .MapToApiVersion(1);
    }
}


