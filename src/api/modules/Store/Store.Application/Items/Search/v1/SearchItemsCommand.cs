namespace FSH.Starter.WebApi.Store.Application.Items.Search.v1;

/// <summary>
/// Command for searching items with advanced filtering.
/// </summary>
public class SearchItemsCommand : PaginationFilter, IRequest<PagedList<ItemResponse>>
{
    public string? SearchTerm { get; set; }
    public string? Sku { get; set; }
    public string? Barcode { get; set; }
    public DefaultIdType? CategoryId { get; set; }
    public DefaultIdType? SupplierId { get; set; }
    public bool? IsPerishable { get; set; }
    public bool? IsSerialTracked { get; set; }
    public bool? IsLotTracked { get; set; }
    public bool? IsActive { get; set; }
    public string? Brand { get; set; }
    public string? Manufacturer { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
}
