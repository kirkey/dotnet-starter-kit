using Accounting.Application.Vendors.Get.v1;

namespace Accounting.Infrastructure.Endpoints.Vendors.v1;

public static class VendorGetEndpoint
{
    internal static RouteHandlerBuilder MapVendorGetEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new VendorGetRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(VendorGetEndpoint))
            .WithSummary("Get a vendor by ID")
            .WithDescription("Retrieves a vendor by its unique identifier")
            .Produces<VendorGetResponse>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission("Permissions.Accounting.View")
            .MapToApiVersion(1);
    }
}
