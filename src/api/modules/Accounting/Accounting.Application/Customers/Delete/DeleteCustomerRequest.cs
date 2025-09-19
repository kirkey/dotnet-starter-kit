namespace Accounting.Application.Customers.Delete;

public class DeleteCustomerCommand(DefaultIdType id) : IRequest
{
    public DefaultIdType Id { get; set; } = id;
}
