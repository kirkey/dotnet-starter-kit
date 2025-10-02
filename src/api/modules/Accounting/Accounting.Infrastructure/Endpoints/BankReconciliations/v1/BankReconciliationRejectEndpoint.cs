using Accounting.Application.BankReconciliations.Reject.v1;

namespace Accounting.Infrastructure.Endpoints.BankReconciliations.v1;

public static class BankReconciliationRejectEndpoint
{
    internal static RouteHandlerBuilder MapBankReconciliationRejectEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id}/reject", async (DefaultIdType id, RejectBankReconciliationCommand command, ISender mediator) =>
            {
                if (id != command.Id)
                    return Results.BadRequest("ID mismatch");

                await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok();
            })
            .WithName(nameof(BankReconciliationRejectEndpoint))
            .WithSummary("Reject a bank reconciliation")
            .WithDescription("Reject a completed bank reconciliation for rework")
            .Produces(StatusCodes.Status200OK)
            .RequirePermission("Permissions.Accounting.Approve")
            .MapToApiVersion(1);
    }
}
