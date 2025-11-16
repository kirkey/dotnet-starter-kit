using FSH.Starter.WebApi.HumanResources.Application.BenefitAllocations.Get.v1;
using Shared.Authorization;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.BenefitAllocations.v1;

/// <summary>
/// Endpoint for retrieving a specific benefit allocation by ID.
/// </summary>
public static class GetBenefitAllocationEndpoint
{
    internal static RouteHandlerBuilder MapGetBenefitAllocationEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var request = new GetBenefitAllocationRequest(id);
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(GetBenefitAllocationEndpoint))
            .WithSummary("Gets a benefit allocation by ID")
            .WithDescription("Retrieves detailed information about a specific benefit allocation including amount, status, and approval details.")
            .Produces<BenefitAllocationResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Benefits))
            .MapToApiVersion(1);
    }
}

