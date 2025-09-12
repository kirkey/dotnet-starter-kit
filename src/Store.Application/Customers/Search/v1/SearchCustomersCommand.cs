using FSH.Starter.WebApi.Store.Application.Customers.Get.v1;

namespace FSH.Starter.WebApi.Store.Application.Customers.Search.v1;

public class SearchCustomersCommand : PaginationFilter, IRequest<PagedList<CustomerResponse>>
{
    public string? Name { get; set; }
    public string? Code { get; set; }
    public string? CustomerType { get; set; }
    public string? Email { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
}
