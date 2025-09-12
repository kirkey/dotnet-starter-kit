

namespace FSH.Starter.WebApi.Store.Application.GroceryItems.Search.v1;

public class SearchGroceryItemsSpecs : EntitiesByPaginationFilterSpec<GroceryItem, GroceryItemResponse>
{
    public SearchGroceryItemsSpecs(SearchGroceryItemsCommand command)
        : base(command) =>
        Query
            .OrderBy(g => g.Name, !command.HasOrderBy())
            .Where(g => g.Name!.Contains(command.Name!), !string.IsNullOrEmpty(command.Name))
            .Where(g => g.SKU == command.SKU, !string.IsNullOrEmpty(command.SKU))
            .Where(g => g.Barcode == command.Barcode, !string.IsNullOrEmpty(command.Barcode))
            .Where(g => g.CategoryId == command.CategoryId!.Value, command.CategoryId.HasValue)
            .Where(g => g.SupplierId == command.SupplierId!.Value, command.SupplierId.HasValue)
            .Where(g => g.CurrentStock <= g.ReorderPoint, command.IsLowStock == true)
            .Where(g => g.IsPerishable == command.IsPerishable, command.IsPerishable.HasValue)
            .Where(g => g.Price >= command.MinPrice!.Value, command.MinPrice.HasValue)
            .Where(g => g.Price <= command.MaxPrice!.Value, command.MaxPrice.HasValue);
}
