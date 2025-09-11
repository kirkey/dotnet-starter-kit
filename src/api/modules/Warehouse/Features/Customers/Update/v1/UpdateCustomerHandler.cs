using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.Warehouse.Domain;
using FSH.Starter.WebApi.Warehouse.Exceptions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.Warehouse.Features.Customers.Update.v1;

public sealed class UpdateCustomerHandler(
    ILogger<UpdateCustomerHandler> logger,
    [FromKeyedServices("warehouse")] IRepository<Customer> repository)
    : IRequestHandler<UpdateCustomerCommand, UpdateCustomerResponse>
{
    public async Task<UpdateCustomerResponse> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
    {
        var entity = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        _ = entity ?? throw new CustomerNotFoundException(request.Id);
        entity.Update(request.FirstName, request.LastName, request.Phone, request.Email, request.Address, request.DateOfBirth, request.LoyaltyPoints, request.TotalSpent);
        await repository.UpdateAsync(entity, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("customer updated {CustomerId}", entity.Id);
        return new UpdateCustomerResponse(entity.Id);
    }
}

