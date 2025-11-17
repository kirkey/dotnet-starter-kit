using Accounting.Application.AccountingPeriods.Delete.v1;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.AccountingPeriods.v1;

public static class AccountingPeriodDeleteEndpoint
{
    internal static RouteHandlerBuilder MapAccountingPeriodDeleteEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapDelete("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                await mediator.Send(new DeleteAccountingPeriodCommand(id)).ConfigureAwait(false);
                return Results.NoContent();
            })
            .WithName(nameof(AccountingPeriodDeleteEndpoint))
            .WithSummary("delete accounting period by id")
            .WithDescription("delete accounting period by id")
            .Produces(StatusCodes.Status204NoContent)
            .RequirePermission(FshPermission.NameFor(FshActions.Delete, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}
