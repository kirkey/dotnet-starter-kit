using Accounting.Application.AccountsPayableAccounts.Create.v1;

namespace Accounting.Infrastructure.Endpoints.AccountsPayableAccounts.v1;

public static class APAccountCreateEndpoint
{
    internal static RouteHandlerBuilder MapAPAccountCreateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (AccountsPayableAccountCreateCommand command, ISender mediator) =>
            {
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Created($"/accounting/accounts-payable/{response.Id}", response);
            })
            .WithName(nameof(APAccountCreateEndpoint))
            .WithSummary("Create AP account")
            .Produces<AccountsPayableAccountCreateResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .RequirePermission("Permissions.Accounting.Create")
            .MapToApiVersion(1);
    }
}

