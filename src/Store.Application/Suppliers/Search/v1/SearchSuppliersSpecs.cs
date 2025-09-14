using FSH.Starter.WebApi.Store.Application.Suppliers.Get.v1;

namespace FSH.Starter.WebApi.Store.Application.Suppliers.Search.v1;

public class SearchSuppliersSpecs : EntitiesByPaginationFilterSpec<Supplier, SupplierResponse>
{
    public SearchSuppliersSpecs(SearchSuppliersCommand command)
        : base(command)
    {
        Query
            .Where(x => x.Name.Contains(command.Name!), !string.IsNullOrWhiteSpace(command.Name))
            .Where(x => x.Code == command.Code, !string.IsNullOrWhiteSpace(command.Code))
            .Where(x => x.City == command.City, !string.IsNullOrWhiteSpace(command.City))
            .Where(x => x.Country == command.Country, !string.IsNullOrWhiteSpace(command.Country))
            .OrderByDescending(x => x.Name, !command.HasOrderBy());
    }
}

