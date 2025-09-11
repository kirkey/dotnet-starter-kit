using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.Warehouse.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.Warehouse.Features.Customers.Create.v1;

public sealed class CreateCustomerHandler(
    ILogger<CreateCustomerHandler> logger,
    [FromKeyedServices("warehouse")] IRepository<Customer> repository)
    : IRequestHandler<CreateCustomerCommand, CreateCustomerResponse>
{
    public async Task<CreateCustomerResponse> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        var entity = Customer.Create(request.FirstName, request.LastName, request.Phone, request.Email, request.Address, request.DateOfBirth);
        await repository.AddAsync(entity, cancellationToken).ConfigureAwait(false);
        await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        logger.LogInformation("customer created {CustomerId}", entity.Id);
        return new CreateCustomerResponse(entity.Id);
    }
}
