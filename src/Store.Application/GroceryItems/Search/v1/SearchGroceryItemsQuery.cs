namespace FSH.Starter.WebApi.Store.Application.GroceryItems.Search.v1;

public class SearchGroceryItemsCommand : PaginationFilter, IRequest<PagedList<GroceryItemResponse>>
{
    public string? Name { get; set; }
    public string? Sku { get; set; }
    public string? Barcode { get; set; }
    public DefaultIdType? CategoryId { get; set; }
    public DefaultIdType? SupplierId { get; set; }
    public bool? IsLowStock { get; set; }
    public bool? IsExpiringSoon { get; set; }
    public bool? IsPerishable { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
}
