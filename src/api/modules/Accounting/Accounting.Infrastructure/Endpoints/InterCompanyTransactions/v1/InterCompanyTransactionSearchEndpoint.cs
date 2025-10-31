using Accounting.Application.InterCompanyTransactions.Responses;
using Accounting.Application.InterCompanyTransactions.Search.v1;

namespace Accounting.Infrastructure.Endpoints.InterCompanyTransactions.v1;

public static class InterCompanyTransactionSearchEndpoint
{
    internal static RouteHandlerBuilder MapInterCompanyTransactionSearchEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (SearchInterCompanyTransactionsRequest request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(InterCompanyTransactionSearchEndpoint))
            .WithSummary("Search inter-company transactions")
            .Produces<List<InterCompanyTransactionResponse>>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .RequirePermission("Permissions.Accounting.View")
            .MapToApiVersion(1);
    }
}


