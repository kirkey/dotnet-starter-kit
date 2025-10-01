namespace FSH.Starter.WebApi.Store.Application.Customers.CreditLimit.v1;

/// <summary>
/// Response after changing a customer's credit limit.
/// </summary>
/// <param name="Id">Customer identifier.</param>
/// <param name="CreditLimit">New credit limit value.</param>
public sealed record ChangeCustomerCreditLimitResponse(DefaultIdType Id, decimal CreditLimit);

