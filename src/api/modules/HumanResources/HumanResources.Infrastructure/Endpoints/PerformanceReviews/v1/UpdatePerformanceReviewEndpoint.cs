using Shared.Authorization;
using FSH.Starter.WebApi.HumanResources.Application.PerformanceReviews.Update.v1;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.PerformanceReviews.v1;

/// <summary>
/// Endpoint for updating a performance review.
/// </summary>
public static class UpdatePerformanceReviewEndpoint
{
    internal static RouteHandlerBuilder MapUpdatePerformanceReviewEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id:guid}", async (DefaultIdType id, UpdatePerformanceReviewCommand request, ISender mediator) =>
            {
                var updateRequest = request with { Id = id };
                var response = await mediator.Send(updateRequest).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(UpdatePerformanceReviewEndpoint))
            .WithSummary("Updates a performance review")
            .WithDescription("Updates a performance review. Allowed only before submission. Cannot update submitted reviews.")
            .Produces<UpdatePerformanceReviewResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Employees))
            .MapToApiVersion(1);
    }
}

