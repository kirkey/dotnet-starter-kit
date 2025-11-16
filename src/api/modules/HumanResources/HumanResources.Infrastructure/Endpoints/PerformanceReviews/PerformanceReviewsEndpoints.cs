using FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.PerformanceReviews.v1;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.PerformanceReviews;

/// <summary>
/// Endpoint routes for managing performance reviews.
/// </summary>
public static class PerformanceReviewsEndpoints
{
    internal static IEndpointRouteBuilder MapPerformanceReviewsEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/performance-reviews")
            .WithTags("Performance Reviews")
            .WithDescription("Endpoints for managing employee performance reviews with submission and acknowledgment workflows");

        group.MapCreatePerformanceReviewEndpoint();
        group.MapGetPerformanceReviewEndpoint();
        group.MapUpdatePerformanceReviewEndpoint();
        group.MapSearchPerformanceReviewsEndpoint();
        group.MapSubmitPerformanceReviewEndpoint();
        group.MapAcknowledgePerformanceReviewEndpoint();
        group.MapCompletePerformanceReviewEndpoint();

        return app;
    }
}

