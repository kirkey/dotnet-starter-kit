using FSH.Starter.WebApi.HumanResources.Application.BenefitEnrollments.Search.v1;
using FSH.Starter.WebApi.HumanResources.Application.BenefitEnrollments.Get.v1;
using Shared.Authorization;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.BenefitEnrollments.v1;

/// <summary>
/// Endpoint for searching benefit enrollments with filtering and pagination.
/// </summary>
public static class SearchBenefitEnrollmentsEndpoint
{
    internal static RouteHandlerBuilder MapSearchBenefitEnrollmentsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (SearchBenefitEnrollmentsRequest request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(SearchBenefitEnrollmentsEndpoint))
            .WithSummary("Searches benefit enrollments")
            .WithDescription("Searches and filters benefit enrollments by employee, benefit, status with pagination support.")
            .Produces<PagedList<BenefitEnrollmentResponse>>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Benefits))
            .MapToApiVersion(1);
    }
}

