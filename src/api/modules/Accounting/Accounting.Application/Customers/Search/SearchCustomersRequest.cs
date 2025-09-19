using Accounting.Application.Customers.Dtos;

namespace Accounting.Application.Customers.Search;

public class SearchCustomersQuery : PaginationFilter, IRequest<PagedList<CustomerResponse>>
{
    public string? CustomerCode { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
}
