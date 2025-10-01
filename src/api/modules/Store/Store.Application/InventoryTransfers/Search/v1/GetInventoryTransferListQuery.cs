namespace FSH.Starter.WebApi.Store.Application.InventoryTransfers.Search.v1;

public record SearchInventoryTransfersCommand(
    int PageNumber = 1,
    int PageSize = 10,
    string? SearchTerm = null,
    DefaultIdType? FromWarehouseId = null,
    DefaultIdType? ToWarehouseId = null,
    string? Status = null,
    DateTime? FromDate = null,
    DateTime? ToDate = null) : IRequest<PagedList<GetInventoryTransferListResponse>>;
