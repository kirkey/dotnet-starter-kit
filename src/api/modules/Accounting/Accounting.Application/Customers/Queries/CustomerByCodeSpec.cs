using Accounting.Application.Customers.Dtos;

namespace Accounting.Application.Customers.Queries;

public sealed class CustomerByCodeSpec :
    Specification<Customer, CustomerDto>,
    ISingleResultSpecification<Customer, CustomerDto>
{
    public CustomerByCodeSpec(string code) =>
        Query.Where(w => w.CustomerCode == code);
}
