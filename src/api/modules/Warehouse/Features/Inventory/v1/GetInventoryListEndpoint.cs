using FSH.Framework.Core.Paging;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Starter.WebApi.Warehouse.Features.Inventory.v1;

public static class GetInventoryListEndpoint
{
    public static void MapGetInventoryListEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGet("/inventory", async (
            DefaultIdType? warehouseId,
            string? productSku,
            bool? lowStockOnly,
            int pageNumber = 1,
            int pageSize = 10,
            ISender sender = default!) =>
        {
            var response = await sender.Send(new GetInventoryListQuery(warehouseId, productSku, lowStockOnly, pageNumber, pageSize));
            return Results.Ok(response);
        })
        .WithName("GetInventoryList")
        .WithSummary("Get inventory list")
        .WithDescription("Retrieves a paginated list of inventory items across warehouses with optional filtering")
        .Produces<PagedList<InventoryItemDto>>();
    }
}

public sealed record GetInventoryListQuery(
    DefaultIdType? WarehouseId,
    string? ProductSku,
    bool? LowStockOnly,
    int PageNumber,
    int PageSize) : IRequest<PagedList<InventoryItemDto>>;

public sealed class GetInventoryListQueryHandler(GetInventoryListHandler handler)
    : IRequestHandler<GetInventoryListQuery, PagedList<InventoryItemDto>>
{
    public async Task<PagedList<InventoryItemDto>> Handle(GetInventoryListQuery request, CancellationToken cancellationToken)
    {
        return await handler.Handle(new GetInventoryListRequest(
            request.WarehouseId,
            request.ProductSku,
            request.LowStockOnly,
            request.PageNumber,
            request.PageSize), cancellationToken);
    }
}
