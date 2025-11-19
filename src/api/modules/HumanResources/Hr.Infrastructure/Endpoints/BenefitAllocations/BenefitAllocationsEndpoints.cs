using FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.BenefitAllocations.v1;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.BenefitAllocations;

/// <summary>
/// Endpoint routes for managing benefit allocations.
/// </summary>
public static class BenefitAllocationsEndpoints
{
    internal static void MapBenefitAllocationsEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/benefit-allocations")
            .WithTags("Benefit Allocations")
            .WithDescription("Endpoints for managing benefit allocations with approval workflows");

        group.MapCreateBenefitAllocationEndpoint();
        group.MapGetBenefitAllocationEndpoint();
        group.MapSearchBenefitAllocationsEndpoint();
        group.MapApproveBenefitAllocationEndpoint();
        group.MapRejectBenefitAllocationEndpoint();
    }
}

