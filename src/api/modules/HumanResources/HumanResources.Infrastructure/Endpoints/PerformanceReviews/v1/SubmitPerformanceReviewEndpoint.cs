using Shared.Authorization;
using FSH.Starter.WebApi.HumanResources.Application.PerformanceReviews.Submit.v1;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.PerformanceReviews.v1;

/// <summary>
/// Endpoint for submitting a performance review.
/// </summary>
public static class SubmitPerformanceReviewEndpoint
{
    internal static RouteHandlerBuilder MapSubmitPerformanceReviewEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id}/submit", async (DefaultIdType id, ISender mediator) =>
            {
                var request = new SubmitPerformanceReviewCommand(id);
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(SubmitPerformanceReviewEndpoint))
            .WithSummary("Submits a performance review")
            .WithDescription("Submits a draft performance review to the employee. Once submitted, review cannot be edited by reviewer.")
            .Produces<SubmitPerformanceReviewResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Submit, FshResources.Employees))
            .MapToApiVersion(1);
    }
}

