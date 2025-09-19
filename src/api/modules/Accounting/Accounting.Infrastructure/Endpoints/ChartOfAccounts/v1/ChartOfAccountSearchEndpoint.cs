using Accounting.Application.ChartOfAccounts.Responses;
using Accounting.Application.ChartOfAccounts.Search.v1;

namespace Accounting.Infrastructure.Endpoints.ChartOfAccounts.v1;

public static class ChartOfAccountSearchEndpoint
{
    internal static RouteHandlerBuilder MapChartOfAccountSearchEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (ISender mediator, [FromBody] SearchChartOfAccountQuery query) =>
            {
                var response = await mediator.Send(query).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(ChartOfAccountSearchEndpoint))
            .WithSummary("Gets a list of chart of accounts")
            .WithDescription("Gets a list of chart of accounts with pagination and filtering support")
            .Produces<PagedList<ChartOfAccountResponse>>()
            .RequirePermission("Permissions.Accounting.View")
            .MapToApiVersion(1);
    }
}
