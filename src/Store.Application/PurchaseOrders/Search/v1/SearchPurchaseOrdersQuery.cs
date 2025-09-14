namespace FSH.Starter.WebApi.Store.Application.PurchaseOrders.Search.v1;

public record SearchPurchaseOrdersCommand(
    int PageNumber = 1,
    int PageSize = 10,
    string? SearchTerm = null,
    DefaultIdType? SupplierId = null,
    string? Status = null,
    DateTime? FromDate = null,
    DateTime? ToDate = null
) : IRequest<PagedList<GetPurchaseOrderListResponse>>;

