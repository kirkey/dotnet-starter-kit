using Shared.Authorization;
using FSH.Starter.WebApi.HumanResources.Application.PerformanceReviews.Get.v1;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.PerformanceReviews.v1;

/// <summary>
/// Endpoint for retrieving a specific performance review by ID.
/// </summary>
public static class GetPerformanceReviewEndpoint
{
    internal static RouteHandlerBuilder MapGetPerformanceReviewEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var request = new GetPerformanceReviewRequest(id);
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(GetPerformanceReviewEndpoint))
            .WithSummary("Gets a performance review by ID")
            .WithDescription("Retrieves detailed information about a specific performance review including scores, feedback, and status.")
            .Produces<PerformanceReviewResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Employees))
            .MapToApiVersion(1);
    }
}

