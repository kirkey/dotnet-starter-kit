using Accounting.Application.TrialBalance.Search.v1;

namespace Accounting.Infrastructure.Endpoints.TrialBalance.v1;

/// <summary>
/// Endpoint for searching trial balance reports.
/// </summary>
public static class TrialBalanceSearchEndpoint
{
    internal static RouteHandlerBuilder MapTrialBalanceSearchEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (TrialBalanceSearchRequest request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(TrialBalanceSearchEndpoint))
            .WithSummary("Search trial balance reports")
            .WithDescription("Searches trial balance reports with filtering and pagination")
            .Produces<PagedList<TrialBalanceSearchResponse>>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .RequirePermission("Permissions.Accounting.View")
            .MapToApiVersion(1);
    }
}
