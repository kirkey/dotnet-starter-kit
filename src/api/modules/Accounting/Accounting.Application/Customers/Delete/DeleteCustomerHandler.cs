using Accounting.Application.Customers.Exceptions;

namespace Accounting.Application.Customers.Delete;

public sealed class DeleteCustomerHandler(
    [FromKeyedServices("accounting:customers")] IRepository<Customer> repository)
    : IRequestHandler<DeleteCustomerRequest>
{
    public async Task Handle(DeleteCustomerRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var customer = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (customer == null) throw new CustomerNotFoundException(request.Id);

        await repository.DeleteAsync(customer, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);
    }
}
