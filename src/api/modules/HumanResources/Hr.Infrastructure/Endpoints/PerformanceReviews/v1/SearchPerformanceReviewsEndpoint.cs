using Shared.Authorization;
using FSH.Starter.WebApi.HumanResources.Application.PerformanceReviews.Search.v1;
using FSH.Starter.WebApi.HumanResources.Application.PerformanceReviews.Get.v1;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.PerformanceReviews.v1;

/// <summary>
/// Endpoint for searching performance reviews with filtering and pagination.
/// </summary>
public static class SearchPerformanceReviewsEndpoint
{
    internal static RouteHandlerBuilder MapSearchPerformanceReviewsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (SearchPerformanceReviewsRequest request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(SearchPerformanceReviewsEndpoint))
            .WithSummary("Searches performance reviews")
            .WithDescription("Searches and filters performance reviews by employee, period, status, reviewer with pagination support.")
            .Produces<PagedList<PerformanceReviewResponse>>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Employees))
            .MapToApiVersion(1);
    }
}

