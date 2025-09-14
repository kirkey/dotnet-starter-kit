using Accounting.Application.Customers.Dtos;

namespace Accounting.Application.Customers.Search;

public sealed class SearchCustomersSpec : EntitiesByPaginationFilterSpec<Customer, CustomerDto>
{
    public SearchCustomersSpec(SearchCustomersRequest request) : base(request)
    {
        Query
            .OrderBy(c => c.CustomerCode, !request.HasOrderBy())
            .Where(c => c.CustomerCode!.Contains(request.CustomerCode!), !string.IsNullOrEmpty(request.CustomerCode))
            .Where(c => c.Name!.Contains(request.Name!), !string.IsNullOrEmpty(request.Name))
            .Where(c => c.Email!.Contains(request.Email!), !string.IsNullOrEmpty(request.Email));
    }
}


