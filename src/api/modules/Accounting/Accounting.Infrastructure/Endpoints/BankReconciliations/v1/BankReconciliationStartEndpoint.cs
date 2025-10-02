using Accounting.Application.BankReconciliations.Start.v1;

namespace Accounting.Infrastructure.Endpoints.BankReconciliations.v1;

public static class BankReconciliationStartEndpoint
{
    internal static RouteHandlerBuilder MapBankReconciliationStartEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id}/start", async (DefaultIdType id, ISender mediator) =>
            {
                await mediator.Send(new StartBankReconciliationCommand(id)).ConfigureAwait(false);
                return Results.Ok();
            })
            .WithName(nameof(BankReconciliationStartEndpoint))
            .WithSummary("Start a bank reconciliation")
            .WithDescription("Mark a bank reconciliation as in progress")
            .Produces(StatusCodes.Status200OK)
            .RequirePermission("Permissions.Accounting.Update")
            .MapToApiVersion(1);
    }
}
