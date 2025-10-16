using Accounting.Application.CreditMemos.Responses;
using Accounting.Application.CreditMemos.Search;

namespace Accounting.Infrastructure.Endpoints.CreditMemos.v1;

/// <summary>
/// Endpoint for searching credit memos with pagination and filtering.
/// </summary>
public static class CreditMemoSearchEndpoint
{
    internal static RouteHandlerBuilder MapCreditMemoSearchEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (ISender mediator, [FromBody] SearchCreditMemosQuery query) =>
            {
                var response = await mediator.Send(query).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(CreditMemoSearchEndpoint))
            .WithSummary("Search credit memos")
            .WithDescription("Search and filter credit memos with pagination")
            .Produces<PagedList<CreditMemoResponse>>()
            .RequirePermission("Permissions.Accounting.View")
            .MapToApiVersion(1);
    }
}
