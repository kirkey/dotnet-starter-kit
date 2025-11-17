using Accounting.Application.AccountingPeriods.Update.v1;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.AccountingPeriods.v1;

public static class AccountingPeriodUpdateEndpoint
{
    internal static RouteHandlerBuilder MapAccountingPeriodUpdateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id:guid}", async (DefaultIdType id, UpdateAccountingPeriodCommand request, ISender mediator) =>
            {
                var command = request with { Id = id };
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(AccountingPeriodUpdateEndpoint))
            .WithSummary("update accounting period")
            .WithDescription("update accounting period")
            .Produces<DefaultIdType>()
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}
