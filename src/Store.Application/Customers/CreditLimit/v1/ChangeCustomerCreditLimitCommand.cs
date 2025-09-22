namespace FSH.Starter.WebApi.Store.Application.Customers.CreditLimit.v1;

/// <summary>
/// Command to change a customer's credit limit.
/// </summary>
/// <param name="Id">Customer identifier.</param>
/// <param name="NewCreditLimit">New credit limit (>= 0).</param>
public sealed record ChangeCustomerCreditLimitCommand(DefaultIdType Id, decimal NewCreditLimit) : IRequest<ChangeCustomerCreditLimitResponse>;

