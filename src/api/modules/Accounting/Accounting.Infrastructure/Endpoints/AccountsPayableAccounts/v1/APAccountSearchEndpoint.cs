using Accounting.Application.AccountsPayableAccounts.Responses;
using Accounting.Application.AccountsPayableAccounts.Search.v1;

namespace Accounting.Infrastructure.Endpoints.AccountsPayableAccounts.v1;

public static class APAccountSearchEndpoint
{
    internal static RouteHandlerBuilder MapAPAccountSearchEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (SearchAPAccountsRequest request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(APAccountSearchEndpoint))
            .WithSummary("Search AP accounts")
            .Produces<List<APAccountResponse>>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .RequirePermission("Permissions.Accounting.View")
            .MapToApiVersion(1);
    }
}


