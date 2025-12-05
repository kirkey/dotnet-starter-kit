using Accounting.Application.Vendors.Update.v1;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.Vendors.v1;

public static class VendorUpdateEndpoint
{
    internal static RouteHandlerBuilder MapVendorUpdateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id:guid}", async (DefaultIdType id, UpdateVendorCommand request, ISender mediator) =>
            {
                var command = request with { Id = id };
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(VendorUpdateEndpoint))
            .WithSummary("Update a vendor")
            .WithDescription("Updates an existing vendor")
            .Produces<VendorUpdateResponse>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}
