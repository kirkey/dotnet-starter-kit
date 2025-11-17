using Accounting.Application.AccountReconciliations.Update.v1;

using Accounting.Application.AccountReconciliations.Update.v1;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.AccountReconciliations.v1;

/// <summary>
/// Endpoint for updating an account reconciliation.
/// </summary>
public static class UpdateAccountReconciliationEndpoint
{
    internal static RouteHandlerBuilder MapUpdateAccountReconciliationEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id}", async (DefaultIdType id, ISender mediator, UpdateAccountReconciliationCommand command) =>
            {
                await mediator.Send(command with { Id = id }).ConfigureAwait(false);
                return Results.NoContent();
            })
            .WithName(nameof(UpdateAccountReconciliationEndpoint))
            .WithSummary("Update Account Reconciliation")
            .WithDescription("Update reconciliation balances and metadata")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}

