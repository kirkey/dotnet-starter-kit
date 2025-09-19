using Accounting.Application.Customers.Dtos;

namespace Accounting.Application.Customers.Get;

public class GetCustomerQuery(DefaultIdType id) : IRequest<CustomerResponse>
{
    public DefaultIdType Id { get; set; } = id;
}
