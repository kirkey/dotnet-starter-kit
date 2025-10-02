using Accounting.Application.BankReconciliations.Complete.v1;

namespace Accounting.Infrastructure.Endpoints.BankReconciliations.v1;

public static class BankReconciliationCompleteEndpoint
{
    internal static RouteHandlerBuilder MapBankReconciliationCompleteEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id}/complete", async (DefaultIdType id, CompleteBankReconciliationCommand command, ISender mediator) =>
            {
                if (id != command.Id)
                    return Results.BadRequest("ID mismatch");

                await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok();
            })
            .WithName(nameof(BankReconciliationCompleteEndpoint))
            .WithSummary("Complete a bank reconciliation")
            .WithDescription("Mark a bank reconciliation as completed")
            .Produces(StatusCodes.Status200OK)
            .RequirePermission("Permissions.Accounting.Update")
            .MapToApiVersion(1);
    }
}
