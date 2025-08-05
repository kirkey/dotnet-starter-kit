using Accounting.Domain;
using Accounting.Application.Customers.Exceptions;
using FSH.Framework.Core.Persistence;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Accounting.Application.Customers.Delete;

public sealed class DeleteCustomerHandler(
    [FromKeyedServices("accounting")] IRepository<Customer> repository)
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
