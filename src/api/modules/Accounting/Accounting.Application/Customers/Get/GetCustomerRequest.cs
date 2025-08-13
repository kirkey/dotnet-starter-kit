using MediatR;
using Accounting.Application.Customers.Dtos;

namespace Accounting.Application.Customers.Get;

public class GetCustomerRequest : IRequest<CustomerDto>
{
    public DefaultIdType Id { get; set; }

    public GetCustomerRequest(DefaultIdType id)
    {
        Id = id;
    }
}
