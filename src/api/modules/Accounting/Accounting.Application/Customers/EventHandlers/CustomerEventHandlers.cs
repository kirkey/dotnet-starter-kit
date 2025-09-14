using Accounting.Domain.Events.Customer;

namespace Accounting.Application.Customers.EventHandlers;

public sealed class CustomerCreatedEventHandler(ILogger<CustomerCreatedEventHandler> logger)
    : INotificationHandler<CustomerCreated>
{
    public Task Handle(CustomerCreated notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Customer created: {CustomerId} - {CustomerCode}", 
            notification.Id, notification.CustomerCode);
        return Task.CompletedTask;
    }
}

public sealed class CustomerUpdatedEventHandler(ILogger<CustomerUpdatedEventHandler> logger)
    : INotificationHandler<CustomerUpdated>
{
    public Task Handle(CustomerUpdated notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Customer updated: {CustomerId}", notification.Customer.Id);
        return Task.CompletedTask;
    }
}
