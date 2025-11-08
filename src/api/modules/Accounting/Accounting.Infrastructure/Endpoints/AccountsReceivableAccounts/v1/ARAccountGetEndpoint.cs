using Accounting.Application.AccountsReceivableAccounts.Get;
using Accounting.Application.AccountsReceivableAccounts.Responses;

namespace Accounting.Infrastructure.Endpoints.AccountsReceivableAccounts.v1;

public static class ArAccountGetEndpoint
{
    internal static RouteHandlerBuilder MapArAccountGetEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetARAccountRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(ArAccountGetEndpoint))
            .WithSummary("Get AR account by ID")
            .Produces<ARAccountResponse>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission("Permissions.Accounting.View")
            .MapToApiVersion(1);
    }
}

