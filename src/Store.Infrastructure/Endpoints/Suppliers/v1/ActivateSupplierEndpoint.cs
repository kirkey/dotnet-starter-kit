using FSH.Starter.WebApi.Store.Application.Suppliers.Activate.v1;

namespace Store.Infrastructure.Endpoints.Suppliers.v1;

public static class ActivateSupplierEndpoint
{
    internal static RouteHandlerBuilder MapActivateSupplierEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/{id:guid}/activate", async (DefaultIdType id, ISender sender) =>
            {
                var response = await sender.Send(new ActivateSupplierCommand(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(ActivateSupplierEndpoint))
            .WithSummary("Activate a supplier")
            .WithDescription("Activates a supplier to allow transactions")
            .Produces<ActivateSupplierResponse>()
            .RequirePermission("Permissions.Store.Update")
            .MapToApiVersion(1);
    }
}
