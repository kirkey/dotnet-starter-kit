using Accounting.Application.Customers.Dtos;

namespace Accounting.Application.Customers.Get;

public class GetCustomerRequest(DefaultIdType id) : IRequest<CustomerDto>
{
    public DefaultIdType Id { get; set; } = id;
}
