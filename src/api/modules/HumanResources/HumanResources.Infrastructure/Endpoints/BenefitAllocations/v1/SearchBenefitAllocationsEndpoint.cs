using FSH.Starter.WebApi.HumanResources.Application.BenefitAllocations.Search.v1;
using FSH.Starter.WebApi.HumanResources.Application.BenefitAllocations.Get.v1;
using Shared.Authorization;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.BenefitAllocations.v1;

/// <summary>
/// Endpoint for searching benefit allocations with filtering and pagination.
/// </summary>
public static class SearchBenefitAllocationsEndpoint
{
    internal static RouteHandlerBuilder MapSearchBenefitAllocationsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (SearchBenefitAllocationsRequest request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(SearchBenefitAllocationsEndpoint))
            .WithSummary("Searches benefit allocations")
            .WithDescription("Searches and filters benefit allocations by employee, benefit, status, year with pagination support.")
            .Produces<PagedList<BenefitAllocationResponse>>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Benefits))
            .MapToApiVersion(1);
    }
}

