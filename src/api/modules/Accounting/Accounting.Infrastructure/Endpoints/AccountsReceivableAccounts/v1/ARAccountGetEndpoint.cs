using Accounting.Application.AccountsReceivableAccounts.Get;
using Accounting.Application.AccountsReceivableAccounts.Responses;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.AccountsReceivableAccounts.v1;

public static class ArAccountGetEndpoint
{
    internal static RouteHandlerBuilder MapArAccountGetEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetArAccountRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(ArAccountGetEndpoint))
            .WithSummary("Get AR account by ID")
            .Produces<ArAccountResponse>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}

