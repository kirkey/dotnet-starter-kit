using Accounting.Application.Vendors.Create.v1;

namespace Accounting.Infrastructure.Endpoints.Vendors.v1;

public static class VendorCreateEndpoint
{
    internal static RouteHandlerBuilder MapVendorCreateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (VendorCreateCommand command, ISender mediator) =>
            {
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(VendorCreateEndpoint))
            .WithSummary("create a vendor")
            .WithDescription("create a vendor")
            .WithTags("Vendors")
            .Produces<VendorCreateResponse>()
            .RequirePermission("Permissions.Accounting.Create")
            .MapToApiVersion(1);
    }
}
