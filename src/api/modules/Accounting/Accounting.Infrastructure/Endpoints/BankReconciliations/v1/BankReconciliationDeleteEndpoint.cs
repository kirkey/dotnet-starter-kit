using Accounting.Application.BankReconciliations.Delete.v1;

namespace Accounting.Infrastructure.Endpoints.BankReconciliations.v1;

public static class BankReconciliationDeleteEndpoint
{
    internal static RouteHandlerBuilder MapBankReconciliationDeleteEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapDelete("/{id}", async (DefaultIdType id, ISender mediator) =>
            {
                await mediator.Send(new DeleteBankReconciliationCommand(id)).ConfigureAwait(false);
                return Results.NoContent();
            })
            .WithName(nameof(BankReconciliationDeleteEndpoint))
            .WithSummary("Delete a bank reconciliation")
            .WithDescription("Delete a bank reconciliation by ID")
            .Produces(StatusCodes.Status204NoContent)
            .RequirePermission("Permissions.Accounting.Delete")
            .MapToApiVersion(1);
    }
}
