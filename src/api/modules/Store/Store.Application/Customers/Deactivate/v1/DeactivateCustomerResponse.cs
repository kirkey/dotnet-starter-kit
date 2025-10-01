namespace FSH.Starter.WebApi.Store.Application.Customers.Deactivate.v1;

/// <summary>
/// Response after deactivating a customer.
/// </summary>
/// <param name="Id">Deactivated customer identifier.</param>
public sealed record DeactivateCustomerResponse(DefaultIdType Id);

