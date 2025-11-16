using FSH.Framework.Core.Identity.Users.Abstractions;
using FSH.Starter.WebApi.HumanResources.Application.BenefitAllocations.Reject.v1;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.BenefitAllocations.v1;

/// <summary>
/// Endpoint for rejecting a benefit allocation.
/// </summary>
public static class RejectBenefitAllocationEndpoint
{
    internal static RouteHandlerBuilder MapRejectBenefitAllocationEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id}/reject", async (DefaultIdType id, ICurrentUser currentUser, ISender mediator) =>
            {
                var request = new RejectBenefitAllocationCommand(id, currentUser.GetUserId());
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(RejectBenefitAllocationEndpoint))
            .WithSummary("Rejects a benefit allocation")
            .WithDescription("Rejects a pending benefit allocation. Marks as rejected with optional reason.")
            .Produces<RejectBenefitAllocationResponse>()
            .RequirePermission("Permissions.BenefitAllocations.Reject")
            .MapToApiVersion(1);
    }
}

