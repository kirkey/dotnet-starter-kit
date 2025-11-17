using FSH.Starter.WebApi.Store.Application.InventoryTransactions.Search.v1;
using Shared.Authorization;

namespace Store.Infrastructure.Endpoints.InventoryTransactions.v1;

public static class SearchInventoryTransactionsEndpoint
{
    internal static RouteHandlerBuilder MapSearchInventoryTransactionsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (SearchInventoryTransactionsCommand command, ISender sender) =>
            {
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(SearchInventoryTransactionsEndpoint))
            .WithSummary("Search inventory transactions")
            .WithDescription("Searches for inventory transactions with pagination and filtering by transaction number, item, warehouse, type, date range, approval status, and cost range.")
            .Produces<PagedList<InventoryTransactionDto>>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Store))
            .MapToApiVersion(1);
    }
}
