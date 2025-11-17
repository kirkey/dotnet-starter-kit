using Accounting.Application.ChartOfAccounts.Responses;
using Accounting.Application.ChartOfAccounts.Search.v1;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.ChartOfAccounts.v1;

public static class ChartOfAccountSearchEndpoint
{
    internal static RouteHandlerBuilder MapChartOfAccountSearchEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (ISender mediator, [FromBody] SearchChartOfAccountRequest request) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(ChartOfAccountSearchEndpoint))
            .WithSummary("Search chart of accounts")
            .WithDescription("Searches chart of accounts with pagination and filtering support")
            .Produces<PagedList<ChartOfAccountResponse>>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}
