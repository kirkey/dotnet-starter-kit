using Ardalis.Specification;
using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.Warehouse.Domain.ValueObjects;

namespace FSH.Starter.WebApi.Warehouse.Features.StockMovement.v1;

public sealed record GetStockMovementListRequest(
    DefaultIdType? WarehouseId = null,
    string? ProductSku = null,
    StockMovementType? MovementType = null,
    StockMovementStatus? Status = null,
    DateTime? FromDate = null,
    DateTime? ToDate = null,
    int PageNumber = 1,
    int PageSize = 10);

public sealed record StockMovementDto(
    DefaultIdType Id,
    DefaultIdType WarehouseId,
    string WarehouseName,
    string ProductSku,
    string ProductName,
    StockMovementType MovementType,
    decimal Quantity,
    string UnitOfMeasure,
    string? ReferenceNumber,
    string? Notes,
    DateTime MovementDate,
    StockMovementStatus Status,
    DefaultIdType? SourceWarehouseId,
    string? SourceWarehouseName,
    DefaultIdType? DestinationWarehouseId,
    string? DestinationWarehouseName);

public sealed class GetStockMovementListHandler(
    IReadRepository<Domain.StockMovement> stockMovementRepository,
    IReadRepository<Domain.Warehouse> warehouseRepository)
{
    public async Task<PagedList<StockMovementDto>> Handle(GetStockMovementListRequest request, CancellationToken cancellationToken)
    {
        var spec = new StockMovementListSpec(request);
        var stockMovements = await stockMovementRepository.ListAsync(spec, cancellationToken);
        var totalCount = await stockMovementRepository.CountAsync(spec, cancellationToken);

        // Get warehouse names
        var warehouseIds = stockMovements
            .SelectMany(sm => new[] { sm.WarehouseId, sm.SourceWarehouseId, sm.DestinationWarehouseId })
            .Where(id => id.HasValue)
            .Select(id => id!.Value)
            .Distinct()
            .ToList();

        var warehouses = await warehouseRepository.ListAsync(
            w => warehouseIds.Contains(w.Id), cancellationToken);
        var warehouseDict = warehouses.ToDictionary(w => w.Id, w => w.Name);

        var stockMovementDtos = stockMovements.Select(sm => new StockMovementDto(
            sm.Id,
            sm.WarehouseId,
            warehouseDict.GetValueOrDefault(sm.WarehouseId, "Unknown"),
            sm.ProductSku,
            sm.ProductName,
            sm.MovementType,
            sm.Quantity,
            sm.UnitOfMeasure.Symbol,
            sm.ReferenceNumber,
            sm.Notes,
            sm.MovementDate,
            sm.Status,
            sm.SourceWarehouseId,
            sm.SourceWarehouseId.HasValue ? warehouseDict.GetValueOrDefault(sm.SourceWarehouseId.Value, "Unknown") : null,
            sm.DestinationWarehouseId,
            sm.DestinationWarehouseId.HasValue ? warehouseDict.GetValueOrDefault(sm.DestinationWarehouseId.Value, "Unknown") : null)).ToList();

        return new PagedList<StockMovementDto>(stockMovementDtos, totalCount, request.PageNumber, request.PageSize);
    }
}

public sealed class StockMovementListSpec : Specification<Domain.StockMovement>
{
    public StockMovementListSpec(GetStockMovementListRequest request)
    {
        if (request.WarehouseId.HasValue)
        {
            Query.Where(sm => sm.WarehouseId == request.WarehouseId.Value ||
                            sm.SourceWarehouseId == request.WarehouseId.Value ||
                            sm.DestinationWarehouseId == request.WarehouseId.Value);
        }

        if (!string.IsNullOrEmpty(request.ProductSku))
        {
            Query.Where(sm => sm.ProductSku.Contains(request.ProductSku) ||
                            sm.ProductName.Contains(request.ProductSku));
        }

        if (request.MovementType.HasValue)
        {
            Query.Where(sm => sm.MovementType == request.MovementType.Value);
        }

        if (request.Status.HasValue)
        {
            Query.Where(sm => sm.Status == request.Status.Value);
        }

        if (request.FromDate.HasValue)
        {
            Query.Where(sm => sm.MovementDate >= request.FromDate.Value);
        }

        if (request.ToDate.HasValue)
        {
            Query.Where(sm => sm.MovementDate <= request.ToDate.Value);
        }

        Query.OrderByDescending(sm => sm.MovementDate);
        Query.Skip((request.PageNumber - 1) * request.PageSize);
        Query.Take(request.PageSize);
    }
}
