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
                var response = await mediator.Send(new GetAccountingPeriodRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(AccountingPeriodGetEndpoint))
            .WithSummary("get an accounting period by id")
            .WithDescription("get an accounting period by id")
            .Produces<AccountingPeriodDto>()
            .RequirePermission("Permissions.Accounting.View")
            .MapToApiVersion(1);
    }
}
