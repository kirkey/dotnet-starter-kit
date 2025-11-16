using Shared.Authorization;
using FSH.Starter.WebApi.HumanResources.Application.PerformanceReviews.Acknowledge.v1;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.PerformanceReviews.v1;

/// <summary>
/// Endpoint for acknowledging a performance review.
/// </summary>
public static class AcknowledgePerformanceReviewEndpoint
{
    internal static RouteHandlerBuilder MapAcknowledgePerformanceReviewEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id}/acknowledge", async (DefaultIdType id, ISender mediator) =>
            {
                var request = new AcknowledgePerformanceReviewCommand(id);
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(AcknowledgePerformanceReviewEndpoint))
            .WithSummary("Acknowledges a performance review")
            .WithDescription("Employee acknowledges receipt of the performance review. Can add comments or disputes.")
            .Produces<AcknowledgePerformanceReviewResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Acknowledge, FshResources.Employees))
            .MapToApiVersion(1);
    }
}

