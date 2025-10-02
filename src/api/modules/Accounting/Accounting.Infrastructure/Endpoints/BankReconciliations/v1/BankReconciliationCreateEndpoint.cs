using Accounting.Application.BankReconciliations.Create.v1;

namespace Accounting.Infrastructure.Endpoints.BankReconciliations.v1;

public static class BankReconciliationCreateEndpoint
{
    internal static RouteHandlerBuilder MapBankReconciliationCreateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (CreateBankReconciliationCommand command, ISender mediator) =>
            {
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(BankReconciliationCreateEndpoint))
            .WithSummary("Create a bank reconciliation")
            .WithDescription("Create a new bank reconciliation")
            .Produces<DefaultIdType>()
            .RequirePermission("Permissions.Accounting.Create")
            .MapToApiVersion(1);
    }
}
