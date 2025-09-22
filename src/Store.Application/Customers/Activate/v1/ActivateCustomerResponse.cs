namespace FSH.Starter.WebApi.Store.Application.Customers.Activate.v1;

/// <summary>
/// Response after activating a customer.
/// </summary>
/// <param name="Id">Activated customer identifier.</param>
public sealed record ActivateCustomerResponse(DefaultIdType Id);

