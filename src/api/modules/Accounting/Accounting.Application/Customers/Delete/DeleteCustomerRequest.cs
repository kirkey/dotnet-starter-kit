namespace Accounting.Application.Customers.Delete;

public class DeleteCustomerRequest(DefaultIdType id) : IRequest
{
    public DefaultIdType Id { get; set; } = id;
}
