using Accounting.Application.AccountsReceivableAccounts.Responses;
using Accounting.Application.AccountsReceivableAccounts.Search.v1;

namespace Accounting.Infrastructure.Endpoints.AccountsReceivableAccounts.v1;

public static class ArAccountSearchEndpoint
{
    internal static RouteHandlerBuilder MapArAccountSearchEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (SearchARAccountsRequest request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(ArAccountSearchEndpoint))
            .WithSummary("Search AR accounts")
            .Produces<List<ARAccountResponse>>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .RequirePermission("Permissions.Accounting.View")
            .MapToApiVersion(1);
    }
}


