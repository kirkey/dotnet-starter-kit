using Accounting.Application.InventoryItems.Responses;
using Accounting.Application.InventoryItems.Search.v1;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.InventoryItems.v1;

public static class InventoryItemSearchEndpoint
{
    internal static RouteHandlerBuilder MapInventoryItemSearchEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (SearchInventoryItemsRequest request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(InventoryItemSearchEndpoint))
            .WithSummary("Search inventory items")
            .WithDescription("Searches inventory items with filters and pagination")
            .Produces<PagedList<InventoryItemResponse>>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}

