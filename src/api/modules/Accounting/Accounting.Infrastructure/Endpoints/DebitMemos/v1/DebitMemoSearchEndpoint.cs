using Accounting.Application.DebitMemos.Responses;
using Accounting.Application.DebitMemos.Search;

namespace Accounting.Infrastructure.Endpoints.DebitMemos.v1;

public static class DebitMemoSearchEndpoint
{
    internal static RouteHandlerBuilder MapDebitMemoSearchEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (ISender mediator, [FromBody] SearchDebitMemosQuery query) =>
            {
                var response = await mediator.Send(query).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(DebitMemoSearchEndpoint))
            .WithSummary("Search debit memos")
            .WithDescription("Search and filter debit memos with pagination")
            .Produces<PagedList<DebitMemoResponse>>()
            .RequirePermission("Permissions.Accounting.View")
            .MapToApiVersion(1);
    }
}
