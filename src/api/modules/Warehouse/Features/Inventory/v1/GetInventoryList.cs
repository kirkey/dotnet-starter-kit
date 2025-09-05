using Ardalis.Specification;
using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Persistence;

namespace FSH.Starter.WebApi.Warehouse.Features.Inventory.v1;

public sealed record GetInventoryListRequest(
    DefaultIdType? WarehouseId = null,
    string? ProductSku = null,
    bool? LowStockOnly = null,
    int PageNumber = 1,
    int PageSize = 10);

public sealed record InventoryItemDto(
    DefaultIdType Id,
    DefaultIdType WarehouseId,
    string WarehouseName,
    string ProductSku,
    string ProductName,
    decimal CurrentStock,
    decimal ReservedStock,
    decimal AvailableStock,
    decimal MinimumStock,
    decimal MaximumStock,
    string UnitOfMeasure,
    DateTime LastMovementDate,
    bool IsLowStock,
    bool IsOverstock);

public sealed class GetInventoryListHandler(
    IReadRepository<Domain.InventoryItem> inventoryRepository,
    IReadRepository<Domain.Warehouse> warehouseRepository)
{
    public async Task<PagedList<InventoryItemDto>> Handle(GetInventoryListRequest request, CancellationToken cancellationToken)
    {
        var spec = new InventoryListSpec(request);
        var inventoryItems = await inventoryRepository.ListAsync(spec, cancellationToken);
        var totalCount = await inventoryRepository.CountAsync(spec, cancellationToken);

        // Get warehouse names for the items
        var warehouseIds = inventoryItems.Select(i => i.WarehouseId).Distinct().ToList();
        var warehouses = await warehouseRepository.ListAsync(
            w => warehouseIds.Contains(w.Id), cancellationToken);
        var warehouseDict = warehouses.ToDictionary(w => w.Id, w => w.Name);

        var inventoryDtos = inventoryItems.Select(item => new InventoryItemDto(
            item.Id,
            item.WarehouseId,
            warehouseDict.GetValueOrDefault(item.WarehouseId, "Unknown"),
            item.ProductSku,
            item.ProductName,
            item.CurrentStock,
            item.ReservedStock,
            item.AvailableStock,
            item.MinimumStock,
            item.MaximumStock,
            item.UnitOfMeasure.Symbol,
            item.LastMovementDate,
            item.CurrentStock <= item.MinimumStock,
            item.CurrentStock >= item.MaximumStock)).ToList();

        return new PagedList<InventoryItemDto>(inventoryDtos, totalCount, request.PageNumber, request.PageSize);
    }
}

public sealed class InventoryListSpec : Specification<Domain.InventoryItem>
{
    public InventoryListSpec(GetInventoryListRequest request)
    {
        if (request.WarehouseId.HasValue)
        {
            Query.Where(i => i.WarehouseId == request.WarehouseId.Value);
        }

        if (!string.IsNullOrEmpty(request.ProductSku))
        {
            Query.Where(i => i.ProductSku.Contains(request.ProductSku) || 
                           i.ProductName.Contains(request.ProductSku));
        }

        if (request.LowStockOnly == true)
        {
            Query.Where(i => i.CurrentStock <= i.MinimumStock);
        }

        Query.OrderBy(i => i.ProductSku);
        Query.Skip((request.PageNumber - 1) * request.PageSize);
        Query.Take(request.PageSize);
    }
}
