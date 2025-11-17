using Accounting.Application.AccountingPeriods.Create.v1;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.AccountingPeriods.v1;

public static class AccountingPeriodCreateEndpoint
{
    internal static RouteHandlerBuilder MapAccountingPeriodCreateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (CreateAccountingPeriodCommand request, ISender mediator) =>
            {
                var id = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(id);
            })
            .WithName(nameof(AccountingPeriodCreateEndpoint))
            .WithSummary("create accounting period")
            .WithDescription("create accounting period")
            .Produces<DefaultIdType>()
            .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}
