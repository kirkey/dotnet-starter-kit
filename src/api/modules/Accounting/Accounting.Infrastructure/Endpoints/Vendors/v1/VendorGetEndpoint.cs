using Accounting.Application.Vendors.Get.v1;

namespace Accounting.Infrastructure.Endpoints.Vendors.v1;

public static class VendorGetEndpoint
{
    internal static RouteHandlerBuilder MapVendorGetEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new VendorGetQuery(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(VendorGetEndpoint))
            .WithSummary("get a vendor by id")
            .WithDescription("get a vendor by id")
            .Produces<VendorGetResponse>()
            .RequirePermission("Permissions.Accounting.View")
            .MapToApiVersion(1);
    }
}
