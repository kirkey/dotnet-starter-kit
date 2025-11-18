using Accounting.Application.AccountsPayableAccounts.Responses;
using Accounting.Application.AccountsPayableAccounts.Search.v1;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.AccountsPayableAccounts.v1;

public static class ApAccountSearchEndpoint
{
    internal static RouteHandlerBuilder MapApAccountSearchEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (SearchApAccountsRequest request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(ApAccountSearchEndpoint))
            .WithSummary("Search AP accounts")
            .Produces<PagedList<ApAccountResponse>>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}


