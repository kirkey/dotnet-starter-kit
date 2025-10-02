using Accounting.Application.BankReconciliations.Get.v1;
using Accounting.Application.BankReconciliations.Responses;

namespace Accounting.Infrastructure.Endpoints.BankReconciliations.v1;

public static class BankReconciliationGetEndpoint
{
    internal static RouteHandlerBuilder MapBankReconciliationGetEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetBankReconciliationRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(BankReconciliationGetEndpoint))
            .WithSummary("Get a bank reconciliation")
            .WithDescription("Get a bank reconciliation by ID")
            .Produces<BankReconciliationResponse>()
            .RequirePermission("Permissions.Accounting.View")
            .MapToApiVersion(1);
    }
}
