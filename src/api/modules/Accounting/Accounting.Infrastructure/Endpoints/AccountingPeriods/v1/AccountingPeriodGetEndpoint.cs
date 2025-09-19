using Accounting.Application.AccountingPeriods.Dtos;
using Accounting.Application.AccountingPeriods.Get.v1;

namespace Accounting.Infrastructure.Endpoints.AccountingPeriods.v1;

public static class AccountingPeriodGetEndpoint
{
    internal static RouteHandlerBuilder MapAccountingPeriodGetEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetAccountingPeriodQuery(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(AccountingPeriodGetEndpoint))
            .WithSummary("get accounting period by id")
            .WithDescription("get accounting period by id")
            .Produces<AccountingPeriodResponse>()
            .RequirePermission("Permissions.Accounting.View")
            .MapToApiVersion(1);
    }
}
