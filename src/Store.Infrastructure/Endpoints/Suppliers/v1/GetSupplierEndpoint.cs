using FSH.Starter.WebApi.Store.Application.Suppliers.Get.v1;

namespace Store.Infrastructure.Endpoints.Suppliers.v1;

public static class GetSupplierEndpoint
{
    internal static RouteHandlerBuilder MapGetSupplierEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (DefaultIdType id, ISender sender) =>
        {
            var result = await sender.Send(new GetSupplierRequest(id)).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName(nameof(GetSupplierEndpoint))
        .WithSummary("Get a supplier")
        .WithDescription("Retrieves a supplier by id")
        .Produces<SupplierResponse>()
        .RequirePermission("Permissions.Store.View")
        .MapToApiVersion(1);
    }
}
