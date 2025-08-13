using MediatR;

namespace Accounting.Application.Customers.Delete;

public class DeleteCustomerRequest : IRequest
{
    public DefaultIdType Id { get; set; }

    public DeleteCustomerRequest(DefaultIdType id)
    {
        Id = id;
    }
}
