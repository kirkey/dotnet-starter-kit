using Accounting.Application.CreditMemos.Search;
using Microsoft.AspNetCore.Mvc;

namespace Accounting.Infrastructure.Endpoints.CreditMemos.v1;

public static class CreditMemoSearchEndpoint
{
    internal static RouteHandlerBuilder MapCreditMemoSearchEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async ([FromBody] SearchCreditMemosQuery query, ISender mediator) =>
            {
                var response = await mediator.Send(query).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(CreditMemoSearchEndpoint))
            .WithSummary("Search credit memos")
            .WithDescription("Search and filter credit memos with pagination")
            .Produces<FSH.Framework.Core.Paging.PaginationResponse<Accounting.Application.CreditMemos.Responses.CreditMemoResponse>>()
            .RequirePermission("Permissions.Accounting.View")
            .MapToApiVersion(1);
    }
}
