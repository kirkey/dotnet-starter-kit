using MediatR;

namespace Accounting.Application.Customers.Create;

public record CreateCustomerRequest(
    string CustomerCode,
    string Name,
    string? Address = null,
    string? BillingAddress = null,
    string? ContactPerson = null,
    string? Email = null,
    string? Terms = null,
    string? RevenueAccountCode = null,
    string? RevenueAccountName = null,
    string? Tin = null,
    string? PhoneNumber = null,
    decimal CreditLimit = 0,
    string? Description = null,
    string? Notes = null) : IRequest<DefaultIdType>;
