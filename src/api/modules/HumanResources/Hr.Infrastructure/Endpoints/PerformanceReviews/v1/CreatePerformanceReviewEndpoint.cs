using Shared.Authorization;
namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.PerformanceReviews.v1;
            
using FSH.Starter.WebApi.HumanResources.Application.PerformanceReviews.Create.v1;

public static class CreatePerformanceReviewEndpoint
{
    /// <summary>
    /// Endpoint for creating a new performance review.
    /// </summary>
    internal static RouteHandlerBuilder MapCreatePerformanceReviewEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/", async (CreatePerformanceReviewCommand request, ISender mediator) =>
        {
            var response = await mediator.Send(request).ConfigureAwait(false);
            return Results.CreatedAtRoute(nameof(GetPerformanceReviewEndpoint), new { id = response.Id }, response);
        })
        .MapToApiVersion(1)
        .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Employees))
        .Produces<CreatePerformanceReviewResponse>(StatusCodes.Status201Created)
        .WithDescription("Creates a new performance review for an employee. Requires manager/reviewer assignment.")
        .WithSummary("Creates a new performance review")
        .WithName(nameof(CreatePerformanceReviewEndpoint));
    }
}
