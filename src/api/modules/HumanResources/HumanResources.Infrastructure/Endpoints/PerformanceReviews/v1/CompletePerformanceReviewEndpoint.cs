using FSH.Starter.WebApi.HumanResources.Application.PerformanceReviews.Complete.v1;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.PerformanceReviews.v1;

/// <summary>
/// Endpoint for completing a performance review.
/// </summary>
public static class CompletePerformanceReviewEndpoint
{
    internal static RouteHandlerBuilder MapCompletePerformanceReviewEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id}/complete", async (DefaultIdType id, ISender mediator) =>
            {
                var request = new CompletePerformanceReviewCommand(id);
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(CompletePerformanceReviewEndpoint))
            .WithSummary("Completes a performance review")
            .WithDescription("Marks the performance review as complete. Final step after all approvals and acknowledgments.")
            .Produces<CompletePerformanceReviewResponse>()
            .RequirePermission("Permissions.PerformanceReviews.Complete")
            .MapToApiVersion(1);
    }
}

