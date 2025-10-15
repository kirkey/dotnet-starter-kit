using Accounting.Application.DebitMemos.Search;
using Microsoft.AspNetCore.Mvc;

namespace Accounting.Infrastructure.Endpoints.DebitMemos.v1;

public static class DebitMemoSearchEndpoint
{
    internal static RouteHandlerBuilder MapDebitMemoSearchEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async ([FromBody] SearchDebitMemosQuery query, ISender mediator) =>
            {
                var response = await mediator.Send(query).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(DebitMemoSearchEndpoint))
            .WithSummary("Search debit memos")
            .WithDescription("Search and filter debit memos with pagination")
            .Produces<FSH.Framework.Core.Paging.PaginationResponse<Accounting.Application.DebitMemos.Responses.DebitMemoResponse>>()
            .RequirePermission("Permissions.Accounting.View")
            .MapToApiVersion(1);
    }
}
