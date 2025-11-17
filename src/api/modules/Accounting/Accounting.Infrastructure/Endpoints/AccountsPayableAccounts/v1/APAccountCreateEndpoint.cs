using Accounting.Application.AccountsPayableAccounts.Create.v1;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.AccountsPayableAccounts.v1;

public static class ApAccountCreateEndpoint
{
    internal static RouteHandlerBuilder MapApAccountCreateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (AccountsPayableAccountCreateCommand command, ISender mediator) =>
            {
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Created($"/accounting/accounts-payable/{response.Id}", response);
            })
            .WithName(nameof(ApAccountCreateEndpoint))
            .WithSummary("Create AP account")
            .Produces<AccountsPayableAccountCreateResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}

