using Accounting.Application.Customers.Dtos;

namespace Accounting.Application.Customers.Queries;

public sealed class CustomerByIdSpec :
    Specification<Customer, CustomerResponse>,
    ISingleResultSpecification<Customer, CustomerResponse>
{
    public CustomerByIdSpec(DefaultIdType id) =>
        Query.Where(w => w.Id == id);
}
