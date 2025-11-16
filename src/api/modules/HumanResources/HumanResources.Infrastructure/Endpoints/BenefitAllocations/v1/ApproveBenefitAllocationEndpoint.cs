using FSH.Framework.Core.Identity.Users.Abstractions;
using Shared.Authorization;
using FSH.Starter.WebApi.HumanResources.Application.BenefitAllocations.Approve.v1;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.BenefitAllocations.v1;

/// <summary>
/// Endpoint for approving a benefit allocation.
/// </summary>
public static class ApproveBenefitAllocationEndpoint
{
    internal static RouteHandlerBuilder MapApproveBenefitAllocationEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id:guid}/approve", async (DefaultIdType id, ICurrentUser currentUser, ISender mediator) =>
            {
                var request = new ApproveBenefitAllocationCommand(id, currentUser.GetUserId());
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(ApproveBenefitAllocationEndpoint))
            .WithSummary("Approves a benefit allocation")
            .WithDescription("Approves a pending benefit allocation. Activates the allocation for the employee.")
            .Produces<ApproveBenefitAllocationResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Approve, FshResources.Benefits))
            .MapToApiVersion(1);
    }
}

