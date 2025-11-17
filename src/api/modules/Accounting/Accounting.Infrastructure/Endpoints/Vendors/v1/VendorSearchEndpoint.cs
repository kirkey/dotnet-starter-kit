using Accounting.Application.Vendors.Search.v1;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.Vendors.v1;

public static class VendorSearchEndpoint
{
    internal static RouteHandlerBuilder MapVendorSearchEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (ISender mediator, [FromBody] VendorSearchRequest request) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(VendorSearchEndpoint))
            .WithSummary("Search vendors")
            .WithDescription("Searches vendors with pagination and filtering support")
            .Produces<PagedList<VendorSearchResponse>>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}
