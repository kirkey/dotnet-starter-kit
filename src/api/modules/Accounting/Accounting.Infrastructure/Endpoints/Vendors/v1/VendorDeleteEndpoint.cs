using Accounting.Application.Vendors.Delete.v1;

namespace Accounting.Infrastructure.Endpoints.Vendors.v1;

public static class VendorDeleteEndpoint
{
    internal static RouteHandlerBuilder MapVendorDeleteEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapDelete("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                await mediator.Send(new VendorDeleteCommand(id)).ConfigureAwait(false);
                return Results.NoContent();
            })
            .WithName(nameof(VendorDeleteEndpoint))
            .WithSummary("delete vendor by id")
            .WithDescription("delete vendor by id")
            .Produces(StatusCodes.Status204NoContent)
            .RequirePermission("Permissions.Accounting.Delete")
            .MapToApiVersion(1);
    }
}
