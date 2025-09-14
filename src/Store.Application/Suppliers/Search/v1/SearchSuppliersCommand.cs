using FSH.Starter.WebApi.Store.Application.Suppliers.Get.v1;

namespace FSH.Starter.WebApi.Store.Application.Suppliers.Search.v1;

public class SearchSuppliersCommand : PaginationFilter, IRequest<PagedList<SupplierResponse>>
{
    public string? Name { get; set; }
    public string? Code { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
}

