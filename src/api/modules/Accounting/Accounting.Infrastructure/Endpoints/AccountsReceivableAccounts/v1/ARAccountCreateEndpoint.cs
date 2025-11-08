using Accounting.Application.AccountsReceivableAccounts.Create.v1;

namespace Accounting.Infrastructure.Endpoints.AccountsReceivableAccounts.v1;

public static class ArAccountCreateEndpoint
{
    internal static RouteHandlerBuilder MapArAccountCreateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (AccountsReceivableAccountCreateCommand command, ISender mediator) =>
            {
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Created($"/accounting/accounts-receivable/{response.Id}", response);
            })
            .WithName(nameof(ArAccountCreateEndpoint))
            .WithSummary("Create AR account")
            .Produces<AccountsReceivableAccountCreateResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .RequirePermission("Permissions.Accounting.Create")
            .MapToApiVersion(1);
    }
}

