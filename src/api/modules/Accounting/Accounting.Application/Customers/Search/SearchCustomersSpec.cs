using Accounting.Application.Customers.Responses;

namespace Accounting.Application.Customers.Search;

public sealed class SearchCustomersSpec : EntitiesByPaginationFilterSpec<Customer, CustomerResponse>
{
    public SearchCustomersSpec(SearchCustomersQuery request) : base(request)
    {
        Query
            .OrderBy(c => c.CustomerCode, !request.HasOrderBy())
            .Where(c => c.CustomerCode!.Contains(request.CustomerCode!), !string.IsNullOrEmpty(request.CustomerCode))
            .Where(c => c.Name!.Contains(request.Name!), !string.IsNullOrEmpty(request.Name))
            .Where(c => c.Email!.Contains(request.Email!), !string.IsNullOrEmpty(request.Email));
    }
}
