using Accounting.Application.AccountReconciliations.Approve.v1;

using Accounting.Application.AccountReconciliations.Approve.v1;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.AccountReconciliations.v1;

/// <summary>
/// Endpoint for approving an account reconciliation.
/// </summary>
public static class ApproveAccountReconciliationEndpoint
{
    internal static RouteHandlerBuilder MapApproveAccountReconciliationEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id}/approve", async (DefaultIdType id, ISender mediator, ApproveAccountReconciliationCommand command) =>
            {
                await mediator.Send(command with { Id = id }).ConfigureAwait(false);
                return Results.NoContent();
            })
            .WithName(nameof(ApproveAccountReconciliationEndpoint))
            .WithSummary("Approve Account Reconciliation")
            .WithDescription("Approve a reconciled account reconciliation")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .RequirePermission(FshPermission.NameFor(FshActions.Approve, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}

