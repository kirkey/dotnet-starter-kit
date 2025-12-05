using Accounting.Application.Vendors.Delete.v1;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.Vendors.v1;

public static class VendorDeleteEndpoint
{
    internal static RouteHandlerBuilder MapVendorDeleteEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapDelete("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                await mediator.Send(new DeleteVendorCommand(id)).ConfigureAwait(false);
                return Results.NoContent();
            })
            .WithName(nameof(VendorDeleteEndpoint))
            .WithSummary("delete vendor by id")
            .WithDescription("delete vendor by id")
            .Produces(StatusCodes.Status204NoContent)
            .RequirePermission(FshPermission.NameFor(FshActions.Delete, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}
