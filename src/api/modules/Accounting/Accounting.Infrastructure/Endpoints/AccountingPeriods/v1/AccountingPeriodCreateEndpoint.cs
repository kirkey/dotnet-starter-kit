using Accounting.Application.AccountingPeriods.Create.v1;

namespace Accounting.Infrastructure.Endpoints.AccountingPeriods.v1;

public static class AccountingPeriodCreateEndpoint
{
    internal static RouteHandlerBuilder MapAccountingPeriodCreateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (CreateAccountingPeriodRequest request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(AccountingPeriodCreateEndpoint))
            .WithSummary("create an accounting period")
            .WithDescription("create an accounting period")
            .Produces<DefaultIdType>()
            .RequirePermission("Permissions.Accounting.Create")
            .MapToApiVersion(1);
    }
}

