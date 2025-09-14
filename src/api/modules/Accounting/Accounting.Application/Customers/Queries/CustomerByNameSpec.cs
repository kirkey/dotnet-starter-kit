using Accounting.Application.Customers.Dtos;

namespace Accounting.Application.Customers.Queries;

public sealed class CustomerByNameSpec :
    Specification<Customer, CustomerDto>,
    ISingleResultSpecification<Customer, CustomerDto>
{
    public CustomerByNameSpec(string name) =>
        Query.Where(w => w.Name == name);
}
