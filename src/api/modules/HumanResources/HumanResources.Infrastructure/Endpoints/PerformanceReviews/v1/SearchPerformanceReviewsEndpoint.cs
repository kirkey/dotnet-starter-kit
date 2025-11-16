using FSH.Starter.WebApi.HumanResources.Application.PerformanceReviews.Search.v1;
using Resp = FSH.Starter.WebApi.HumanResources.Application.PerformanceReviews.Get.v1.PerformanceReviewResponse;

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
            .Produces<PagedList<Resp>>()
            .RequirePermission("Permissions.PerformanceReviews.View")
            .MapToApiVersion(1);
    }
}

