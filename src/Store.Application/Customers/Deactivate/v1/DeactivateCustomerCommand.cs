namespace FSH.Starter.WebApi.Store.Application.Customers.Deactivate.v1;

/// <summary>
/// Command to deactivate a customer account.
/// </summary>
/// <param name="Id">Customer identifier.</param>
public sealed record DeactivateCustomerCommand(DefaultIdType Id) : IRequest<DeactivateCustomerResponse>;

