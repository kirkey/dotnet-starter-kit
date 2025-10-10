using Accounting.Application.BankReconciliations.Responses;
using Accounting.Application.BankReconciliations.Search.v1;

namespace Accounting.Infrastructure.Endpoints.BankReconciliations.v1;

public static class BankReconciliationSearchEndpoint
{
    internal static RouteHandlerBuilder MapBankReconciliationSearchEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (SearchBankReconciliationsCommand command, ISender mediator) =>
            {
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(BankReconciliationSearchEndpoint))
            .WithSummary("Search bank reconciliations")
            .WithDescription("Search and filter bank reconciliations with pagination")
            .Produces<PagedList<BankReconciliationResponse>>()
            .RequirePermission("Permissions.Accounting.View")
            .MapToApiVersion(1);
    }
}
