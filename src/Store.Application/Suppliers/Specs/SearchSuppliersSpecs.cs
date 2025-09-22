using FSH.Starter.WebApi.Store.Application.Suppliers.Get.v1;
using FSH.Starter.WebApi.Store.Application.Suppliers.Search.v1;

namespace FSH.Starter.WebApi.Store.Application.Suppliers.Specs;

/// <summary>
/// Specification to filter and order suppliers for search queries.
/// </summary>
public class SearchSuppliersSpecs : EntitiesByPaginationFilterSpec<Supplier, SupplierResponse>
{
    public SearchSuppliersSpecs(SearchSuppliersCommand command)
        : base(command)
    {
        Query
            .OrderBy(s => s.Name, !command.HasOrderBy())
            .Where(s => s.Name!.Contains(command.Name!), !string.IsNullOrEmpty(command.Name))
            .Where(s => s.Code == command.Code, !string.IsNullOrEmpty(command.Code))
            .Where(s => s.City == command.City, !string.IsNullOrEmpty(command.City))
            .Where(s => s.Country == command.Country, !string.IsNullOrEmpty(command.Country));
    }
}
