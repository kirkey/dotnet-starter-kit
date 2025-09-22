namespace FSH.Starter.WebApi.Store.Application.Customers.Activate.v1;

/// <summary>
/// Command to activate a customer account.
/// </summary>
/// <param name="Id">Customer identifier.</param>
public sealed record ActivateCustomerCommand(DefaultIdType Id) : IRequest<ActivateCustomerResponse>;

