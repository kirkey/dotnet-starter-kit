using Accounting.Application.Bills.Approve.v1;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.Bills.v1;

/// <summary>
/// Endpoint for approving bills.
/// </summary>
public static class ApproveBillEndpoint
{
    /// <summary>
    /// Maps the bill approval endpoint to the route builder.
    /// </summary>
    internal static RouteHandlerBuilder MapApproveBillEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id}/approve", async (DefaultIdType id, ISender mediator) =>
            {
                var command = new ApproveBillCommand(id);
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(ApproveBillEndpoint))
            .WithSummary("Approve a bill")
            .WithDescription("Approves a bill for payment processing. The approver is automatically determined from the current user session.")
            .Produces<ApproveBillResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.Approve, FshResources.Accounting))
            .MapToApiVersion(new ApiVersion(1, 0));
    }
}
