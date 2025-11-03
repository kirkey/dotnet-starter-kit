using Accounting.Application.Bills.Get.v1;
using Accounting.Application.Bills.Search.v1;
using Asp.Versioning;

namespace Accounting.Infrastructure.Endpoints.Bills.v1;

/// <summary>
/// Endpoint for searching bills with filters and pagination.
/// </summary>
public static class SearchBillsEndpoint
{
    internal static RouteHandlerBuilder MapSearchBillsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async ([FromBody] SearchBillsCommand command, ISender mediator) =>
            {
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(SearchBillsEndpoint))
            .WithSummary("Search bills")
            .WithDescription("Search and filter bills with pagination.")
            .Produces<PagedList<BillResponse>>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .RequirePermission("Permissions.Bills.View")
            .MapToApiVersion(new ApiVersion(1, 0));
    }
}
