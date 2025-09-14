using Accounting.Application.Customers.Dtos;

namespace Accounting.Application.Customers.Queries;

public sealed class CustomerByIdSpec :
    Specification<Customer, CustomerDto>,
    ISingleResultSpecification<Customer, CustomerDto>
{
    public CustomerByIdSpec(DefaultIdType id) =>
        Query.Where(w => w.Id == id);
}
